using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Move : MonoBehaviour
{
    public KeyCode upKey, downKey, rightKey, leftKey;
    public float speed = 16f;

    private Rigidbody2D rb2d;

    // spawning light walls
    public GameObject wallPrefab;
    private Collider2D wallCollider;
    private Vector2 lastWallPos;

    public GameObject ParticlePrefab;

    public Color playerColor;

    private List<GameObject> walls;

    private bool hasStarted;

    // Use this for initialization
    void Start()
    {
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        walls = new List<GameObject>();
        SpawnWall();

        hasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            if (Input.GetKeyDown(upKey) && rb2d.velocity.y == 0)
            {
                rb2d.velocity = Vector2.up * speed;
                SpawnWall();
            }
            else if (Input.GetKeyDown(downKey) && rb2d.velocity.y == 0)
            {
                rb2d.velocity = Vector2.down * speed;
                SpawnWall();
            }
            else if (Input.GetKeyDown(leftKey) && rb2d.velocity.x == 0)
            {
                rb2d.velocity = Vector2.left * speed;
                SpawnWall();
            }
            else if (Input.GetKeyDown(rightKey) && rb2d.velocity.x == 0)
            {
                rb2d.velocity = Vector2.right * speed;
                SpawnWall();
            }

            FitColliderBetween(wallCollider, lastWallPos, transform.position);
        }
    }

    void SpawnWall()
    {
        // Save last wall's position
        lastWallPos = transform.position;

        // Spawn a new Lightwall
        GameObject g = (GameObject)Instantiate(wallPrefab, transform.position, Quaternion.identity);
        g.GetComponent<SpriteRenderer>().color = playerColor;
        wallCollider = g.GetComponent<Collider2D>();
        walls.Add(g);
    }

    void FitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist + 1, 1);
        else
            co.transform.localScale = new Vector2(1, dist + 1);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col != wallCollider)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        SpawnParticles(transform.position);
        for (int i = 0; i < walls.Count; ++i)
        {
            Destroy(walls[i]);
        }
    }

    void SpawnParticles(Vector3 pos)
    {
        GameObject g = Instantiate(ParticlePrefab, pos, Quaternion.identity) as GameObject;
        g.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void StartGame()
    {
        rb2d.velocity = Vector2.up * speed;
        hasStarted = true;
    }
    public void StopGame()
    {
        rb2d.velocity = Vector2.zero;
        hasStarted = false;
    }
}