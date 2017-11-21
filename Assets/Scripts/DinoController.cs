using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour {

	private InstantTrackingController trackerScript;
	private GameObject ButtonsParent;

	// Use this for initialization
	void Start () {

		trackerScript = GameObject.Find ("Controller").gameObject.GetComponent<InstantTrackingController> ();
		ButtonsParent = GameObject.Find ("Buttons Parent");

		trackerScript._gridRenderer.enabled = false;
		ButtonsParent.SetActive (false);
		
	}

	void OnEnable(){
		trackerScript._gridRenderer.enabled = false;
		ButtonsParent.SetActive (false);
	}

	void OnDisable(){
		ButtonsParent.SetActive (true);
	}

}
