using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MultipleCamerasController : NetworkBehaviour
{
    
	// Use this for initialization
	void Start () {
        Camera camera = GetComponentInChildren<Camera>();
        camera.enabled = isLocalPlayer;
	}
	
	// Update is called once per frame
	void Update () {
    }
}
