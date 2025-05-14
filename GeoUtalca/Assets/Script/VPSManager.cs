using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;

public class VPSManager : MonoBehaviour
{
    [SerializeField]private AREarthManager m_earthManager; 
    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }
    [Serializable]
    public struct EarthPosition
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
    }
    [SerializeField] private ARAnchorManager m_arAnchorManager;
    [SerializeField] private List<GeospatialObject> m_geospatialObjects = new List<GeospatialObject>();


    private void Start()
    {
        VerifyGeospatialSupport();
    }

    private void VerifyGeospatialSupport()
    {
        var result = m_earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        switch (result)
        {
            case FeatureSupported.Supported:
                ShowDebug.Instance.ShowMessage("Ready to use VPS");
                Debug.Log("Ready to use VPS");
                PlaceObject();
                break;
            case FeatureSupported.Unknown:
                Debug.Log("Unknow");
                ShowDebug.Instance.ShowMessage("Unknow");
                Invoke("VerifyGeospatialSupport", 5.0f);
                break;
            case FeatureSupported.Unsupported:
                ShowDebug.Instance.ShowMessage("VPS Unsupported");
                Debug.Log("VPS Unsupported");
                break;
            default:
                break;
        }
    }

    private void PlaceObject()
    {
        if (m_earthManager.EarthTrackingState==TrackingState.Tracking)
        {
            var geospatialPose = m_earthManager.CameraGeospatialPose;

            foreach (var obj in m_geospatialObjects)
            {
                var earthPosition = obj.EarthPosition;
                var objAnchor = ARAnchorManagerExtensions.AddAnchor(m_arAnchorManager, earthPosition.Latitude, earthPosition.Longitude, earthPosition.Altitude, Quaternion.identity);
                Instantiate(obj.ObjectPrefab,objAnchor.transform);
            }


        }
        else if (m_earthManager.EarthTrackingState==TrackingState.None)
        {
            Invoke("PlaceObject", 5.0f);
        }
    }
}
