using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocatorController : MonoBehaviour {

	public float speed;

	public Button startBtn;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startBtn.onClick.AddListener(BallMove);
	}
	
	public void BallMove ()
	{
		// float moveHorizontal = Input.GetAxis ("Horizontal");
		// float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (0.0f, 0.0f, 1.0f);

		rb.AddForce (movement * speed);
	}

}