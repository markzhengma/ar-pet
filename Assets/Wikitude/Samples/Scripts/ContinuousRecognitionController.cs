using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class ContinuousRecognitionController : SampleController 
{
	public ImageTracker Tracker;
	public Text buttonText;
	private bool _trackerRunning = false;
	private bool _connectionInitialized = false;
	
	private double _recognitionInterval = 1.5;

	#region UI Events
	public void OnToggleClicked() {
		_trackerRunning = !_trackerRunning;
		ToggleContinuousCloudRecognition(_trackerRunning);
	}
	#endregion

	#region Tracker Events
	public void OnInitialized() {
		base.OnTargetsLoaded();
		_connectionInitialized = true;
	}

	public void OnInitializationError(int errorCode, string errorMessage) {
		Debug.Log("Error initializing cloud connection!");
	}

	public void OnRecognitionResponse(CloudRecognitionServiceResponse response) {
		if (response.Recognized) {
			// If the cloud recognized a target, we stop continuous recognition and track that target locally
			ToggleContinuousCloudRecognition(false);
		}
	}

	public void OnInterruption(double suggestedInterval) {
		_recognitionInterval = suggestedInterval;
		StartContinuousCloudRecognition();
	}
	#endregion

	private void ToggleContinuousCloudRecognition(bool enabled) {
		if (Tracker != null && _connectionInitialized) {
			if (enabled) {
				buttonText.text = "Scanning";
				StartContinuousCloudRecognition();
			} else {
				buttonText.text = "Press to start scanning";
				StopContinuousCloudRecognition();
			}
			_trackerRunning = enabled;
		}
	}

	private void StartContinuousCloudRecognition() {
		Tracker.CloudRecognitionService.StartContinuousRecognition(_recognitionInterval);
	}

	private void StopContinuousCloudRecognition() {
		Tracker.CloudRecognitionService.StopContinuousRecognition();
	}
}
