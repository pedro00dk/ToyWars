using UnityEngine;
using System.Collections;

public class ToyCamera : MonoBehaviour {
    private Camera _camera;

	// Use this for initialization
	void Start () {
        _camera = FindObjectOfType<Camera>();
        _camera.transform.parent = this.transform;
        _camera.transform.position = this.transform.position;//new Vector3(0,0,0);
        _camera.transform.rotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
