using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryController : MonoBehaviour
{
    public Transform resultsParent;
    public GameObject resultItemPrefab;

    public List<LocationData> allLocations = new List<LocationData>();
    private List<GameObject> spawnedItems = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<LocationData> history = GameManager.Instance.locationHistory;

        if (history == null || history.Count == 0)
        {
            Debug.Log("No location history to display.");
            return;
        }

        history.Sort(CompareLocationsSmart);

        foreach (LocationData item in history)
        {
            GameObject newItem = Instantiate(resultItemPrefab, resultsParent);
            newItem.GetComponentInChildren<TMP_Text>().text = item.name;

            Button button = newItem.GetComponent<Button>();
            if (button != null)
            {
                LocationData capturedItem = item;
                button.onClick.AddListener(() => GameManager.Instance.SetSelectedLocation(capturedItem));
                button.onClick.AddListener(() => GameManager.Instance.GoNextScene());
            }

            spawnedItems.Add(newItem);
        }
    }

    private int CompareLocationsSmart(LocationData a, LocationData b)
    {
        string nameA = a.name;
        string nameB = b.name;

        int alphaCompare = string.Compare(nameA, nameB, System.StringComparison.OrdinalIgnoreCase);
        if (alphaCompare != 0)
            return alphaCompare;

        int numberA = ExtractTrailingNumber(nameA);
        int numberB = ExtractTrailingNumber(nameB);

        return numberA.CompareTo(numberB);
    }

    private int ExtractTrailingNumber(string input)
    {
        var match = System.Text.RegularExpressions.Regex.Match(input, @"(\d+)$");
        if (match.Success)
        {
            int.TryParse(match.Value, out int number);
            return number;
        }
        return 0;
    }

    void ClearResults()
    {
        foreach (GameObject obj in spawnedItems)
            Destroy(obj);
        spawnedItems.Clear();
    }
}
