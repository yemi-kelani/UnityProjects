using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Head : MonoBehaviour
{
    //Explosion prefab
    public GameObject explosionPrefab;

    //Snake prefab
    public Snake snakePrefab;

    // Audio
    public AudioSource source;
    public AudioClip clipLoss;
    public AudioClip clipWin;
    public AudioClip clipFast;

    // An array of Snake body segments gameObjects
    private List<Transform> segmentTransforms = new List<Transform>();
    private List<Snake> segments = new List<Snake>();

    // screen coords
    private Vector3 left;
    private Vector3 right;
    private Vector3 top;
    private Vector3 bot;

    // snake direction - initially moves to the left
    private Vector3 direction = Vector3.left;

    // is the game reloading?
    private bool reloading = false;

    // player action
    private bool gottaGoFast = false;
    private float coolDown = 0.0f;
    private float waitTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        segmentTransforms.Add(gameObject.transform);
        this.left = Camera.main.ViewportToWorldPoint(Vector3.zero);
        this.right = Camera.main.ViewportToWorldPoint(Vector3.right);
        this.top = Camera.main.ViewportToWorldPoint(Vector3.up);
        this.bot = Camera.main.ViewportToWorldPoint(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (!reloading)
        {
            // change direction left -> up -> right -> down direction check for movement
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                direction = Vector3.up;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                direction = Vector3.down;
            }

            if ((coolDown >= waitTime) && Input.GetKey(KeyCode.Space))
            {
                gottaGoFast = true;
                coolDown = 0.0f;
            }

            coolDown += Time.fixedDeltaTime;

                Vector3 currPosition = gameObject.transform.position;
            // check the boundries of the screen

            if (currPosition.x < left.x + 0.2f || currPosition.x > right.x - 0.2f)
            {
                GameLose(true);
            }
            else if (currPosition.y > top.y - 0.2f || currPosition.y < bot.y + 0.2f)
            {
                GameLose(true);
            }
        }
    }

    void FixedUpdate()
    {
        if (!reloading)
        {
            // move all of snake segments but head
            for (int i = segmentTransforms.Count - 1; i > 0; i--)
            {
                segmentTransforms[i].position = segmentTransforms[i - 1].position;
            }

            // move head
            Vector3 prevSegmentPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(
                Mathf.Round(prevSegmentPosition.x) + direction.x,
                Mathf.Round(prevSegmentPosition.y) + direction.y,
                0.0f
                );

            if (gottaGoFast)
            {
                source.PlayOneShot(clipFast);
                for (int j = 0; j < 6; j++)
                {
                    // move all of snake segments but head
                    for (int i = segmentTransforms.Count - 1; i > 0; i--)
                    {
                        segmentTransforms[i].position = segmentTransforms[i - 1].position;
                    }

                    // move head
                    Vector3 prevSegmPosition = gameObject.transform.position;
                    gameObject.transform.position = new Vector3(
                        Mathf.Round(prevSegmPosition.x) + direction.x,
                        Mathf.Round(prevSegmPosition.y) + direction.y,
                        0.0f
                        );
                }
                gottaGoFast = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Apple")
        {
            Vector3 lastSegPosition = segmentTransforms[segmentTransforms.Count - 1].position;
            Snake newSeg = Instantiate(snakePrefab);
            newSeg.transform.position = lastSegPosition;
            segmentTransforms.Add(newSeg.transform);
            segments.Add(newSeg);

            if (segmentTransforms.Count == 15)
            {
                GameWin();
            }

        }
        else if (other.gameObject.tag == "Snake" && segmentTransforms.Count > 2)
        {
            GameLose(false);
        }
    }

    void GameLose(bool boundLoss)
    {
        // losing sound
        source.PlayOneShot(clipLoss);

        if (boundLoss)
        {
            // explosion
            Instantiate(
                explosionPrefab,
                gameObject.transform.position,
                Quaternion.identity
                );
        } else
        {
            // change head color
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        // reset score
        ScoreKeeper.SetScore(0.0f);

        // Reload game
        Invoke("Reload", 3.0f);
        reloading = true;
    }

    void GameWin()
    {
        // add to score here
        ScoreKeeper.AddToScore(15.0f);

        // wining sound
        source.PlayOneShot(clipWin);

        Invoke("Reload", 5.0f);
        reloading = true;
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
