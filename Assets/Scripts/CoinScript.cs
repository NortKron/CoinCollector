using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    GameObject controller;
    public int index = 0;

    void Start()
    {
        controller = GameObject.Find("Panel UI");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller.SendMessage("IncCoins", index);
        }
    }
}
