using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class ExtendedTrackingController : SampleController 
{
	public ImageTracker Tracker;

	public Text TrackingQualityText;
	public Image TrackingQualityBackground;
	public GameObject StopExtendedTrackingButton;

	protected override void Start() {
		base.Start();
		StopExtendedTrackingButton.SetActive(false);
	}

	public void OnStopExtendedTrackingButtonPressed() {
		Tracker.StopExtendedTracking();
		StopExtendedTrackingButton.SetActive(false);
	}

	public void OnExtendedTrackingQualityChanged(string targetName, ExtendedTrackingQuality newQuality) {
		if (targetName == null) {
			TrackingQualityText.text = "No target";
			TrackingQualityBackground.color = Color.red;
		} else {
			switch (newQuality) {
			case ExtendedTrackingQuality.Bad:
				TrackingQualityText.text = "Target: " + targetName + " Quality: Bad";
				TrackingQualityBackground.color = Color.red;
				break;
			case ExtendedTrackingQuality.Average:
				TrackingQualityText.text = "Target: " + targetName + " Quality: Average";
				TrackingQualityBackground.color = Color.yellow;
				break;
			case ExtendedTrackingQuality.Good:
				TrackingQualityText.text = "Target: " + targetName + " Quality: Good";
				TrackingQualityBackground.color = Color.green;
				break;
			default:
				break;
			}
		}
	}

	public void OnEnterFieldOfVision(string target) {
		StopExtendedTrackingButton.SetActive(true);
	}

	public void OnExitFieldOfVision(string target) {
		TrackingQualityText.text = "Target lost";
		TrackingQualityBackground.color = Color.red;
	}
}
