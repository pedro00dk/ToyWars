using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	string name = "Player";
	int id = 0;
	Team team = Team.TEDDY;

	public void ConfigurePlayer(string name, int id, Team team) {
		this.name = name;
		this.id = id;
		this.team = team;
	}

	public string Name {
		get {
			return name;
		}
	}

	public int Id {
		get {
			return id;
		}
	}

	public Team Team {
		get {
			return team;
		}
	}
}
