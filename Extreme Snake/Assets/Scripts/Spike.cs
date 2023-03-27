using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioSource source;
    public AudioClip clip;

    // screen coords
    private Vector3 bot;

    // spikes fall downward
    private Vector3 direction = Vector3.down;

    // is the something happening?
    private bool actionPending = false;

    // Start is called before the first frame update
    void Start()
    {
        this.bot = Camera.main.ViewportToWorldPoint(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (!actionPending)
        {
            Vector3 currPosition = gameObject.transform.position;
            // check the boundries of the screen
            if (currPosition.y < bot.y + 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        if (!actionPending)
        {
            // move spike downward
            Vector3 prevSegmentPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(
                Mathf.Round(prevSegmentPosition.x),
                Mathf.Round(prevSegmentPosition.y) + direction.y,
                0.0f
                );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!actionPending)
        {
            string tag = other.gameObject.tag;
            if (tag == "Head" || tag == "Snake" || tag =="Apple")
            {
                actionPending = true;

                // disable object
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;

                // explosion
                Instantiate(
                    explosionPrefab,
                    gameObject.transform.position,
                    Quaternion.identity
                    );

                // play music
                source.PlayOneShot(clip);

                if (tag == "Head" || tag == "Snake")
                {
                    // decrease score
                    ScoreKeeper.AddToScore(-15.0f);
                }

                Invoke("CleanUp", 1.0f);
            }
        } 
    }

    void CleanUp()
    {
        Destroy(gameObject);
    }
}
