using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

	Toy localPlayerToy;

	// Components
	Slider ammoSlider;
	Slider healthSlider;

	void Start() {
		Slider[] sliders = GetComponentsInChildren<Slider>();
		if (sliders[0].name.Equals("AmmoSlider")) {
			ammoSlider = sliders[0];
			healthSlider = sliders[1];
		} else {
			ammoSlider = sliders[1];
			healthSlider = sliders[0];
		}
		FindLocalPlayerToy();
	}

	void Update() {
		if (localPlayerToy != null) {
			ToyGun toyGun = localPlayerToy.GetComponent<ToyGun>();
			ammoSlider.value = toyGun.CurrentMagazine / toyGun.magazine;
			healthSlider.value = localPlayerToy.CurrentHealth / localPlayerToy.health;
		} else {
			ammoSlider.value = 0;
			healthSlider.value = 0;
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
