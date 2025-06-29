using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using TMPro;

public class SimpleGeospatialController : MonoBehaviour
{   
    
    
    public AREarthManager EarthManager;
    public ARAnchorManager AnchorManager;
    public TMP_Text DebugText;
    
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

    [SerializeField] private List<GeospatialObject> m_geospatialObjects = new List<GeospatialObject>();
    

    void Update()
    {
        if (EarthManager == null || DebugText == null)
        {
            return;
        }

        var earthState = EarthManager.EarthState;
        var trackingState = EarthManager.EarthTrackingState;

        var pose = earthState == EarthState.Enabled && trackingState == TrackingState.Tracking
            ? EarthManager.CameraGeospatialPose
            : new GeospatialPose();

        DebugText.text =
            $"EarthState: {earthState}\n" +
            $"TrackingState: {trackingState}\n" +
            $"Latitude: {pose.Latitude:F6}\n" +
            $"Longitude: {pose.Longitude:F6}\n" +
            $"Altitud: {pose.Altitude:F2}m\n" +
            $"Orientation Yaw Accuracy: {pose.OrientationYawAccuracy:F2}°";
    }

    public void PlaceObject()
    {
        if (EarthManager.EarthTrackingState == TrackingState.Tracking)
        {

            var geospatialPose = EarthManager.CameraGeospatialPose;

            foreach (var obj in m_geospatialObjects)
            {
                var earthPosition = obj.EarthPosition;
                var objAnchor = ARAnchorManagerExtensions.AddAnchor(AnchorManager, earthPosition.Latitude, earthPosition.Longitude, earthPosition.Altitude, Quaternion.identity);
                Instantiate(obj.ObjectPrefab, objAnchor.transform);
            }


        }
        else if (EarthManager.EarthTrackingState == TrackingState.None)
        {

        }
    }
}
