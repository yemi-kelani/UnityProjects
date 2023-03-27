using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;
    private bool reloading = false;
    public GameObject ExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!reloading)
        {
            if (other.gameObject.tag == "EndIWall")
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "Missile")
            {
                source.PlayOneShot(clip);
                Vector3 BoomSpot = other.gameObject.transform.position;
                Destroy(other.gameObject);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(ExplosionPrefab, BoomSpot, Quaternion.identity, gameObject.transform.parent);
                Invoke("Destruct", 0.1f);
                ScoreKeeper.AddToScore(10.0f);
                reloading = true;
                // Score system
            }
            else if (other.gameObject.tag == "PlayerIWall")
            {
                ScoreKeeper.SetScore(0.0f);
            }
        }
    }

    void Destruct()
    {
        Destroy(gameObject);
    }
}
