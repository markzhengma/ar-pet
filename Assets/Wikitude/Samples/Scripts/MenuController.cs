using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using UnityEngine.UI;
using Wikitude;

public class MenuController : MonoBehaviour
{
	public GameObject InfoPanel;

	public Text VersionNumberText;
	public Text BuildDateText;
	public Text BuildNumberText;
	public Text BuildConfigurationText;
	public Text UnityVersionText;

	public void OnSampleButtonClicked(Button sender) {
		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(sender.name);
		#else
		Application.LoadLevel(sender.name);
		#endif
	}

	public void OnInfoButtonPressed() {
		InfoPanel.SetActive(true);

		var buildInfo = WikitudeSDK.BuildInformation;
		VersionNumberText.text = buildInfo.SDKVersion;
		BuildDateText.text = buildInfo.BuildDate;
		BuildNumberText.text = buildInfo.BuildNumber;
		BuildConfigurationText.text = buildInfo.BuildConfiguration;
		UnityVersionText.text = Application.unityVersion;
	}

	public void OnInfoDoneButtonPressed() {
		InfoPanel.SetActive(false);
	}

	void Update() {
		// Also handles the back button on Android
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			Application.Quit();
		}
	}
}