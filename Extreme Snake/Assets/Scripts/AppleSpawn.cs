using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawn : MonoBehaviour
{
    // Apple Prefab
    public Apple applePrefab;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 random = randomSpot();
        Instantiate(applePrefab, random, Quaternion.identity, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // if there is no active apple, spawn one
        if(transform.childCount == 0)
        {
            Vector3 random = randomSpot();
            Instantiate(applePrefab, random, Quaternion.identity, gameObject.transform);
        }
    }

    private Vector3 randomSpot()
    {
        Vector2 start = Camera.main.ScreenToWorldPoint(Vector2.zero); // where the camera starts
        Vector2 end = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float yCoord = Mathf.Round(Random.Range(start.y + 1, end.y - 1));
        float xCoord = Mathf.Round(Random.Range(start.x + 1, end.x - 1));

        return new Vector3(xCoord, yCoord, 1);
    }
}
