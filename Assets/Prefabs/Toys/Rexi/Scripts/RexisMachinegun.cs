using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ToyGun))]
public class RexisMachinegun : MonoBehaviour {

	// Components
	ToyGun toyGun;

	// Internal properties
	float lastShootTime;


	void Start() {
		toyGun = GetComponentInChildren<ToyGun>();
	}

	void Update() {

		// Trigger exec
		if (toyGun.Triggered) {
			if (Time.timeSinceLevelLoad >= lastShootTime + 1 / toyGun.fireRate) {
				lastShootTime = Time.timeSinceLevelLoad + 1 / toyGun.fireRate;
				toyGun.Shoot();
				Shoot();
			}
		}
	}

	void Shoot() {
		RaycastHit[] hits = Physics.RaycastAll(toyGun.barrelOut.position, toyGun.barrelOut.forward);
		foreach (RaycastHit hit in hits) {
			ToyPart hittedPart = hit.collider.GetComponent<ToyPart>();
			if (hittedPart != null) {
				hittedPart.Hit(toyGun.toy, toyGun.damage);
			}
		}
	}
}
