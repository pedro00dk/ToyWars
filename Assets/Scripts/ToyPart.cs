using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ToyPart : MonoBehaviour {

	[Header("Toy part")]
	public Part part;

	// Internal properties
	Action<string, Part, float> onHit;

	//

	public void SetOnHit(Action<string, Part, float> onHit) {
		this.onHit = onHit;
	}

	public void Hit(Toy damager, float damage) {
		if (onHit != null) {
			onHit("nothing at this moment", part, damage);
		}
	}

	public enum Part {
		HEAD,
		BODY,
		ARM,
		LEG
	}
}
