using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	string nickname = "Player";
	string id = "000";
	Team team = Team.TEDDY;

	public void ConfigurePlayer(string name, string id, Team team) {
		this.nickname = name;
		this.id = id;
		this.team = team;
	}

	public string Nickname {
		get {
			return nickname;
		}
	}

	public string Id {
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
