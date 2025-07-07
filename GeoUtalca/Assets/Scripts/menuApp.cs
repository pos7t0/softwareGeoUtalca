using System.Collections;
using UnityEngine;

public class menuApp : MonoBehaviour
{
    public GameObject canvasMenu;
    public GameObject LocationMenu;
    public GameObject SavedLocationMenu;
    public GameObject ShopMenu;

    public GameObject Perfil;
    public GameObject Insignias;
    public GameObject Cosmetics;
    public GameObject About;

    public float slideDuration = 0.5f; // duración del deslizamiento en segundos
    public Vector3 hiddenOffset = new Vector3(0, -10, 0); // cuánto se desplaza hacia abajo
    private Vector3 originalPosition;
    private bool isSliding = false;

    void Start()
    {
        if (canvasMenu != null)
        {
            originalPosition = canvasMenu.transform.localPosition;
            canvasMenu.transform.localPosition = originalPosition + hiddenOffset;
            canvasMenu.SetActive(false);
        }
    }

    public void Activate()
    {
        if (canvasMenu != null && !canvasMenu.activeSelf && !isSliding)
        {
            canvasMenu.SetActive(true);
            StartCoroutine(SlideIn());
        }

        // Desactivar todos primero
        LocationMenu.SetActive(false);
        SavedLocationMenu.SetActive(false);
        ShopMenu.SetActive(false);

        Perfil.SetActive(false);
        Insignias.SetActive(false);
        Cosmetics.SetActive(false);
        About.SetActive(false);
    }

    public void Close()
    {
        if (canvasMenu.activeSelf && !isSliding)
        {
            StartCoroutine(SlideOut());
        }
    }

    IEnumerator SlideIn()
    {
        isSliding = true;

        Vector3 startPos = canvasMenu.transform.localPosition;
        Vector3 targetPos = originalPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            canvasMenu.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasMenu.transform.localPosition = targetPos;
        isSliding = false;
    }

    IEnumerator SlideOut()
    {
        isSliding = true;

        Vector3 startPos = canvasMenu.transform.localPosition;
        Vector3 targetPos = originalPosition + hiddenOffset;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            canvasMenu.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasMenu.transform.localPosition = targetPos;
        canvasMenu.SetActive(false);
        isSliding = false;
    }

    public void ShowMenu(string menuName)
    {
        Activate();

        // Activar el seleccionado
        switch (menuName)
        {
            case "Location":
                LocationMenu.SetActive(true);
                break;
            case "SavedLocation":
                SavedLocationMenu.SetActive(true);
                break;
            case "Shop":
                ShopMenu.SetActive(true);
                break;
            case "Perfil":
                Perfil.SetActive(true);
                break;
            case "Insignias":
                Insignias.SetActive(true);
                break;
            case "Cosmeticos":
                Cosmetics.SetActive(true);
                break;
            case "About":
                About.SetActive(true);
                break;
            default:
                Debug.LogWarning("Menú no reconocido: " + menuName);
                break;
        }
    }
}
