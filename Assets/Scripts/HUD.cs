using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	[Header("Extra components")]
	public Slider ammoSlider;
	public Slider healthSlider;
	public Image crosshairImage;

	// Internal properties
	Toy localPlayerToy;

	void Start() {
		FindLocalPlayerToy();
	}

	void Update() {
		if (localPlayerToy != null) {
			ToyController toyController = localPlayerToy.GetComponent<ToyController>();
			ToyGun toyGun = localPlayerToy.GetComponent<ToyGun>();
			ammoSlider.value = toyGun.CurrentMagazine / toyGun.magazine;
			healthSlider.value = localPlayerToy.CurrentHealth / localPlayerToy.health;
			//
			RaycastHit hitInfo;
			Physics.Raycast(toyGun.barrelOut.position, toyGun.barrelOut.forward, out hitInfo, 50);
			Vector3 worldCrosshairPoint;
			if (hitInfo.collider != null) {
				worldCrosshairPoint = hitInfo.point;
			} else {
				worldCrosshairPoint = toyGun.barrelOut.position + toyGun.barrelOut.forward * 50;
			}
			Vector3 crosshairScreenPoint = toyController.camera.WorldToScreenPoint(worldCrosshairPoint);
			crosshairImage.rectTransform.position = crosshairScreenPoint;

		} else {
			ammoSlider.value = 0;
			healthSlider.value = 0;
			crosshairImage.rectTransform.position = new Vector3(-100, -100, 0);
			FindLocalPlayerToy();
		}
	}

	void FindLocalPlayerToy() {
		Toy[] toys = FindObjectsOfType<Toy>();
		foreach (Toy toy in toys) {
			if (toy.isLocalPlayer) {
				localPlayerToy = toy;
				break;
			}
		}
	}
}
