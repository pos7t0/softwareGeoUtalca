using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject prefabCosmetico3D;
    public Transform contenedor3D;
    public float separacion = 2f; // separación entre cada cubo

    public string adquirido;

    private void Start()
    {
        var lista = GameManager.Instance.CargarCosmeticos();

        for (int i = 0; i < lista.Count; i++)
        {
            var c = lista[i];

            // Instanciar y posicionar con separación
            Vector3 posicion = new Vector3(i * separacion, 0, 0); // separados en X
            GameObject go = Instantiate(prefabCosmetico3D, posicion, Quaternion.identity, contenedor3D);

            // Cargar textura
            Texture2D textura = Resources.Load<Texture2D>(c.image);
            if (textura != null)
            {
                Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.mainTexture = textura;
                go.GetComponent<Renderer>().material = mat;
            }
            else
            {
                Debug.LogWarning("No se encontró la textura: " + c.image);
            }

            go.name = c.nombre;
        }
    }


    private void Update()
    {
        GameManager.Instance.MarcarComoAdquirido(adquirido);
    }
}
