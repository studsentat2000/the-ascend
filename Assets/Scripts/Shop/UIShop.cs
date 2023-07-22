using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer customer;

    private GameObject health;
    private GameObject bombs;

    private bool inside;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
        health = GameObject.FindGameObjectWithTag("HealthUI");
        bombs = GameObject.FindGameObjectWithTag("BombsUI");
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CreateItemButton("sword", "Sword Upgrade", 25, 0);
        CreateItemButton("bomb", "Bomb Upgrade", 50, 1);
        CreateItemButton("fire", "Fire Upgrade", 40, 2);
        CreateItemButton("fireTimeout", "Shot Timeout", 60, 3);
        CreateItemButton("fireSpeed", "Fire Speed", 50, 4);
        Hide();
    }

    private void CreateItemButton(string itemType,string itemName, int itemCost, int positionIndex) {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 130f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, 100-shopItemHeight * positionIndex);

        shopItemTransform.Find("name").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("price").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () =>
        {
            TryBuyItem(itemType, itemCost);
        };
    }

    void TryBuyItem(string itemType, int cost) {
        if (customer.CanSpend(cost))
        {
            customer.BoughtItem(itemType, cost);
        }
    }

    public void Show(IShopCustomer customer) {
        this.customer = customer;
        gameObject.SetActive(true);
        health.gameObject.SetActive(false);
        bombs.gameObject.SetActive(false);
        inside = true;
    }

    public void Hide() { 
        gameObject.SetActive(false);
        health.gameObject.SetActive(true);
        bombs.gameObject.SetActive(true);
        inside = false;
    }
}
