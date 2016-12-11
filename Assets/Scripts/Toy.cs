using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Toy : NetworkBehaviour {

	[Header("Health properties")]
	public float health;
	public float regeneration;

	// Components
	ToyPart[] toyParts;

	// Internal properties
	[SyncVar]
	float currentHealth;
	[SyncVar]
	bool dead;

	//

	void Start() {
		toyParts = GetComponentsInChildren<ToyPart>();
		foreach (ToyPart part in toyParts) {
			part.SetOnHit(TakeDamage);
		}
		currentHealth = health;
		dead = false;
	}

	void Update() {

		if (!isServer) {
			return;
		}

		// Regeneration (runs on server)
		if (!dead) {
			currentHealth = Mathf.Clamp(currentHealth + regeneration * Time.deltaTime, 0, health);
		}

	}

	// Take damage runs in the server (should be called by a Command function)
	void TakeDamage(string damager, ToyPart.Part part, float damage) {
		if (!dead) {
			float remainingHealth = currentHealth - damage;
			bool died = false;
			if (remainingHealth <= 0) {
				remainingHealth = 0;
				died = true;
			}
			currentHealth = remainingHealth;
			dead = died;
		}
	}

	// Reset runs on server
	public void Reset() {
		currentHealth = health;
		dead = false;
	}

	public float CurrentHealth {
		get {
			return currentHealth;
		}
	}

	public bool Dead {
		get {
			return dead;
		}
	}
}
