using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ToyWeapon))]
[RequireComponent(typeof(AudioSource))]
public class ZulusShotgun : MonoBehaviour {

	[Header("Exclusive properties")]
	public int shootParticleCount;
	public float outputAngleVariation;

	[Header("Transform properties")]
	public Transform barrelOut;

	[Header("Shoot effect properties")]
	public float shootFlashTime;
	public SpriteRenderer[] shootEffectsRenderers;
	public Sprite[] shootRandomSpriteEffects;
	public Light flashLightEffect;
	public AudioClip shootClip;

	// Components
	ToyWeapon toyWeapon;
	AudioSource shootPlayer;

	// Internal properties
	float lastShootTime;

	//

	void Start() {
		toyWeapon = GetComponent<ToyWeapon>();
		shootPlayer = GetComponent<AudioSource>();
		shootPlayer.clip = shootClip;
	}

	void Update() {
		
		// Trigger exec
		if (toyWeapon.Triggered) {
			if (Time.timeSinceLevelLoad >= lastShootTime + 1 / toyWeapon.fireRate) {
				lastShootTime = Time.timeSinceLevelLoad + 1 / toyWeapon.fireRate;
				Shoot();
				EnableShootEffects();
			}
		}
	}

	void Shoot() {
		toyWeapon.Shoot();
		for (int i = 0; i < shootParticleCount; i++) {
			Quaternion verticalRotation = Quaternion.AngleAxis(
				                              Random.Range(-outputAngleVariation / 2, outputAngleVariation / 2),
				                              barrelOut.right
			                              );
			Quaternion horizontallRotation = Quaternion.AngleAxis(
				                                 Random.Range(-outputAngleVariation / 2, outputAngleVariation / 2),
				                                 barrelOut.up
			                                 );
			Vector3 particleDiretion = verticalRotation * horizontallRotation * barrelOut.forward;
			RaycastHit[] hits = Physics.RaycastAll(barrelOut.position, particleDiretion);
			foreach (RaycastHit hit in hits) {
				ToyPart hittedPart = hit.collider.GetComponent<ToyPart>();
				if (hittedPart != null) {
					hittedPart.Hit(toyWeapon.toy, toyWeapon.damage);
				}
			}
		}
	}

	void EnableShootEffects() {
		Sprite selectedSprite = shootRandomSpriteEffects[Random.Range(0, shootRandomSpriteEffects.Length - 1)];
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.sprite = selectedSprite;
			effectRenderer.gameObject.SetActive(true);
		}
		flashLightEffect.gameObject.SetActive(true);
		shootPlayer.clip = shootClip;
		shootPlayer.Play();
		Invoke("DisableShootEffects", shootFlashTime);
	}

	void DisableShootEffects() {
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.gameObject.SetActive(false);
		}
		flashLightEffect.gameObject.SetActive(false);
	}
}
