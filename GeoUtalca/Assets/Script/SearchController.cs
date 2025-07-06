using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchController : MonoBehaviour
{
    public TMP_InputField searchInput;
    public Transform resultsParent;
    public GameObject resultItemPrefab;

    public List<LocationData> allLocations = new List<LocationData>();
    private List<GameObject> spawnedItems = new List<GameObject>();

    void Start()
    {
        LoadLocations();
        searchInput.onValueChanged.AddListener(OnSearchChanged);
        OnSearchChanged("");
    }

    private void LoadLocations()
    {
        TextAsset itemData = Resources.Load<TextAsset>("Locations");
        if (itemData != null)
        {
            string[] lines = itemData.text.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                LocationData loc = new LocationData(line.Trim());
                if (!string.IsNullOrEmpty(loc.name)) allLocations.Add(loc);
            }
        }
        else
        {
            Debug.LogError("Locations.txt not found in Resources folder!");
        }
    }

    void OnSearchChanged(string search)
    {
        ClearResults();

        foreach (LocationData item in allLocations)
        {
            if (string.IsNullOrEmpty(search) || item.name.ToLower().Contains(search.ToLower()))
            {
                GameObject newItem = Instantiate(resultItemPrefab, resultsParent);
                newItem.GetComponentInChildren<TMP_Text>().text = item.name;
                spawnedItems.Add(newItem);
            }
        }
    }

    void ClearResults()
    {
        foreach (GameObject obj in spawnedItems)
            Destroy(obj);
        spawnedItems.Clear();
    }
}
