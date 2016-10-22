using UnityEngine;
using System.Collections;

public class GLOBALS : MonoBehaviour {

    private static bool isGameOn = false;

    public static void SetGameOn(bool b) {isGameOn = b;}
    public static bool IsGameOn() {return isGameOn;}
}
