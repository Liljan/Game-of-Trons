using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NetworkPlayerController localPlayer = gamePlayer.GetComponent<NetworkPlayerController>();

        //    localPlayer.CmdSetColor(lobby.playerColor);
        //  localPlayer.CmdSetName(lobby.playerName);
        localPlayer.playerColor = lobby.playerColor;
        localPlayer.playerName = lobby.playerName;
    }
}
