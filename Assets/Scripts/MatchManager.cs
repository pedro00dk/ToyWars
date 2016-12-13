using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : NetworkBehaviour {

	public Transform teddySpawnPosition;
	public Transform plasticSpawnPosition;

	Toy[] toys;
	bool[] toysDeathFlag;
	float[] toysDeathTime;

	bool gameStarted;

	// Team statistics

	void OnStartGame() {

		if (!isServer) {
			return;
		}

		toys = GetComponents<Toy>();
		toysDeathFlag = new bool[toys.Length];
		toysDeathTime = new float[toys.Length];

		gameStarted = true;
	}

	void Update() {

		if (!isServer || !gameStarted) {
			return;
		}

		for (int i = 0; i < toys.Length; i++) {
			if (toys[i].Dead && !toysDeathFlag[i]) {
				toysDeathFlag[i] = true;
				toysDeathTime[i] = Time.timeSinceLevelLoad;

				// check team

			} else if (toys[i].Dead && toysDeathFlag[i]) {
				if (toysDeathTime[i] + 10 < Time.timeSinceLevelLoad) {
					toys[i].Reset();

					//
					//toys[i].transform.position = teddySpawnPosition.position;
					//toys[i].transform.position = plasticSpawnPosition.position;

					toysDeathFlag[i] = false;
				}
			}
		}
	}
}
