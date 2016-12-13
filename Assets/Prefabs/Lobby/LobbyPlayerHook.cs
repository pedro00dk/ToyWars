using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayerHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<LobbyPlayer>();
		Player playerComponent = gamePlayer.GetComponent<Player>();

		if (lobbyPlayerComponent.playerName.Contains("teddy")) {
			playerComponent.team = Team.TEDDY;
		} else if (lobbyPlayerComponent.playerName.Contains("plastic")) {
			playerComponent.team = Team.PLASTIC;
		} else {
			playerComponent.team = Team.TEDDY;
		}
		if (lobbyPlayerComponent.playerName.Contains("puu")) {
			playerComponent.toyEnum = ToyEnum.PUU;
		} else if (lobbyPlayerComponent.playerName.Contains("qsilver")) {
			playerComponent.toyEnum = ToyEnum.QSILVER;
		} else if (lobbyPlayerComponent.playerName.Contains("rexi")) {
			playerComponent.toyEnum = ToyEnum.REXI;
		} else if (lobbyPlayerComponent.playerName.Contains("zulu")) {
			playerComponent.toyEnum = ToyEnum.ZULU;
		} else {
			playerComponent.toyEnum = ToyEnum.PUU;
		}
		playerComponent.nickname = lobbyPlayerComponent.playerName;
	}
}
