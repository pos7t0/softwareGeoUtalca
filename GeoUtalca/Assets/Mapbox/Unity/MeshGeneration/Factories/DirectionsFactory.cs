
namespace Mapbox.Unity.MeshGeneration.Factories
{
    using UnityEngine;
    using Mapbox.Directions;
    using System.Collections.Generic;
    using System.Linq;
    using Mapbox.Unity.Map;
    using Data;
    using Modifiers;
    using Mapbox.Utils;
    using Mapbox.Unity.Utilities;
    using System.Collections;

    public class DirectionsFactory : MonoBehaviour
    {
        public List<Vector2d> samplePoints;
        public GameObject puntoRA;

        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        MeshModifier[] MeshModifiers;
        [SerializeField]
        Material _material;

        [SerializeField]
        Transform[] _waypoints;
        [SerializeField] private Vector2 puntoFijo;
        private List<Vector3> _cachedWaypoints;

        [HideInInspector]
        public List<Vector2d> geoRoutePoints = new List<Vector2d>(); // coordenadas GPS de la ruta

        [SerializeField]
        [Range(1, 10)]
        private float UpdateFrequency = 2;

        private Directions _directions;
        private int _counter;

        GameObject _directionsGO;
        private bool _recalculateNext;

    


        protected virtual void Awake()
        { 
            LocationData locdat = GameManager.Instance.selectedLocation;

            /*
            Vector3 newWorldPos = Conversions.GeoToWorldPosition(
                puntoFijo.x, puntoFijo.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
            */

            // Convertir latitud/longitud a posición en el mundo
            Vector3 newWorldPos = Conversions.GeoToWorldPosition(
                locdat.latitude, locdat.longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();


            _waypoints[0].position = newWorldPos; // el primer waypoint se actualiza
            if (_map == null)
            {
                _map = FindObjectOfType<AbstractMap>();
            }
            _directions = MapboxAccess.Instance.Directions;
            _map.OnInitialized += Query;
            _map.OnUpdated += Query;
        }

        public void Start()
        {

            _cachedWaypoints = new List<Vector3>(_waypoints.Length);
            foreach (var item in _waypoints)
            {
                _cachedWaypoints.Add(item.position);
            }
            _recalculateNext = false;

            foreach (var modifier in MeshModifiers)
            {
                modifier.Initialize();
            }

            StartCoroutine(QueryTimer());
        }

        protected virtual void OnDestroy()
        {
            _map.OnInitialized -= Query;
            _map.OnUpdated -= Query;
        }

        void Query()
        {
            var count = _waypoints.Length;
            var wp = new Vector2d[count];
            for (int i = 0; i < count; i++)
            {
                wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
            }
            var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
            _directionResource.Steps = true;
            _directions.Query(_directionResource, HandleDirectionsResponse);
        }

        public IEnumerator QueryTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateFrequency);
                for (int i = 0; i < _waypoints.Length; i++)
                {
                    if (_waypoints[i].position != _cachedWaypoints[i])
                    {
                        _recalculateNext = true;
                        _cachedWaypoints[i] = _waypoints[i].position;
                    }
                }

                if (_recalculateNext)
                {
                    Query();
                    _recalculateNext = false;
                }
            }
        }

        void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (response == null || null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }

            var meshData = new MeshData();
            var dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
            }

            var feat = new VectorFeatureUnity();
            feat.Points.Add(dat);

            foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
            {
                mod.Run(feat, meshData, _map.WorldRelativeScale);
            }

            CreateGameObject(meshData);

            var pathPoints = response.Routes[0].Geometry;
            geoRoutePoints.Clear();
            geoRoutePoints.AddRange(pathPoints); // guarda la ruta completa
            int total = pathPoints.Count;
            if (total < 4)
            {
                Debug.LogWarning("Not enough points to sample.");
                return;
            }

            samplePoints.Clear();
            samplePoints.Add(pathPoints[total / 4]);
            samplePoints.Add(pathPoints[total / 2]);
            samplePoints.Add(pathPoints[3 * total / 4]);


            foreach (var geoPoint in samplePoints)
            {
                Vector3 worldPos = Conversions.GeoToWorldPosition(
                    geoPoint.x, geoPoint.y, _map.CenterMercator, _map.WorldRelativeScale
                ).ToVector3xz();

                Instantiate(GameManager.Instance.pathPoint, worldPos, Quaternion.identity);
            }
        }

        GameObject CreateGameObject(MeshData data)
        {
            if (_directionsGO != null)
            {
                _directionsGO.Destroy();
            }
            _directionsGO = new GameObject("direction waypoint " + " entity");
            var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
            mesh.subMeshCount = data.Triangles.Count;

            mesh.SetVertices(data.Vertices);
            _counter = data.Triangles.Count;
            for (int i = 0; i < _counter; i++)
            {
                var triangle = data.Triangles[i];
                mesh.SetTriangles(triangle, i);
            }

            _counter = data.UV.Count;
            for (int i = 0; i < _counter; i++)
            {
                var uv = data.UV[i];
                mesh.SetUVs(i, uv);
            }

            mesh.RecalculateNormals();
            _directionsGO.AddComponent<MeshRenderer>().material = _material;
            return _directionsGO;
        }

        public void UpdateMovingWaypoint(double latitude, double longitude)
        {
            if (_waypoints.Length < 2)
            {
                Debug.LogWarning("Se requieren al menos 2 waypoints (inicio y destino).");
                return;
            }

            Vector3 newWorldPos = Conversions.GeoToWorldPosition(
                latitude, longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();

            _waypoints[1].position = newWorldPos; // el segundo waypoint se actualiza
        }

    }



}
