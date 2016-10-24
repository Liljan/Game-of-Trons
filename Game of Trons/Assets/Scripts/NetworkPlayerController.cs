using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    public float speed = 1.0f;
    [SyncVar]
    public Color playerColor;
    [SyncVar]
    public string playerName;

    [SyncVar]
    private Collider2D wallCollider;

    [SyncVar]
    private float vx, vy;

    private List<GameObject> walls;

    [SyncVar]
    private Vector3 lastWallPos;

    public GameObject WallPrefab;

    // Use this for initialization
    void Start()
    {
        vx = 0.0f;
        vy = 0.0f;

        walls = new List<GameObject>();

        CmdSetColor(playerColor);
        CmdSetName(playerName);
        Init();

        CmdSpawnWall();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // go left
            if (Input.GetButtonDown("Left") && vx == 0.0f)
            {
                vx = -speed;
                vy = 0.0f;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetButtonDown("Right") && vx == 0.0f)
            {
                vx = speed;
                vy = 0.0f;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetButtonDown("Up") && vy == 0.0f)
            {
                vx = 0.0f;
                vy = speed;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetButtonDown("Down") && vy == 0.0f)
            {
                vx = 0.0f;
                vy = -speed;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
        }

        transform.Translate(vx * Time.deltaTime, vy * Time.deltaTime, 0.0f);

        //  FitColliderBetween(wallCollider, lastWallPos, transform.position);
    }

    [Command]
    public void CmdSpawnWall()
    {
        lastWallPos = transform.position;

        // spawn new lightwall locally
        GameObject g = Instantiate(WallPrefab, transform.position, Quaternion.identity) as GameObject;

        NetworkServer.Spawn(g);

        // do it locally first
        RpcSetWallColor(g);

        // wallCollider = g.GetComponent<Collider2D>();
        // RpcSetWallCollider(g);

        walls.Add(g);
    }

    [ClientRpc]
    public void RpcSetWallCollider(GameObject g)
    {
        wallCollider = g.GetComponent<Collider2D>();
    }

    public void FitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
        {
            co.transform.localScale = new Vector2(dist + 1, 1);
        }
        else
        {
            co.transform.localScale = new Vector2(1, dist + 1);
        }

        CmdUpdateWallStretch(co.transform.localScale);
    }

    [Command]
    public void CmdUpdateWallStretch(Vector2 stretch)
    {
        wallCollider.transform.localScale = stretch;
        RpcUpdateWallStretch(stretch);
    }

    [ClientRpc]
    public void RpcUpdateWallStretch(Vector3 stretch)
    {
        wallCollider.transform.localScale = stretch;
    }

    [ClientRpc]
    public void RpcSetWallColor(GameObject g)
    {
        g.GetComponent<SpriteRenderer>().color = playerColor;
    }

    [Command]
    public void CmdMove(float x, float y)
    {
        vx = x;
        vy = y;
    }

    [Command]
    public void CmdSetColor(Color c)
    {
        playerColor = c;
    }

    [Command]
    public void CmdSetName(string s)
    {
        playerName = s;
    }

    public void Init()
    {
        // this.GetComponent<SpriteRenderer>().color = playerColor;
        this.GetComponentInChildren<TextMesh>().text = playerName;
        this.GetComponentInChildren<TextMesh>().color = playerColor;
    }
}
