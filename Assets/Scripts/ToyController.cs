using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Toy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ToyController : MonoBehaviour {

	[Header("Skeleton components")]
	public Transform spineJoint;
	public Transform toyLocator;

	[Header("Camera components")]
	public Transform camSpineParent;
	public new Camera camera;

	[Header("Movement properties")]
	public float forwardSpeed;
	public float backSpeed;
	public float sideSpeed;
	public float jumpSpeed;

	[Header("Rotation properties")]
	public Vector2 sensibility;
	public Vector2 verticalLimitMinMax;

	// Components
	Toy toy;
	Animator animator;
	Rigidbody body;

	// Animator properties
	bool walking = false;
	bool grounded = true;
	bool dead = false;

	// Internal properties
	Vector2 movementAxis;
	float jumping;
	Vector2 rotationAxis;

	// Smooths and temps
	Vector3 smoothMovementVelocity;
	Vector3 spineUpdateEulerAngles;

	//

	void Start() {
		toy = GetComponent<Toy>();
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();

		SetAnimationProperties();
	}

	void Update() {

		// Movement check
		movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movementAxis.x *= sideSpeed;
		movementAxis.y *= movementAxis.y > 0 ? forwardSpeed : backSpeed;
		walking = movementAxis.sqrMagnitude > 0;

		// Jump check
		grounded = Physics.Raycast(toyLocator.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
		jumping = grounded ? Input.GetAxisRaw("Jump") * jumpSpeed : 0;

		// Rotation check
		rotationAxis = new Vector2(
			Input.GetAxisRaw("Mouse X") * sensibility.x, Input.GetAxisRaw("Mouse Y") * sensibility.y
		);

		// Dead check
		dead = toy.Dead;

		SetAnimationProperties();
	}

	void LateUpdate() {

		// Dead block
		if (dead) {
			return;
		}

		// Rotation exec
		Vector3 updatedEulerAngles = new Vector3(
			                             transform.localEulerAngles.x,
			                             transform.localEulerAngles.y + rotationAxis.x * Time.deltaTime,
			                             transform.localEulerAngles.z
		                             );
		transform.localEulerAngles = updatedEulerAngles; // Horizontal rotation (over y axis)
		spineUpdateEulerAngles = new Vector3(Mathf.Clamp(
			spineUpdateEulerAngles.x - rotationAxis.y * Time.deltaTime,
			-verticalLimitMinMax.y, verticalLimitMinMax.x),
			0,
			0
		);
		spineJoint.localEulerAngles += spineUpdateEulerAngles; // Vertical rotation (rotation of the spine joint over x)
		camSpineParent.localEulerAngles = spineUpdateEulerAngles;
	}

	void FixedUpdate() {

		// Dead block
		if (dead) {
			return;
		}

		// Movement exec
		Vector3 targetPosition = body.position
		                         + ((transform.forward * movementAxis.y) + (transform.right * movementAxis.x));
		body.MovePosition(Vector3.SmoothDamp(body.position, targetPosition, ref smoothMovementVelocity, 0.2f,
			Mathf.Max(Mathf.Max(forwardSpeed, backSpeed), sideSpeed),
			Time.fixedDeltaTime)
		);

		// Jump exec
		body.AddForce(Vector3.up * jumping, ForceMode.VelocityChange);
	}

	void SetAnimationProperties() {
		animator.SetBool("walking", walking);
		animator.SetBool("grounded", grounded);
		animator.SetBool("dead", dead);
	}
}
