using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Puu : MonoBehaviour {

	[Header("Animation/Transform Properties")]
	public Transform columnJoint;
	public Transform cameraColumnJoint;

	[Header("Movement Properties")]
	public float speed;
	public Vector2 sensibility;

	// Joint independent (the up is negated in script)
	public Vector2 yRotationUpDown;

	// Animatior parameters
	string walkingAnimParam = "walking";
	// boolean

	// Components
	Animator animator;
	Rigidbody body;

	// Internal properties
	bool walking;

	// Smooths, lerps and temps
	Vector3 currentMovementVelocity;

	Vector3 columnIndependentJointLocalEulerAngles;


	void Start() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
		walking = false;
		SetAnimationParams();
	}

	void Update() {

		// Movement
		walking = Input.GetKey(KeyCode.W);

		// X rotation
		Vector3 localEulerAngles = new Vector3(
			                           transform.localEulerAngles.x,
			                           transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensibility.x * Time.deltaTime,
			                           transform.localEulerAngles.z
		                           );
		transform.localEulerAngles = localEulerAngles;

		// Animation
		SetAnimationParams();
	}

	void LateUpdate() {
		// Y rotation (runs after animation)
		float rotation = -Input.GetAxis("Mouse Y") * sensibility.y * Time.deltaTime;
		columnIndependentJointLocalEulerAngles = new Vector3(
			Mathf.Clamp(
				columnIndependentJointLocalEulerAngles.x + rotation,
				-yRotationUpDown.x, yRotationUpDown.y
			),
			0,
			0
		);
		columnJoint.localEulerAngles += columnIndependentJointLocalEulerAngles;
		cameraColumnJoint.localEulerAngles = columnIndependentJointLocalEulerAngles;
	}

	void FixedUpdate() {

		// Movement
		if (walking) {
			body.MovePosition(
				Vector3.SmoothDamp(
					body.position,
					body.position + transform.forward,
					ref currentMovementVelocity,
					0.2f
				)
			);
		}
	}

	void SetAnimationParams() {
		animator.SetBool(walkingAnimParam, walking);
	}
}
