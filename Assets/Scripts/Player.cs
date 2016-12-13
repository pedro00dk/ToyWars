using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	public string nickname = "Player";
	public Team team = Team.TEDDY;
	public ToyEnum toyEnum = ToyEnum.PUU;
}
