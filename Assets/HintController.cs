using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintController : MonoBehaviour {

	public Button hintBtn;
	public Button backBtn;
	public GameObject hintContent;
	// public Button actBtn;
	public GameObject actGroup;
	public GameObject singleHint;


	void Start () {
		hintBtn.onClick.AddListener(ShowHint);
		backBtn.onClick.AddListener(ShowHint);
		// actBtn.onClick.AddListener(ShowSingleHint);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void ShowHint()
	{
		actGroup.SetActive(true);
		singleHint.SetActive(false);
		hintContent.SetActive(!hintContent.active);
	}

	public void ShowSingleHint()
	{
		actGroup.SetActive(!actGroup.active);
		singleHint.SetActive(!singleHint.active);
	}
}
