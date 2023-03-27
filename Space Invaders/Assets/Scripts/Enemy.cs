using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public float movementSpeed = 4.0f;
    public EnemyMissile misslePrefab;
    private float waitTime = 0.0f;
    private float coolDown = 4.0f;
    private bool reloading = false;
    public AudioSource source;
    public AudioClip clip;
    public GameObject ExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < player.transform.position.x)
        {
            Vector3 oldPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(oldPosition.x + (1.0f * (movementSpeed * Time.deltaTime)), oldPosition.y, 0);
            if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 0.005f)
            {
                gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, 0);
            }
        } else if (gameObject.transform.position.x > player.transform.position.x)
        {
            Vector3 oldPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(oldPosition.x + (-1.0f * (movementSpeed * Time.deltaTime)), oldPosition.y, 0);
            if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 0.005f)
            {
                gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, 0);
            }
        }

        if (waitTime >= coolDown && !reloading)
        {
            Fire();
            waitTime = 0.0f;
        }

        waitTime += Time.deltaTime;
    }

    private void Fire()
    {
        // instantiate the missile
        Instantiate(misslePrefab, gameObject.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!reloading)
        {
            if (other.gameObject.tag == "Missile")
            {
                source.PlayOneShot(clip);
                Vector3 BoomSpot = other.gameObject.transform.position;
                Destroy(other.gameObject);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Instantiate(ExplosionPrefab, BoomSpot, Quaternion.identity, gameObject.transform.parent);
                Invoke("Reload", 4.0f);
                ScoreKeeper.AddToScore(100.0f);
                reloading = true;
            }
        }
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
