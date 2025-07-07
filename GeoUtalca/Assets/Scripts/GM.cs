using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GM : MonoBehaviour
{
    public static GM Instance;

    public int money = 600;
    public TMP_Text moneyText;

    public Transform purchasedItemsContainer; // Contenedor visual en GridLayoutGroup

    // Lista de objetos comprados
    public List<GameObject> purchasedItems = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateMoneyUI();
    }

    public bool TryPurchase(int price)
    {
        if (money >= price)
        {
            money -= price;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("No hay suficiente dinero.");
            return false;
        }
    }

    public void AddPurchasedItem(GameObject item)
    {
        purchasedItems.Add(item);
        Debug.Log("Objeto agregado a la lista: " + item.name);

        // Crear una copia visual en el contenedor
        GameObject newItem = Instantiate(item, purchasedItemsContainer);
        newItem.SetActive(true);
    }

    void UpdateMoneyUI()
    {
        moneyText.text = money.ToString();
    }
}
