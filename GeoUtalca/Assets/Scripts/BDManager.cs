using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

public class BDManager : MonoBehaviour
{
    public static BDManager Instance { get; private set; }
    private string rutaArchivo => Path.Combine(Application.persistentDataPath, "Saves/SaveData.txt");

    [System.Serializable]
    public class Usuario
    {
        public string nombreUsuario;
        public string contraseña;
    }

    [System.Serializable]
    public class ListaUsuarios
    {
        public List<Usuario> usuarios = new List<Usuario>();
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

    public List<Usuario> CargarUsuarios()
    {
        if (!File.Exists(rutaArchivo))
        {
            Debug.LogWarning("Archivo de usuarios no encontrado en: " + rutaArchivo);
            return new List<Usuario>();
        }

        string json = File.ReadAllText(rutaArchivo);
        ListaUsuarios contenedor = JsonUtility.FromJson<ListaUsuarios>(json);
        return contenedor.usuarios;
    }

    public void GuardarUsuarios(List<Usuario> lista)
    {
        ListaUsuarios contenedor = new ListaUsuarios { usuarios = lista };
        string json = JsonUtility.ToJson(contenedor, true);
        File.WriteAllText(rutaArchivo, json);
        Debug.Log("Usuarios guardados en: " + rutaArchivo);
    }

    public int RegistrarUsuario(string nombreUsuario, string contraseña, string contraseñaNueva)
    {
        List<Usuario> usuarios = CargarUsuarios();

        if (usuarios.Exists(u => u.nombreUsuario == nombreUsuario))
        {
            return 0; // Usuario ya existe
        }

        if (contraseña != contraseñaNueva)
        {
            return 1; // Contraseñas no coinciden
        }

        if (contraseña.Length < 8)
        {
            return 2; // Contraseña muy corta
        }

        if (!Regex.IsMatch(contraseña, @"[A-Z]"))
        {
            return 3; // Falta mayúscula
        }

        if (!Regex.IsMatch(contraseña, @"[!@#$%^&*(),.?""{}|<>]"))
        {
            return 4; // Falta carácter especial
        }

        usuarios.Add(new Usuario { nombreUsuario = nombreUsuario, contraseña = contraseña });
        GuardarUsuarios(usuarios);

        return 5; // Registro exitoso
    }

    public bool LogIn(string nombreUsuario, string contraseña)
    {
        List<Usuario> usuarios = CargarUsuarios();

        Usuario usuario = usuarios.Find(u => u.nombreUsuario == nombreUsuario && u.contraseña == contraseña);
        if (usuario != null)
        {
            Debug.Log($"Sesión iniciada con éxito para '{nombreUsuario}'.");
            return true;
        }
        else
        {
            Debug.LogWarning("Nombre de usuario o contraseña incorrectos.");
            return false;
        }
    }
}
