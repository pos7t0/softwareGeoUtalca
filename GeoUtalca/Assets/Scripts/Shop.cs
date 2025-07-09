using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int price = 10;
    public Button button;
    public GameObject itemToPurchase; // Referencia al objeto que se compra
    public TMP_Text purchasedText; // Texto a mostrar al comprar

    private void Start()
    {
        button.onClick.AddListener(AttemptPurchase);
    }

    void AttemptPurchase()
    {
        if (ShopController.Instance.TryPurchase(price))
        {
            Debug.Log("Compra realizada por $" + price);
            ShopController.Instance.AddPurchasedItem(itemToPurchase);

            button.interactable = false; // Desactiva el botón
            purchasedText.text = "Comprado";
        }
    }
}