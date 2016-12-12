using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;

public class PlayerSelector : MonoBehaviour {

	public int selectedPlayer;
	LobbyManager lm;
	public GameObject player0;
	public GameObject player1;
	public GameObject player2;
	public GameObject player3;

	public GameObject PlayerCharacter {
		get;
		set;
	}

	// Use this for initialization
	void Start() {
		selectedPlayer = 0;
		lm = FindObjectOfType<LobbyManager>();
		selectPlayer(0);
	}
	
	// Update is called once per frame
	void Update() {
	
	}

	public void selectPlayer(int player) {
		selectedPlayer = player;

		if (selectedPlayer == 0) {
			lm.gamePlayerPrefab = player0;
			lm.playerPrefab = player0;

			PlayerCharacter = player0;
		} else if (selectedPlayer == 1) {
			lm.gamePlayerPrefab = player1;
			lm.playerPrefab = player1;

			PlayerCharacter = player1;
		} else if (selectedPlayer == 2) {
			lm.gamePlayerPrefab = player2;
			lm.playerPrefab = player2;

			PlayerCharacter = player2;
		} else if (selectedPlayer == 3) {
			lm.gamePlayerPrefab = player3;
			lm.playerPrefab = player3;

			PlayerCharacter = player3;
		}
	}
}
