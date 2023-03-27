using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawn : MonoBehaviour
{
    // spike prefab
    public Spike spikePrefab;

    // Update is called once per frame
    void Update()
    {
        // if there is no active spike, spawn one
        if (transform.childCount < 2)
        {
            Vector3 random = randomSpot();
            Instantiate(spikePrefab, random, Quaternion.identity, gameObject.transform);
        }
    }

    // generate random x coord for spike to fall
    private Vector3 randomSpot()
    {
        Vector2 start = Camera.main.ScreenToWorldPoint(Vector2.zero); // where the camera starts
        Vector2 end = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float xCoord = Mathf.Round(Random.Range(start.x + 1, end.x - 1));

        return new Vector3(xCoord, Camera.main.ViewportToWorldPoint(Vector3.up).y, 0.0f);
    }
}
