using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    public GameObject shopShield;

    private IShopCustomer customer;

    private GameObject health;
    public GameObject bombs;

    // Start is called before the first frame update
    void Start()
    {
        CreateItemButton("sword", "Sword Upgrade", 25, 0);
        CreateItemButton("bomb", "Bomb Upgrade", 50, 1);
        CreateItemButton("fire", "Fire Upgrade", 40, 2);
        CreateItemButton("fireTimeout", "Shot Timeout", 60, 3);
        CreateItemButton("fireSpeed", "Fire Speed", 50, 4);
        Hide();
    }

    private void Awake()
    {
        ListItemPrefab.SetActive(false);
        health = GameObject.FindGameObjectWithTag("HealthUI");
        bombs = GameObject.FindGameObjectWithTag("BombsUI");
        
        DontDestroyOnLoad(this.gameObject);
    }


    private void CreateItemButton(string itemType, string itemName, int itemCost, int positionIndex)
    {
        GameObject shopItem = Instantiate(ListItemPrefab) as GameObject;
        shopItem.SetActive(true);

        shopItem.transform.Find("name").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItem.transform.Find("price").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItem.transform.parent = ContentPanel.transform;
        shopItem.transform.localScale = Vector3.one;

        shopItem.GetComponent<Button_UI>().ClickFunc = () =>
        {
            TryBuyItem(itemType, itemCost);
            Debug.Log("clicked");
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
