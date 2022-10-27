using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListCoins : MonoBehaviour
{
    private List<CoinScript> listCoins;
    private List<CoinScript> listTemp;
    private GameObject controller;

    void Start()
    {
        listCoins = new List<CoinScript>();

        controller = GameObject.Find("Panel UI");

        foreach (CoinScript child in transform.GetComponentsInChildren<CoinScript>())
        {
            listCoins.Add(child);
            child.index = listCoins.Count - 1;
        }
    }

    public void ActivatingCoins()
    {
        foreach(var coin in listCoins)
        {
            coin.gameObject.SetActive(true);
        }
    }

    public void InactiveCoinByIndex(int index)
    {
        listCoins[index].gameObject.SetActive(false);
        listTemp = listCoins.Where(x => x.gameObject.activeSelf == true).ToList();

        if (listTemp.Count == 0)
        {
            controller.SendMessage("FinishGame", true);
        }
    }
}
