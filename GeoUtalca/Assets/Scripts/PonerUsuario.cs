using UnityEngine;

public class PonerUsuario : MonoBehaviour
{
    public TMPro.TextMeshProUGUI UsuarioText;

    private void Start()
    {
        UsuarioText.text = GameManager.Instance.usuario;
    }
}
