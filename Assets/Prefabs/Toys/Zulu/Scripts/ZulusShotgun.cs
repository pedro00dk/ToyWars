using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ToyGun))]
[RequireComponent(typeof(AudioSource))]
public class ZulusShotgun : MonoBehaviour {

	[Header("Exclusive properties")]
	public int shootParticleCount;
	public float outputAngleVariation;

	// Components
	ToyGun toyGun;

	// Internal properties
	float lastShootTime;

	//

	void Start() {
		toyGun = GetComponent<ToyGun>();
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
		for (int i = 0; i < shootParticleCount; i++) {
			Quaternion verticalRotation = Quaternion.AngleAxis(
				                              Random.Range(-outputAngleVariation / 2, outputAngleVariation / 2),
				                              toyGun.barrelOut.right
			                              );
			Quaternion horizontallRotation = Quaternion.AngleAxis(
				                                 Random.Range(-outputAngleVariation / 2, outputAngleVariation / 2),
				                                 toyGun.barrelOut.up
			                                 );
			Vector3 particleDiretion = verticalRotation * horizontallRotation * toyGun.barrelOut.forward;
			RaycastHit[] hits = Physics.RaycastAll(toyGun.barrelOut.position, particleDiretion);
			foreach (RaycastHit hit in hits) {
				ToyPart hittedPart = hit.collider.GetComponent<ToyPart>();
				if (hittedPart != null) {
					hittedPart.Hit(toyGun.toy, toyGun.damage);
				}
			}
		}
	}
}
