using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            allLocations.Sort(CompareLocationsSmart);
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

                Button button = newItem.GetComponentInChildren<Button>();
                if(button != null)
                {
                    LocationData capturedItem = item;
                    button.onClick.AddListener(() => GameManager.Instance.SetSelectedLocation(capturedItem));
                }

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

    private int CompareLocationsSmart(LocationData a, LocationData b)
    {
        string nameA = a.name;
        string nameB = b.name;

        // alphabetically
        int alphaCompare = string.Compare(nameA, nameB, System.StringComparison.OrdinalIgnoreCase);
        if (alphaCompare != 0)
            return alphaCompare;

        // trailing numbers
        int numberA = ExtractTrailingNumber(nameA);
        int numberB = ExtractTrailingNumber(nameB);

        return numberA.CompareTo(numberB);
    }

    private int ExtractTrailingNumber(string input)
    {
        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, @"\d+");
        if (match.Success)
        {
            int.TryParse(match.Value, out int number);
            return number;
        }
        return 0; // if no number found
    }
}
