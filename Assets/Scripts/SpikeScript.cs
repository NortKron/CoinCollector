using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    GameObject controller;

    void Start()
    {
        controller = GameObject.Find("Panel UI");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controller.SendMessage("FinishGame", false);
        }
    }
}
