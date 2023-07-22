using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ShopCollider : MonoBehaviour
{
    public ListController shop;

    private bool show = false;
    IShopCustomer customer;
    private bool openShop = false;
    private bool canShop = false;
    MovePlayerWithKeyboard player;
    private GameObject shield;


    private void Start()
    {
        //shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<UIShop_2>();
        shield = GameObject.FindGameObjectWithTag("ShopShield");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayerWithKeyboard>();
        shield.SetActive(false);
    }

    private void Update()
    {
        
        if (show && canShop)
        {
            if (!openShop && Input.GetKeyDown("g"))
            {
                Debug.Log("!openShop");
                openShop = true;
            }
            else if (openShop && Input.GetKeyDown("g")) 
            {
                Debug.Log("openShop");
                openShop = false;
            }
            if (openShop)
                shop.Show(customer);
            else if (!openShop)
                shop.Hide();
        }
        else 
        {
            shop.Hide();
        }
        if (player.bossIndex >= 1)
        {
            canShop = true;

            shield.SetActive(true);
        }
    }

    void OnShop() {
        Debug.Log("OnShop");

        if (openShop)
        {
            openShop = false;
        }
        else {
            openShop = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered Trigger shop");
        if (collision.GetComponent<IShopCustomer>() != null) {
            customer = collision.GetComponent<IShopCustomer>();
            //shop.Show(customer);
            show = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.GetComponent<IShopCustomer>() != null)
        {
            customer = collision.GetComponent<IShopCustomer>();
            show = false;
            shop.Hide();
        }
    }
}
