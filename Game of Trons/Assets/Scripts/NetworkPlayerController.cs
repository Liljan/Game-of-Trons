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
        // go left
        if (Input.GetAxis("Horizontal") < 0.0f && vx == 0.0f)
        {
            vx = -speed;
            vy = 0.0f;
        }
        else if (Input.GetAxis("Horizontal") > 0.0f && vx == 0.0f)
        {
            vx = speed;
            vy = 0.0f;
        }
        else if (Input.GetAxis("Vertical") > 0.0f && vy == 0.0f)
        {
            vx = 0.0f;
            vy = speed;
        }
        else if (Input.GetAxis("Vertical") < 0.0f && vy == 0.0f)
        {
            vx = 0.0f;
            vy = -speed;
        }

        transform.Translate(vx * Time.deltaTime, vy * Time.deltaTime, 0.0f);
    }
}
