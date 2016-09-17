using UnityEngine;
using System.Collections;

public class Toy : MonoBehaviour {

	[Header("Health properties")]
	public float health;
	public float regeneration;

	// Components
	ToyPart[] toyParts;

	// Internal properties
	float currentHealth;
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
		
		// Regeneration
		if (!dead) {
			currentHealth = Mathf.Clamp(currentHealth + regeneration * Time.deltaTime, 0, health);
		}
	}

	void TakeDamage(Toy damager, ToyPart.Part part, float damage) {
		if (!dead) {
			currentHealth -= damage;
			print("damage " + damage);
			print("health " + currentHealth);
			if (currentHealth <= 0) {
				currentHealth = 0;
				dead = true;
			}
		}
	}

	public void Reset() {
		currentHealth = health;
		dead = false;
	}

	public float Health {
		get {
			return health;
		}
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
