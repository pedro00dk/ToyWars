using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Puu : MonoBehaviour {

	[Header("Animation/Transform Properties")]
	public Transform columnJoint;
	public Transform cameraColumnJoint;
	public Transform locator;

	[Header("Movement Properties")]
	public float speed;
	public float jumpSpeed;
	public Vector2 sensibility;

	// Joint independent (the up is negated in script)
	public Vector2 yRotationUpDown;

	// Animatior parameters
	string walkingAnimParam = "walking";
	string groundedAnimParam = "grounded";
	string walkingbackAnimParam = "walkingback";
	// boolean

	// Components
	Animator animator;
	Rigidbody body;

	// Internal properties
	bool walking;
	bool walkingback;
	bool grounded;

	bool jump;

	// Smooths, lerps and temps
	Vector3 currentMovementVelocity;

	Vector3 columnIndependentJointLocalEulerAngles;


	void Start() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
		walking = false;
		walkingback = false;
		grounded = true;
		jump = false;
		SetAnimationParams();
	}

	void Update() {

		// Movement
		walkingback = Input.GetKey(KeyCode.S);
		walking = Input.GetKey(KeyCode.W) || walkingback;

		// X rotation
		Vector3 localEulerAngles = new Vector3(
			                           transform.localEulerAngles.x,
			                           transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensibility.x * Time.deltaTime,
			                           transform.localEulerAngles.z
		                           );
		transform.localEulerAngles = localEulerAngles;

		// Jump
		Debug.DrawRay(locator.position + Vector3.up * 0.05f, Vector3.down, Color.red);
		grounded = Physics.Raycast(new Ray(locator.position + Vector3.up * 0.05f, Vector3.down), 0.1f);
		if (Input.GetKey(KeyCode.Space) && grounded) {
			jump = true;
		}
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
		if (walking && !walkingback) {
			body.MovePosition(
				Vector3.SmoothDamp(
					body.position,
					body.position + transform.forward,
					ref currentMovementVelocity,
					0.2f
				)
			);
		} else if (walkingback) {
			body.MovePosition(
				Vector3.SmoothDamp(
					body.position,
					body.position - transform.forward,
					ref currentMovementVelocity,
					0.2f
				)
			);
		}


		// Jump
		if (jump) {
			jump = false;
			body.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
		}
	}

	void SetAnimationParams() {
		animator.SetBool(walkingAnimParam, walking);
		animator.SetBool(groundedAnimParam, grounded);
	}
}
