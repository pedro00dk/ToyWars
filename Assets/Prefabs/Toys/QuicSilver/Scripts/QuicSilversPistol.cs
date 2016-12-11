using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Toy))]
[RequireComponent(typeof(ToyGun))]
public class QuicSilversPistol : NetworkBehaviour {

	// Components
	Toy toy;
	ToyGun toyGun;

	// Internal properties
	float lastShootTime;

	void Start() {
		toy = GetComponentInChildren<Toy>();
		toyGun = GetComponentInChildren<ToyGun>();
	}

	void Update() {

		if (!isLocalPlayer) {
			return;
		}

		// Trigger exec
		if (toyGun.Triggered) {
			if (Time.timeSinceLevelLoad >= lastShootTime + 1 / toyGun.fireRate) {
				lastShootTime = Time.timeSinceLevelLoad + 1 / toyGun.fireRate;
				toyGun.Shoot();
				CmdShoot();
			}
		}
	}

	[Command]
	void CmdShoot() {
		RaycastHit[] hits = Physics.RaycastAll(toyGun.barrelOut.position, toyGun.barrelOut.forward);
		foreach (RaycastHit hit in hits) {
			ToyPart hittedPart = hit.collider.GetComponent<ToyPart>();
			if (hittedPart != null) {
				hittedPart.Hit(toy, toyGun.damage);
			}
		}
	}
}
