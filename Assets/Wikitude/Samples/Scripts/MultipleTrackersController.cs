using UnityEngine;
using Wikitude;

public class MultipleTrackersController : SampleController 
{
	public ImageTracker CarTracker;
	public ImageTracker MagazineTracker;

	public GameObject CarInstructions;
	public GameObject MagazineInstructions;

	private bool _waitingForTrackerToLoad = false;

	public void OnTrackCar() {
		if (!CarTracker.enabled && !_waitingForTrackerToLoad) {
			_waitingForTrackerToLoad = true;

			MagazineInstructions.SetActive(false);
			CarInstructions.SetActive(true);

			CarTracker.enabled = true;
		}
	}

	public void OnTrackMagazine() {
		if (!MagazineTracker.enabled && !_waitingForTrackerToLoad) {
			_waitingForTrackerToLoad = true;
			MagazineInstructions.SetActive(true);

			CarInstructions.SetActive(false);

			MagazineTracker.enabled = true;
		}
	}

	public override void OnTargetsLoaded() {
		_waitingForTrackerToLoad = false;
	}
}
