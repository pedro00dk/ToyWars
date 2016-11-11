using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MultipleCamerasController : NetworkBehaviour
{

    Camera camera;
	// Use this for initialization
	void Start () {
        camera = GetComponentInChildren<Camera>();
        camera.enabled = isLocalPlayer;
	}
	
	// Update is called once per frame
	void Update () {
    }
}
