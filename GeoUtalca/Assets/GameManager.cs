using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private string rutaArchivo => Path.Combine(Application.dataPath, "Save/cosmeticos.txt");

    private LocationData selectedLocation;

    [System.Serializable]
    public class Cosmetico
    {
        public string nombre;
        public int valor;
        public bool adquirido;
        public string image;
    }

    [System.Serializable]
    public class ListaCosmeticos
    {
        public List<Cosmetico> cosmeticos = new List<Cosmetico>();
    }
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Crear carpeta si no existe
        Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
    }


    public List<Cosmetico> CargarCosmeticos()
    {
        if (!File.Exists(rutaArchivo))
        {
            Debug.LogWarning("Archivo de cosméticos no encontrado en: " + rutaArchivo);
            return new List<Cosmetico>();
        }

        string json = File.ReadAllText(rutaArchivo);
        ListaCosmeticos contenedor = JsonUtility.FromJson<ListaCosmeticos>(json);
        return contenedor.cosmeticos;
    }

    public void MarcarComoAdquirido(string nombreCosmetico)
    {
        List<GameManager.Cosmetico> cosmeticos = GameManager.Instance.CargarCosmeticos();

        Cosmetico cos = cosmeticos.Find(c => c.nombre == nombreCosmetico);
        if (cos != null)
        {
            cos.adquirido = true;
            GameManager.Instance.GuardarCosmeticos(cosmeticos);
            Debug.Log($"Cosmético '{nombreCosmetico}' marcado como adquirido.");
        }
        else
        {
            Debug.LogWarning("No se encontró el cosmético: " + nombreCosmetico);
        }
    }

    public void GuardarCosmeticos(List<Cosmetico> lista)
    {
        ListaCosmeticos contenedor = new ListaCosmeticos { cosmeticos = lista };
        string json = JsonUtility.ToJson(contenedor, true);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Cosméticos guardados en: " + rutaArchivo);
    }

    public void SetSelectedLocation (LocationData location)
    {
        selectedLocation = location;
        Debug.Log(selectedLocation.latitude +" , "+selectedLocation.longitude + " , " + selectedLocation.altitude);
    }


    //public class Insignia
    //{
    //    public string name;
    //    public string detalles;
    //    public string rutaSprite;
    //}
    //public class Usuario
    //{
    //    public string nombre;
    //    public List<Cosmetico> cosmeticos;
    //    public List<Insignia> insignias;
    //    public string nombreCosmeticoSeleccionado;
    //    public string categoria;
    //    public int puntos;
    //}
    //public class Lugar
    //{
    //    public string nombre;
    //    public bool completado;
    //    public double[] ubicacion; // [latitud, longitud, altitud]
    //    public Insignia insignia;
    //}

    //public class Juego
    //{
    //    public List<Lugar> lugares;
    //}
}



