using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PuusGun : MonoBehaviour {

	// Components
	Animator animator;

	// Internal components
	bool triggered;
	float triggeredTime;

	void Start() {
		animator = GetComponent<Animator>();
		animator.Play("Idle");
	}



	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.Space)) {
			animator.SetTrigger("triggered");
			animator.ResetTrigger("released");
		} else {
			animator.SetTrigger("released");
			animator.ResetTrigger("triggered");
		}
	}
}
