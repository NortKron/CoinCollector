using System.Collections.Generic;
using UnityEngine;

public class ListSpikes : MonoBehaviour
{
    private List<SpikeScript> listSpikes;

    void Start()
    {
        listSpikes = new List<SpikeScript>();

        foreach (SpikeScript child in transform.GetComponentsInChildren<SpikeScript>())
        {
            listSpikes.Add(child);
        }

        RandomRotateSpikes();
    }

    public void RandomRotateSpikes()
    {
        foreach(SpikeScript spike in listSpikes)
        {
            spike.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }
}
