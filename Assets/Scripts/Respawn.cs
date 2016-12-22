using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Respawn : NetworkBehaviour {

	Toy[] Toys;
	float[] Counters;
	public int Time2Respawn = 5;

	public int plasticKills;
	public int teddyKills;

	NetworkStartPosition[] NetworkStartPositions;

	void Populate() {
		Toys = FindObjectsOfType<Toy>();
		Counters = new float[Toys.Length];

		for (int i = 0; i < Counters.Length; i++)
			Counters[i] = 0;
	}

	// Use this for initialization
	void Start() {
		Populate();
		NetworkStartPositions = FindObjectsOfType<NetworkStartPosition>();
	}

	// Update is called once per frame
	void Update() {

		if (FindObjectsOfType<Toy>().Length > Toys.Length)
			Populate();

		for (int i = 0; i < Toys.Length; i++) {
			if (Toys[i].Dead) {

				if (Counters[i] == 0) {
					if (Toys[i].GetComponent<Player>().team == Team.PLASTIC) {
						teddyKills++;
					} else {
						plasticKills++;
					}
				}

				Counters[i] += Time.deltaTime;

				if (Counters[i] >= Time2Respawn) {
					Toys[i].Reset();
					Counters[i] = 0;

					float index = Random.Range(0, 1000) % NetworkStartPositions.Length;
					Toys[i].transform.position = NetworkStartPositions[(int) index].transform.position;
				}
			}
		}
	}
}
