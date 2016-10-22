using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour {

    public float timer = 2.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyThis(timer));
	}

    IEnumerator DestroyThis(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(this.gameObject);
    }
}
