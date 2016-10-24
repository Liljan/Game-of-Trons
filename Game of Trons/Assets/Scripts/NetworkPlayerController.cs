using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/*
Function prefixes:
    Update: Updates variable locally
    Cmd: Updates server version
    Rpc: Updates client version from server
*/


public class NetworkPlayerController : NetworkBehaviour
{
    public float speed = 1.0f;
    [SyncVar]
    public Color playerColor;
    [SyncVar]
    public string playerName;

    private Collider2D wallCollider;

    [SyncVar]
    private float vx, vy;

    private List<GameObject> walls;

    Vector2 stretch;

    [SyncVar]
    private Vector2 lastWallPos;

    public GameObject WallPrefab;

    // Use this for initialization
    void Start()
    {
        vx = 0.0f;
        vy = 0.0f;

        walls = new List<GameObject>();

        // isLocalplayer ?
        if (Network.isClient)
        {
            CmdSetColor(playerColor);
            CmdSetName(playerName);
        }
        Init();

        if (isLocalPlayer)
        {
            CmdSpawnWall();
        }
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

        if (wallCollider)
        {
            FitColliderBetween(wallCollider, lastWallPos, transform.position);
        }

        transform.Translate(vx * Time.deltaTime, vy * Time.deltaTime, 0.0f);
    }

    // done on server
    [Command]
    public void CmdSpawnWall()
    {
        lastWallPos = transform.position;

        // spawn new lightwall locally
        GameObject g = Instantiate(WallPrefab, transform.position, Quaternion.identity) as GameObject;
        g.GetComponent<SpriteRenderer>().color = playerColor;

        NetworkServer.Spawn(g);
        RpcSetWallColor(g);

        wallCollider = g.GetComponent<Collider2D>();
        // update on all clients
        RpcSetWallCollider(g);

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
        Debug.Log(co);
        Debug.Log(a);
        Debug.Log(b);

        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
        {
            co.transform.localScale = new Vector2(dist + 1.0f, 1.0f);
        }
        else if (a.y != b.y)
        {
            co.transform.localScale = new Vector2(1.0f, dist + 1.0f);
        }

        stretch = co.transform.localScale;

        if (isServer)
            RpcSetWallStretch(stretch);
    }

    // on the clients
    [ClientRpc]
    public void RpcSetWallStretch(Vector2 v)
    {
        wallCollider.transform.localScale = v;
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

    private void UpdateColor(Color v)
    {
        playerColor = v;
    }

    private void UpdateName(string s)
    {
        playerName = s;
    }

    public void Init()
    {
        this.GetComponentInChildren<TextMesh>().color = playerColor;
        this.GetComponentInChildren<TextMesh>().text = playerName;
    }
}
