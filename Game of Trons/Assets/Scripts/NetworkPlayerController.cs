using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    public float speed = 1.0f;

    [SyncVar]
    private float vx, vy;

    // Use this for initialization
    void Start()
    {
        vx = 0.0f;
        vy = 0.0f;
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
            }
            else if (Input.GetAxis("Horizontal") > 0.0f && vx == 0.0f)
            {
                vx = speed;
                vy = 0.0f;
                CmdMove(vx, vy);
            }
            else if (Input.GetAxis("Vertical") > 0.0f && vy == 0.0f)
            {
                vx = 0.0f;
                vy = speed;
                CmdMove(vx, vy);
            }
            else if (Input.GetAxis("Vertical") < 0.0f && vy == 0.0f)
            {
                vx = 0.0f;
                vy = -speed;
                CmdMove(vx, vy);
            }
        }

        // if (NetworkServer.active)
        transform.Translate(vx * Time.deltaTime, vy * Time.deltaTime, 0.0f);
    }

    [Command]
    public void CmdMove(float x, float y)
    {
        RpcMove(x, y);
    }

    [ClientRpc]
    public void RpcMove(float x, float y)
    {
        vx = x;
        vy = y;
    }
}
