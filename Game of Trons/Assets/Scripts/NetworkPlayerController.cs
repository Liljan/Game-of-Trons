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

    private Collider2D wallCollider;

    [SyncVar]
    private float vx, vy;

    private List<GameObject> walls;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // go left
            if (Input.GetAxis("Horizontal") < 0.0f && vx == 0.0f)
            {
                vx = -speed;
                vy = 0.0f;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetAxis("Horizontal") > 0.0f && vx == 0.0f)
            {
                vx = speed;
                vy = 0.0f;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetAxis("Vertical") > 0.0f && vy == 0.0f)
            {
                vx = 0.0f;
                vy = speed;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
            else if (Input.GetAxis("Vertical") < 0.0f && vy == 0.0f)
            {
                vx = 0.0f;
                vy = -speed;

                CmdMove(vx, vy);
                CmdSpawnWall();
            }
        }

        // if (NetworkServer.active)
        transform.Translate(vx * Time.deltaTime, vy * Time.deltaTime, 0.0f);
    }

    [Command]
    public void CmdSpawnWall()
    {
        lastWallPos = transform.position;

        // spawn new lightwall locally
        GameObject g = Instantiate(WallPrefab, transform.position, Quaternion.identity) as GameObject;
       
        NetworkServer.Spawn(g);
        RpcSetWallColor(g);

        wallCollider = g.GetComponent<Collider2D>();
        walls.Add(g);
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
        this.GetComponent<SpriteRenderer>().color = playerColor;
        this.GetComponentInChildren<TextMesh>().text = playerName;
        this.GetComponentInChildren<TextMesh>().color = playerColor;
    }
}
