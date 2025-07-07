using UnityEngine;

public class LocationData
{
    public string name;
    public float latitude;
    public float longitude;
    public float altitude;

    public LocationData(string rawLine)
    {
        string[] parts = rawLine.Split(':');
        if(parts.Length != 2)
        {
            Debug.LogWarning("Invalid line format: " + rawLine);
            return;
        }

        name = parts[0].Trim();
        string[] coords = parts[1].Split(',');

        if(coords.Length != 3)
        {
            Debug.LogWarning("Invalid coord format: " + rawLine);
            return;
        }

        latitude = float.Parse(coords[0], System.Globalization.CultureInfo.InvariantCulture);
        longitude = float.Parse(coords[1], System.Globalization.CultureInfo.InvariantCulture);
        altitude = float.Parse(coords[2], System.Globalization.CultureInfo.InvariantCulture);
    }
}
