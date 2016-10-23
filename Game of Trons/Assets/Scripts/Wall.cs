using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Wall : NetworkBehaviour {

    [SyncVar]
    public Color color;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdSetColor(Color c)
    {
        color = c;
        UpdateColor();
    }

    public void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
