using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ToyGun : MonoBehaviour {

	[Header("Toy object")]
	public Toy toy;

	[Header("Transform properties")]
	public Transform barrelOut;

	[Header("Gun properties")]
	public float damage;
	public float fireRate;
	public float magazine;
	public float regeneration;

	[Header("Shoot effect properties")]
	public float shootFlashTime;
	public SpriteRenderer[] shootEffectsRenderers;
	public Sprite[] shootRandomSpriteEffects;
	public Light flashLightEffect;
	public AudioClip shootClip;

	// Components
	AudioSource audioSource;

	// Internal properties
	float currentMagazine;
	bool triggered;

	//

	void Start() {
		audioSource = GetComponent<AudioSource>();
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

	public void Shoot() {
		currentMagazine -= 1;
		EnableShootEffects();
	}

	void EnableShootEffects() {
		Sprite selectedSprite = shootRandomSpriteEffects[Random.Range(0, shootRandomSpriteEffects.Length - 1)];
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.sprite = selectedSprite;
			effectRenderer.gameObject.SetActive(true);
		}
		flashLightEffect.gameObject.SetActive(true);
		audioSource.clip = shootClip;
		audioSource.Play();
		StartCoroutine(DisableShootEffects());
	}

	IEnumerator DisableShootEffects() {
		yield return new WaitForSeconds(shootFlashTime);
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.gameObject.SetActive(false);
		}
		flashLightEffect.gameObject.SetActive(false);
	}

	public bool Triggered {
		get {
			return triggered;
		}
	}
}
