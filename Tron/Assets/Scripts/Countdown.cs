using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Countdown : MonoBehaviour
{

    public float timer = 0.5f;
    public GameObject[] messages;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(FinalCountdown(timer));
        GLOBALS.SetGameOn(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FinalCountdown(float t)
    {
        yield return new WaitForSeconds(t);

        foreach (GameObject obj in messages)
        {
            GameObject g = Instantiate(obj, transform.position, Quaternion.identity) as GameObject;
            g.transform.SetParent(this.transform);
            yield return new WaitForSeconds(t);
            Destroy(g);
        }

        StartPlayers();

    }

    private void StartPlayers()
    {
        foreach (Move m in FindObjectsOfType<Move>())
        {
            m.StartGame();
        }
    }

}
