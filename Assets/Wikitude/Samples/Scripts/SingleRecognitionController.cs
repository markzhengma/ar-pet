using UnityEngine.UI;
using Wikitude;

public class SingleRecognitionController : SampleController
{
	public ImageTracker Tracker;
	public Text InfoText;
	public Button RecognizeButton;
	public Text ButtonText;

	public void OnRecognizeClicked() {
		RecognizeButton.enabled = false;
		ButtonText.text = "Recognizing...";

		Tracker.CloudRecognitionService.Recognize();
	}

	public void OnConnectionInitialized() {
		RecognizeButton.enabled = true;
		ButtonText.text = "Recognize";
	}

	public void OnConnectionInitializationError(int errorCode, string errorMessage) {
		InfoText.text = "Error initializing cloud connection!";
	}

	public void OnRecognitionResponse(CloudRecognitionServiceResponse response) {
		RecognizeButton.enabled = true;
		ButtonText.text = "Recognize";
		if (response.Recognized) {
			InfoText.text = response.Info["name"];
		} else {
			InfoText.text = "No target recognized";
		}
	}

	public void OnRecognitionError(int errorCode, string errorMessage) {
		RecognizeButton.enabled = false;
		ButtonText.text = "Recognize";
		InfoText.text = "Recognition failed!";
	}
}
