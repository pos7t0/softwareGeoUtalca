using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LocationData selectedLocation;

    public GameObject pathPoint;
    public List<GameObject> customizeButtons = new List<GameObject>();

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
    }
    private void Start()
    {
        foreach(GameObject go in customizeButtons)
        {
            GameObject newItem = Instantiate(go, ShopController.Instance.purchasedItemsContainer);
        }   
    }
    public void SetSelectedLocation (LocationData location)
    {
        selectedLocation = location;
        Debug.Log(selectedLocation.latitude +" , "+selectedLocation.longitude + " , " + selectedLocation.altitude);
    }

    public void GoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}



