using UnityEngine;
using System.Collections;

public class ToyWeapon : MonoBehaviour {

	[Header("Toy object")]
	public Toy toy;

	[Header("Gun properties")]
	public float damage;
	public float fireRate;
	public float magazine;
	public float regeneration;

	// Internal properties
	float currentMagazine;
	bool triggered;

	//

	void Start() {
		currentMagazine = magazine;
	}

	void Update() {
		
		// Dead block
		if (toy.Dead) {
			triggered = false;
			return;
		}

		// Trigger check
		triggered = Input.GetAxisRaw("Fire1") != 0 && currentMagazine >= 1;

		// Regeneration
		currentMagazine = Mathf.Clamp(currentMagazine + regeneration * Time.deltaTime, 0, magazine);
	}

	// Call when the weapon shoot
	public void Shoot() {
		currentMagazine -= 1;
	}

	public bool Triggered {
		get {
			return triggered;
		}
	}
}
