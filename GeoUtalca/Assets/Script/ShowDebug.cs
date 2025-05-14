using UnityEngine;
using TMPro;

public class ShowDebug : MonoBehaviour
{
    public static ShowDebug Instance { get; private set; }

    [SerializeField] private TMP_Text text;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowMessage(string message)
    {
        if (text != null)
            text.text = message;
    }


}
