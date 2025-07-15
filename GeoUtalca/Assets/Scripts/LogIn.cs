using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LogIn : MonoBehaviour
{
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContraseña;
    public TMP_InputField inputContraseñaDeNuevo;
    public TMP_Text m_warningUsers;
    private bool m_canRegister=false;
    private bool m_canLogIn=false;
    public void BotonRegistrar()
    {
        string usuario = inputUsuario.text;
        string contraseña = inputContraseña.text;
        string contraseñaNueva = inputContraseñaDeNuevo.text;
        int i= BDManager.Instance.RegistrarUsuario(usuario, contraseña, contraseñaNueva);
        switch (i)
        {
            case 0:
                m_warningUsers.text = "El usuario ya existe.";
                m_canRegister = false;
                break;
            case 1:
                m_warningUsers.text = "Contraseñas no coinciden";
                m_canRegister = false;
                break;
            case 2:
                m_warningUsers.text = "Contraseña muy corta";
                m_canRegister = false;
                break;
            case 3:
                m_warningUsers.text = "Falta mayúscula";
                m_canRegister = false;
                break;
            case 4:
                m_warningUsers.text = "Falta carácter especial";
                m_canRegister = false;
                break;
            case 5:
                m_warningUsers.text = "";
                m_canRegister = true;
                break;
        }
        if (m_canRegister)
        {
            SceneManager.LoadScene("Login");
        }   
    }

    public void BotonIniciarSesion()
    {
        string usuario = inputUsuario.text;
        string contraseña = inputContraseña.text;
        m_canLogIn= BDManager.Instance.LogIn(usuario, contraseña);
        if (m_canLogIn)
        {
            GameManager.Instance.usuario = usuario;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            m_warningUsers.text="Nombre o contraseña incorrecta";
    }

    public void ChangeScene( string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void GoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
