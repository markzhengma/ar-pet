using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CatMovement : MonoBehaviour 
{

	private Animator anim;

	public Button upBtn;
	public Button playBtn;
	public Button downBtn;

	public Text counterText;

	float tempTime;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		// anim.SetBool("stand", true);
	}

	void OnMouseDown ()
	{
		if(anim.GetBool("stand") == true)
		{
			StandReact ();
		}
		if(anim.GetBool("lie") == true)
		{
			LieReact ();
		}
		if(anim.GetBool("sit") == true)
		{
			SitReact ();
		}
		if(anim.GetBool("down") == true)
		{
			DownReact ();
		}
		if(anim.GetBool("back") == true)
		{
			BackReact ();
		}
	}

	void Start ()
	{
		// Button buttonOne = upBtn.GetComponent<Button> ();
		// buttonOne.onClick.AddListener(Stand);
		// Button buttonTwo = playBtn.GetComponent<Button> ();
		// buttonTwo.onClick.AddListener(Sit);
		// Button buttonThree = downBtn.GetComponent<Button> ();
		// buttonThree.onClick.AddListener(Curl);
		if(int.Parse(counterText.text) < 10)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", true);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 10 && int.Parse(counterText.text) < 20)
			{
				anim.SetBool("stand", true);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 20 && int.Parse(counterText.text) < 30)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", true);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 30 && int.Parse(counterText.text) < 40)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", true);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 40)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", true);
			}
	}

	void Update()
	{
		tempTime += Time.deltaTime;
        if (tempTime > 2.0) 
		{
        	tempTime = 0;
			if(int.Parse(counterText.text) < 10)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", true);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 10 && int.Parse(counterText.text) < 20)
			{
				anim.SetBool("stand", true);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 20 && int.Parse(counterText.text) < 30)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", true);
				anim.SetBool("down", false);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 30 && int.Parse(counterText.text) < 40)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", true);
				anim.SetBool("back", false);
			}
			if(int.Parse(counterText.text) >= 40)
			{
				anim.SetBool("stand", false);
				anim.SetBool("lie", false);
				anim.SetBool("sit", false);
				anim.SetBool("down", false);
				anim.SetBool("back", true);
			}
			if(anim.GetBool("stand") == true && anim.GetBool("standReact") == false)
			{
            	StandRandom ();
			}
			if(anim.GetBool("stand") == true && anim.GetBool("standReact") == true)
			{
				StandReactRandom ();
			}
			if(anim.GetBool("lie") == true && anim.GetBool("lieReact") == false)
			{
				LieRandom ();
			}
			if(anim.GetBool("sit") == true && anim.GetBool("sitReact") == false)
			{
				SitRandom ();
			}
			if(anim.GetBool("down") == true && anim.GetBool("downReact") == false)
			{
				DownRandom ();
			}
			if(anim.GetBool("back") == true && anim.GetBool("backReact") == false)
			{
				Debug.Log("Your cat wants to play with you!");
			}

         }
	}

	private void StandRandom ()
	{
		anim.SetInteger("standSelect", Random.Range(0, 3));
	}

	private void StandReactRandom ()
	{
		anim.SetInteger("standReactSelect", Random.Range(1, 4));
		Invoke("ResetStand", 2);
	}

	private void ResetStand()
	{
		anim.SetBool("standReact", false);
		anim.SetInteger("standReactSelect", 0);
	}

	private void StandReact ()
	{
		if(anim.GetBool("standReact") == false)
		{
			GameObject firebaseLogic = GameObject.Find("FirebaseLogic");
			FirebaseClickHandler firebaseClickHandler = firebaseLogic.GetComponent<FirebaseClickHandler>();
			firebaseClickHandler.IncrementClickCounter();
		}
		anim.SetBool("standReact", true);
	}

	private void LieRandom ()
	{
		anim.SetInteger("lieSelect", Random.Range(0, 2));
	}

	private void LieReact ()
	{
		if(anim.GetBool("lieReact") == false)
		{
			GameObject firebaseLogic = GameObject.Find("FirebaseLogic");
			FirebaseClickHandler firebaseClickHandler = firebaseLogic.GetComponent<FirebaseClickHandler>();
			firebaseClickHandler.IncrementClickCounter();
		}
		anim.SetBool("lieReact", true);
		Invoke("ResetLie", 3);
	}

	private void ResetLie()
	{
		anim.SetBool("lieReact", false);
	}

	private void SitRandom ()
	{
		anim.SetInteger("sitSelect", Random.Range(0, 3));
	}

	private void SitReact ()
	{
		if(anim.GetBool("sitReact") == false)
		{
			GameObject firebaseLogic = GameObject.Find("FirebaseLogic");
			FirebaseClickHandler firebaseClickHandler = firebaseLogic.GetComponent<FirebaseClickHandler>();
			firebaseClickHandler.IncrementClickCounter();
		}
		anim.SetBool("sitReact", true);
		Invoke("ResetSit", 3);
	}

	private void ResetSit()
	{
		anim.SetBool("sitReact", false);
	}

	private void DownRandom ()
	{
		anim.SetInteger("downSelect", Random.Range(0, 2));
	}

	private void DownReact ()
	{
		if(anim.GetBool("downReact") == false)
		{
			GameObject firebaseLogic = GameObject.Find("FirebaseLogic");
			FirebaseClickHandler firebaseClickHandler = firebaseLogic.GetComponent<FirebaseClickHandler>();
			firebaseClickHandler.IncrementClickCounter();
		}
		anim.SetBool("downReact", true);
		Invoke("ResetDown", 3);
	}

	private void ResetDown()
	{
		anim.SetBool("downReact", false);
	}

	private void BackReact ()
	{
		if(anim.GetBool("backReact") == false)
		{
			GameObject firebaseLogic = GameObject.Find("FirebaseLogic");
			FirebaseClickHandler firebaseClickHandler = firebaseLogic.GetComponent<FirebaseClickHandler>();
			firebaseClickHandler.IncrementClickCounter();
		}
		anim.SetBool("backReact", true);
		Invoke("ResetBack", 3);
	}

	private void ResetBack()
	{
		anim.SetBool("backReact", false);
	}
	

	// public void Stand ()
	// {
	// 	anim.SetBool("stand", true);
	// 	anim.SetBool("sit", false);
	// 	anim.SetBool("curl", false);
	// 	anim.SetBool("lie", false);
	// 	anim.SetBool("down", false);
	// }
	// public void Sit ()
	// {
	// 	anim.SetBool("stand", false);
	// 	anim.SetBool("sit", true);
	// 	anim.SetBool("curl", false);
	// 	anim.SetBool("lie", false);
	// 	anim.SetBool("down", false);
	// }
	// public void Curl ()
	// {
	// 	anim.SetBool("stand", false);
	// 	anim.SetBool("sit", false);
	// 	anim.SetBool("curl", true);
	// 	anim.SetBool("lie", false);
	// 	anim.SetBool("down", false);
	// }
	// public void Lie ()
	// {
	// 	anim.SetBool("stand", false);
	// 	anim.SetBool("sit", false);
	// 	anim.SetBool("curl", false);
	// 	anim.SetBool("lie", true);
	// 	anim.SetBool("down", false);
	// }
	// public void Down ()
	// {
	// 	anim.SetBool("stand", false);
	// 	anim.SetBool("sit", false);
	// 	anim.SetBool("curl", false);
	// 	anim.SetBool("lie", false);
	// 	anim.SetBool("down", true);
	// }

}