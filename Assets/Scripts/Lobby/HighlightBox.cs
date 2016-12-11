using UnityEngine;
using System.Collections;

public class HighlightBox : MonoBehaviour {

    public GameObject box0;
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;

    PlayerSelector controller;

	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<PlayerSelector>();
	}
	
	// Update is called once per frame
	void Update () {
        box0.SetActive(controller.selectedPlayer == 0);
        box1.SetActive(controller.selectedPlayer == 1);
        box2.SetActive(controller.selectedPlayer == 2);
        box3.SetActive(controller.selectedPlayer == 3);
    }



}
