using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float movementSpeed = 10.0f; //****
    public Missile misslePrefab;
    private float waitTime = 0.0f;
    private float coolDown = 1.5f;
    private bool reloading = false;
    public AudioSource source;
    public AudioClip clip;
    public GameObject ExplosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 left = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 right = Camera.main.ViewportToWorldPoint(Vector3.right);

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && gameObject.transform.position.x >= left.x + 6.0f)
        {
            Vector3 oldPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(oldPosition.x + (-3.0f * (movementSpeed * Time.deltaTime)), oldPosition.y, 0);
        } 
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && gameObject.transform.position.x <= right.x - 6.0f)
        {
            Vector3 oldPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(oldPosition.x + (3.0f * (movementSpeed * Time.deltaTime)), oldPosition.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (waitTime >= coolDown)
            {
                Fire();
                waitTime = 0.0f;
            }
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
            if (other.gameObject.tag == "EnemyMissile")
            {
                source.PlayOneShot(clip);
                Vector3 BoomSpot = other.gameObject.transform.position;
                Destroy(other.gameObject);
                Instantiate(ExplosionPrefab, BoomSpot, Quaternion.identity, gameObject.transform.parent);
                ScoreKeeper.SetScore(0.0f);
                Invoke("Reload", 3.0f);
                reloading = true;
            }
        }
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
