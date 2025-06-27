using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR;

[RequireComponent(typeof(ARAnchorManager))]

public class ARRouteGenerator : MonoBehaviour
{
    [Header("Waypoints")]
    [Tooltip("Transform del punto de inicio en AR (por ejemplo un objeto hijo de XR Origin)")]
    public Transform startPoint;
    [Tooltip("Transform del punto de destino en AR")]
    public Transform endPoint;

    [Header("Route Settings")]
    [Tooltip("Prefab que se instanciará en cada punto de la ruta")]
    public GameObject routePrefab;
    [Range(1, 50)]
    [Tooltip("Número de marcadores intermedios a instanciar")]
    public int pointCount = 10;

    [Header("XR Origin (opcional)")]
    [Tooltip("Transform de tu XR Origin, para organizar la jerarquía")]
    public Transform xrOriginParent;

    // Interno: guardamos las instancias para poder borrarlas luego
    private readonly List<GameObject> _spawnedPoints = new List<GameObject>();

    /// <summary>
    /// Llama a este método (desde un UI Button) para generar o regenerar la ruta.
    /// </summary>
    public void GenerateRoute()
    {
        // 1) Eliminar la ruta anterior
        foreach (var go in _spawnedPoints)
        {
            if (go != null) DestroyImmediate(go);
        }
        _spawnedPoints.Clear();

        // 2) Validaciones mínimas
        if (startPoint == null || endPoint == null || routePrefab == null)
        {
            Debug.LogWarning("SimpleRouteGenerator: faltan referencias (start, end o prefab).");
            return;
        }

        // 3) Generar instancias entre start y end
        for (int i = 1; i <= pointCount; i++)
        {
            float t = (float)i / (pointCount + 1);
            Vector3 worldPos = Vector3.Lerp(startPoint.position, endPoint.position, t);

            var go = Instantiate(routePrefab, worldPos, Quaternion.identity,
                                 xrOriginParent != null ? xrOriginParent : null);

            _spawnedPoints.Add(go);
        }
    }

}
