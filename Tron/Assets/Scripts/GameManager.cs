using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject CountDownPrefab;
    public GameObject WinPrefab;


    private int nPlayers;
	// Use this for initialization
	void Start () {
        nPlayers = FindObjectsOfType<Move>().Length;



	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        foreach (Move m in FindObjectsOfType<Move>())
        {
            m.StartGame();
        }
    }

    public void RemovePlayer()
    {
        --nPlayers;
        if(nPlayers == 1)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        foreach (Move m in FindObjectsOfType<Move>())
        {
            m.StopGame();
        }

    }


}
