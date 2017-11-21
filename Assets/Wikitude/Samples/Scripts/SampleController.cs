using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleController : MonoBehaviour 
{
	protected virtual void Start() {
		QualitySettings.shadowDistance = 60.0f;
	}
	
	public virtual void OnBackButtonClicked() {
		SceneManager.LoadScene("Main Menu");
	}

	// URLResource events
	public virtual void OnFinishLoading() {
		Debug.Log("URL Resource loaded successfully.");
	}

	public virtual void OnErrorLoading(int errorCode, string errorMessage) {
		Debug.LogError("Error loading URL Resource!\nErrorCode: " + errorCode + "\nErrorMessage: " + errorMessage);
	}

	// Tracker events
	public virtual void OnTargetsLoaded() {
		Debug.Log("Targets loaded successfully.");
	}

	public virtual void OnErrorLoadingTargets(int errorCode, string errorMessage) {
		Debug.LogError("Error loading targets!\nErrorCode: " + errorCode + "\nErrorMessage: " + errorMessage);
	}

	protected virtual void Update() {
		// Also handles the back button on Android
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			OnBackButtonClicked();
		}
	}
}
