using UnityEngine;
using UnityEngine.UI;

public class SelectModel : MonoBehaviour
{
    public GameObject model;
    public GameObject markSelected;

    private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameManager.Instance.pathPoint == model) markSelected.gameObject.SetActive(true);
        else markSelected.gameObject.SetActive(false);
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SelectThis);
    }

    private void SelectThis()
    {
        foreach(GameObject go in GameManager.Instance.customizeButtons)
        {
            go.gameObject.GetComponent<SelectModel>().markSelected.SetActive(false);
        }
        markSelected.gameObject.SetActive(true);
        GameManager.Instance.pathPoint = model; 
    }
}
