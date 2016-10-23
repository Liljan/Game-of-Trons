using UnityEngine;
using System.Collections;

public class NetworkPlayerController : MonoBehaviour {

    public float speed = 1.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        float vx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vy = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(vx, vy, 0.0f);
    }
}
