using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop_2 : MonoBehaviour
{
    public GameObject shopItemTemplate;
    public RectTransform content;
    public ScrollRect scrollRect;

    [SerializeField]
    public IShopCustomer customer;

    private GameObject health;
    private GameObject bombs;

    private void Awake()
    {
        shopItemTemplate.SetActive(false);
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

    private void CreateItemButton(string itemType, string itemName, int itemCost, int positionIndex)
    {
        GameObject shopItem = Instantiate(shopItemTemplate, content);
        shopItem.SetActive(true);

        RectTransform shopItemRectTransform = shopItem.GetComponent<RectTransform>();

        float shopItemHeight = 130f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        shopItemRectTransform.parent = scrollRect.transform;

        shopItem.transform.Find("name").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItem.transform.Find("price").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItem.GetComponent<Button_UI>().ClickFunc = () =>
        {
            TryBuyItem(itemType, itemCost);
        };
    }

    void TryBuyItem(string itemType, int cost)
    {
        if (customer.CanSpend(cost))
        {
            customer.BoughtItem(itemType, cost);
        }
    }

    public void Show(IShopCustomer customer)
    {
        this.customer = customer;
        gameObject.SetActive(true);
        health.gameObject.SetActive(false);
        bombs.gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        health.gameObject.SetActive(true);
        bombs.gameObject.SetActive(true);
    }
}
