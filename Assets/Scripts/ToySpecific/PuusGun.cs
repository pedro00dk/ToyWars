using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PuusGun : MonoBehaviour {

	[Header("Properties")]
	public float fireRate;
	public float flashTime;

	[Header("Transform Properties")]
	public Transform barrelOut;
	public SpriteRenderer effectsRenderer1;
	public SpriteRenderer effectsRenderer2;

	[Header("Effect Properties")]
	public Light lightEffect;
	public Sprite[] shootRandomSpriteEffects;
	public AudioClip accelSound;
	public AudioClip desaccelSound;
	public AudioClip rotateSound;
	public AudioClip shootSound;

	// Animatior parameters
	string triggeredAnimParam = "triggered";
	// boolean

	// Components
	Animator animator;
	AudioSource effectsPlayer;
	AudioSource shootPlayer;

	// Internal properties
	bool triggered;
	float triggeredTime;
	float lastShootTime;

	bool idle;
	bool accelerating;
	bool desaccelerating;
	bool rotating;

	void Start() {
		animator = GetComponent<Animator>();
		AudioSource[] audioSources = GetComponents<AudioSource>();
		effectsPlayer = audioSources[0];
		shootPlayer = audioSources[1];
		shootPlayer.clip = shootSound;
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButton(0)) {
			if (!triggered) {
				triggered = true;
				triggeredTime = Time.timeSinceLevelLoad;
			}

			if (!rotating) {
				BarrelRotationAccelerating();
			}

			if (triggered && Time.timeSinceLevelLoad >= triggeredTime + 1) {

				BarrelRotationRotating();

				if (Time.timeSinceLevelLoad >= lastShootTime + (1 / fireRate)) {
					lastShootTime = Time.timeSinceLevelLoad;
					Shoot();
				}

			}
		} else {
			if (triggered) {
				BarrelRotationDesaccelerating();
			}
			triggered = false;
		}
		SetAnimationParams();
	}

	void Shoot() {
		Sprite shootSprite = shootRandomSpriteEffects[Random.Range(0, shootRandomSpriteEffects.Length)];
		effectsRenderer1.sprite = shootSprite;
		effectsRenderer2.sprite = shootSprite;
		effectsRenderer1.gameObject.SetActive(true);
		effectsRenderer2.gameObject.SetActive(true);
		lightEffect.gameObject.SetActive(true);

		if (shootPlayer.isPlaying) {
			shootPlayer.Stop();
		}
		shootPlayer.Play();
		Invoke("StopEffects", flashTime);
	}

	void StopEffects() {
		effectsRenderer1.gameObject.SetActive(false);
		effectsRenderer2.gameObject.SetActive(false);
		lightEffect.gameObject.SetActive(false);
	}

	void BarrelRotationAccelerating() {
		if (!accelerating) {
			accelerating = true;
			desaccelerating = false;
			rotating = false;
			effectsPlayer.clip = accelSound;
			effectsPlayer.loop = false;
			effectsPlayer.Play();
		}
	}

	void BarrelRotationDesaccelerating() {
		if (!desaccelerating) {
			accelerating = false;
			desaccelerating = true;
			rotating = false;
			effectsPlayer.clip = desaccelSound;
			effectsPlayer.loop = false;
			effectsPlayer.Play();
		}
	}

	void BarrelRotationRotating() {
		if (!rotating) {
			accelerating = false;
			desaccelerating = false;
			rotating = true;
			effectsPlayer.clip = rotateSound;
			effectsPlayer.loop = true;
			effectsPlayer.Play();
		}
	}

	void SetAnimationParams() {
		animator.SetBool(triggeredAnimParam, triggered);
	}
}
