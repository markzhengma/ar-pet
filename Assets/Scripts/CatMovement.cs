using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatMovement : MonoBehaviour 
{

	private Animator anim;

	public Button upBtn;
	public Button playBtn;
	public Button downBtn;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		anim.SetBool("isTail", true);
	}
	void Start ()
	{
		Button buttonOne = upBtn.GetComponent<Button> ();
		buttonOne.onClick.AddListener(StandUp);
		Button buttonTwo = playBtn.GetComponent<Button> ();
		buttonTwo.onClick.AddListener(Play);
		Button buttonThree = downBtn.GetComponent<Button> ();
		buttonThree.onClick.AddListener(LieDown);
	}

	public void Play ()
	{
		anim.SetBool("isNameF", !(anim.GetBool("isNameF")));
		anim.SetBool("isTail", !(anim.GetBool("isTail")));
		anim.SetBool("isMaruL", false);
		Debug.Log("play!");
	}

	public void StandUp ()
	{
		anim.SetBool("isTail", true);
		anim.SetBool("isNameF", false);
		anim.SetBool("isMaruL", false);
	}

	public void LieDown ()
	{
		anim.SetBool("isMaruL",!(anim.GetBool("isMaruL")));
		anim.SetBool("isTail", !(anim.GetBool("isTail")));
		anim.SetBool("isNameF", false);
	}

}