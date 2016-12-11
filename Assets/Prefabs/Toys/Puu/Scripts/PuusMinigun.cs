using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Toy))]
[RequireComponent(typeof(ToyGun))]
public class PuusMinigun : NetworkBehaviour {

	[Header("Exclusive properties")]
	public AudioClip accelelerationClip;
	public AudioClip desaccelelerationClip;
	public AudioClip rotationClip;

	[Header("Extra components")]
	public AudioSource effectsAudioSource;
	public Animator animator;

	// Components
	Toy toy;
	ToyGun toyGun;

	// Animator properties
	bool triggered = false;

	// Internal properties
	bool startedRotation;
	float triggeredTime;
	float lastShootTime;

	//

	void Start() {
		toy = GetComponent<Toy>();
		toyGun = GetComponent<ToyGun>();
	}

	void Update() {

		if (!isLocalPlayer) {
			return;
		}

		// Trigger check
		triggered = toyGun.Triggered;

		// Trigger exec
		if (triggered) {
			if (!startedRotation) {
				startedRotation = true;
				triggeredTime = Time.timeSinceLevelLoad;
				BroadcastAccelerateBarrel();
			}
			if (Time.timeSinceLevelLoad >= triggeredTime + 1) {
				if (Time.timeSinceLevelLoad >= lastShootTime + 1 / toyGun.fireRate) {
					lastShootTime = Time.timeSinceLevelLoad + 1 / toyGun.fireRate;
					toyGun.Shoot();
					BroadcastRotateBarrel();
					CmdShoot();
				}
			}
		} else {
			if (startedRotation) {
				BroadcastDesaccelerateBarrel();
			}
			startedRotation = false;
		}
		SetAnimationProperties();
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

	void BroadcastRotateBarrel() {
		CmdRotateBarrel();
	}

	[Command]
	void CmdRotateBarrel() {
		RotateBarrel();
		RpcRotateBarrel();
	}

	[ClientRpc]
	void RpcRotateBarrel() {
		RotateBarrel();
	}

	void RotateBarrel() {
		if (!effectsAudioSource.clip.Equals(rotationClip)) {
			effectsAudioSource.clip = rotationClip;
			effectsAudioSource.loop = true;
			effectsAudioSource.Play();
		}
	}

	void BroadcastAccelerateBarrel() {
		CmdAccelerateBarrel();
	}

	[Command]
	void CmdAccelerateBarrel() {
		AccelerateBarrel();
		RpcAccelerateBarrel();
	}

	[ClientRpc]
	void RpcAccelerateBarrel() {
		AccelerateBarrel();
	}

	void AccelerateBarrel() {
		effectsAudioSource.clip = accelelerationClip;
		effectsAudioSource.loop = false;
		effectsAudioSource.Play();
	}

	void BroadcastDesaccelerateBarrel() {
		CmdDesaccelerateBarrel();
	}

	[Command]
	void CmdDesaccelerateBarrel() {
		DesaccelerateBarrel();
		RpcDesaccelerateBarrel();
	}

	[ClientRpc]
	void RpcDesaccelerateBarrel() {
		DesaccelerateBarrel();
	}

	void DesaccelerateBarrel() {
		effectsAudioSource.clip = desaccelelerationClip;
		effectsAudioSource.loop = false;
		effectsAudioSource.Play();
	}

	void SetAnimationProperties() {
		animator.SetBool("triggered", triggered);
		CmdSetAnimationProperties(triggered);
	}

	[Command]
	void CmdSetAnimationProperties(bool triggered) {
		this.triggered = triggered = triggered;
		animator.SetBool("triggered", triggered);
		RpcSetAnimationProperties(triggered);
	}

	[ClientRpc]
	void RpcSetAnimationProperties(bool triggered) {
		this.triggered = triggered = triggered;
		animator.SetBool("triggered", triggered);
	}
}
