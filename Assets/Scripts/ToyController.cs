using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(Toy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ToyController : NetworkBehaviour {

	[Header("Skeleton components")]
	public Transform spineJoint;
	public Vector3 lateralSpineAxis;
	public Transform toyLocator;

	[Header("Camera components")]
	public Transform camSpineParent;
	public Camera camera;

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
	float spineUpdateEulerAngles;
	// Used in spine rotation sync over clients
	[SyncVar]
	float syncSpineUpdateEulerAngles;

	// Others
	Vector3 initialSpineJointLocalEulerAngles;
	float lastJumpTime;

	//

	void Start() {
		toy = GetComponent<Toy>();
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();

		camera.gameObject.SetActive(isLocalPlayer);

		initialSpineJointLocalEulerAngles = spineJoint.localEulerAngles;

		SetAnimationProperties();
	}

	void Update() {

		if (!isLocalPlayer) {
			return;
		}

		// Movement check
		movementAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		movementAxis.x *= sideSpeed;
		movementAxis.y *= movementAxis.y > 0 ? forwardSpeed : backSpeed;
		walking = movementAxis.sqrMagnitude > 0;

		// Jump check
		grounded = Physics.Raycast(toyLocator.position + Vector3.up * 0.05f, Vector3.down, 0.2f);
		jumping = grounded && lastJumpTime < Time.timeSinceLevelLoad + 0.05f ? Input.GetAxisRaw("Jump") * jumpSpeed : 0;
		lastJumpTime = jumping > 0 ? Time.timeSinceLevelLoad + 0.05f : lastJumpTime;

		// Rotation check
		rotationAxis = new Vector2(
			Input.GetAxisRaw("Mouse X") * sensibility.x, Input.GetAxisRaw("Mouse Y") * sensibility.y
		);

		// Dead check
		dead = toy.Dead;

		SetAnimationProperties();
	}

	void LateUpdate() {

		if (!isLocalPlayer) {
			Vector3 nonLocalspineJointIncrementEulerAngles = lateralSpineAxis * syncSpineUpdateEulerAngles;
			spineJoint.localEulerAngles = initialSpineJointLocalEulerAngles + nonLocalspineJointIncrementEulerAngles; // Spine rotation
			camSpineParent.localEulerAngles = Vector3.right * syncSpineUpdateEulerAngles; // Camera rotation
			return;
		}

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
		transform.localEulerAngles = updatedEulerAngles; // Horizontal rotation
		spineUpdateEulerAngles = Mathf.Clamp(spineUpdateEulerAngles - rotationAxis.y * Time.deltaTime,
			-verticalLimitMinMax.y, verticalLimitMinMax.x
		);
		CmdSetSpineUpdateEulerAngles(spineUpdateEulerAngles);
		Vector3 spineJointIncrementEulerAngles = lateralSpineAxis * spineUpdateEulerAngles;
		spineJoint.localEulerAngles = initialSpineJointLocalEulerAngles + spineJointIncrementEulerAngles; // Spine rotation
		camSpineParent.localEulerAngles = Vector3.right * spineUpdateEulerAngles; // Camera rotation
	}

	void FixedUpdate() {

		if (!isLocalPlayer) {
			return;
		}

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

	// Network

	[Command]
	void CmdSetSpineUpdateEulerAngles(float value) {
		syncSpineUpdateEulerAngles = value;
	}
}
