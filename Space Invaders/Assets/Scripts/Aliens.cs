using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aliens : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public Vector3 direction = Vector3.right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += direction * (movementSpeed * Time.deltaTime);
        Vector3 right = Camera.main.ViewportToWorldPoint(Vector3.right); //(1, 0, 0)
        Vector3 left = Camera.main.ViewportToWorldPoint(Vector3.zero);

        var alienTransforms = gameObject.GetComponent<Transform>();
        foreach (Transform at in alienTransforms)
        {
            if (direction.x == -1.0f && at.position.x <= left.x + 2.0f)
            {
                direction.x *= -1.0f;
                Vector3 oldPosition = gameObject.transform.position;
                gameObject.transform.position = new Vector3(oldPosition.x, (oldPosition.y - 0.2f), 0);
            }

            if (direction.x == 1.0f && at.position.x >= right.x - 2.0f)
            {
                direction.x *= -1.0f;
                Vector3 oldPosition = gameObject.transform.position;
                gameObject.transform.position = new Vector3(oldPosition.x, (oldPosition.y - 0.2f), 0);
            }
        }
    }
}
