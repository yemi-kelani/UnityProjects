using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    private bool actionPending = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!actionPending)
        {
            actionPending = true;

            if (other.gameObject.tag == "Head")
            {
                // play music
                source.PlayOneShot(clip);
                
                // increase score
                ScoreKeeper.AddToScore(15.0f);
            }

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Invoke("CleanUp", 1.0f);
        }
    }

    void CleanUp()
    {
        Destroy(gameObject);
    }

    
}
