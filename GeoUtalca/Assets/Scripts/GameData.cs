using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Usuario
{
    public string nombre;
    public List<Cosmetico> cosmeticos;
    public List<Insignia> insignias;
    public string nombreCosmeticoSeleccionado;
    public string categoria;
    public int puntos;
}

[Serializable]
public class Juego
{
    public List<Lugar> lugares;
}

[Serializable]
public class Lugar
{
    public string nombre;
    public bool completado;
    public double[] ubicacion; // [latitud, longitud, altitud]
    public Insignia insignia;
}

[Serializable]
public class Cosmetico
{
    public string nombre;
    public int valor;
}

[Serializable]
public class Insignia
{
    public string detalles;
    public string rutaSprite;
}
