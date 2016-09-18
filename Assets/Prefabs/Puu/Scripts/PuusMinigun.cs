using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ToyWeapon))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PuusMinigun : MonoBehaviour {

	[Header("Transform properties")]
	public Transform barrelOut;

	[Header("Shoot effect properties")]
	public float shootFlashTime;
	public SpriteRenderer[] shootEffectsRenderers;
	public Sprite[] shootRandomSpriteEffects;
	public Light flashLightEffect;
	public AudioClip accelelerationClip;
	public AudioClip desaccelelerationClip;
	public AudioClip rotationClip;
	public AudioClip shootClip;

	// Components
	ToyWeapon toyWeapon;
	Animator animator;
	AudioSource rotatePlayer;
	AudioSource shootPlayer;

	// Animator properties
	bool triggered = false;

	// Internal properties
	bool startedRotation;
	float triggeredTime;
	float lastShootTime;

	//

	void Start() {
		toyWeapon = GetComponent<ToyWeapon>();
		animator = GetComponent<Animator>();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		rotatePlayer = audioSources[0];
		shootPlayer = audioSources[1];
		shootPlayer.clip = shootClip;
	}

	void Update() {

		// Trigger check
		triggered = toyWeapon.Triggered;

		// Trigger exec
		if (triggered) {
			if (!startedRotation) {
				startedRotation = true;
				triggeredTime = Time.timeSinceLevelLoad;
				AccelerateBarrel();
			}
			if (Time.timeSinceLevelLoad >= triggeredTime + 1) {
				if (Time.timeSinceLevelLoad >= lastShootTime + 1 / toyWeapon.fireRate) {
					lastShootTime = Time.timeSinceLevelLoad + 1 / toyWeapon.fireRate;
					Shoot();
					EnableShootEffects();
				}
			}
		} else {
			if (startedRotation) {
				DesaccelerateBarrel();
			}
			startedRotation = false;
		}

		SetAnimationProperties();
	}

	void Shoot() {
		toyWeapon.Shoot();
		RaycastHit[] hits = Physics.RaycastAll(barrelOut.position, barrelOut.forward);
		foreach (RaycastHit hit in hits) {
			ToyPart hittedPart = hit.collider.GetComponent<ToyPart>();
			if (hittedPart != null) {
				hittedPart.Hit(toyWeapon.toy, toyWeapon.damage);
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
		if (!rotatePlayer.clip.Equals(rotationClip)) {
			rotatePlayer.clip = rotationClip;
			rotatePlayer.loop = true;
			rotatePlayer.Play();
		}
		Invoke("DisableShootEffects", shootFlashTime);
	}

	void DisableShootEffects() {
		foreach (SpriteRenderer effectRenderer in shootEffectsRenderers) {
			effectRenderer.gameObject.SetActive(false);
		}
		flashLightEffect.gameObject.SetActive(false);
	}

	void AccelerateBarrel() {
		rotatePlayer.clip = accelelerationClip;
		rotatePlayer.loop = false;
		rotatePlayer.Play();
	}

	void DesaccelerateBarrel() {
		rotatePlayer.clip = desaccelelerationClip;
		rotatePlayer.loop = false;
		rotatePlayer.Play();
	}

	void SetAnimationProperties() {
		animator.SetBool("triggered", triggered);
	}
}
