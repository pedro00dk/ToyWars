using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Toy))]
public class ToyGun : NetworkBehaviour {

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

	[Header("Extra components")]
	public AudioSource audioSource;

	// Components
	Toy toy;

	// Internal properties
	float currentMagazine;
	bool triggered;

	//

	void Start() {
		toy = GetComponent <Toy>();
		currentMagazine = magazine;
	}

	void Update() {

		if (!isLocalPlayer) {
			return;
		}
		
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
		BroadcastEnableShootEffects();
	}

	void BroadcastEnableShootEffects() {
		CmdEnableShootEffects();
	}

	[Command]
	void CmdEnableShootEffects() {
		EnableShootEffects();
		RpcEnableShootEffects();

	}

	[ClientRpc]
	void RpcEnableShootEffects() {
		EnableShootEffects();
	}

	void EnableShootEffects() {
		Sprite selectedSprite = shootRandomSpriteEffects[Random.Range(0, shootRandomSpriteEffects.Length - 1)];
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.sprite = selectedSprite;
		}
		flashLightEffect.intensity = 1;
		if (audioSource != null) {
			audioSource.clip = shootClip;
			audioSource.Play();
		}
		StartCoroutine(DisableShootEffects());
	}

	IEnumerator DisableShootEffects() {
		yield return new WaitForSeconds(shootFlashTime);
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.sprite = null;
		}
		flashLightEffect.intensity = 0;
	}

	public float CurrentMagazine {
		get {
			return currentMagazine;
		}
	}

	public bool Triggered {
		get {
			return triggered;
		}
	}
}
