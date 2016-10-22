using UnityEngine;
using System.Collections;

public class WallDestroy : MonoBehaviour {

    public GameObject Particles;

    void OnDestroy()
    {
        GameObject g = Instantiate(Particles, transform.position, Quaternion.identity) as GameObject;
        g.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
    }
}
