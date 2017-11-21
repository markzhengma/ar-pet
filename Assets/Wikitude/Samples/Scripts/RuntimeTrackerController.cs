using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class RuntimeTrackerController : SampleController
{
	public InputField Url;
	public GameObject TrackablePrefab;
	public GameObject CarInstructions;

	private ImageTracker _currentTracker;
	private bool _isLoadingTracker = false;

	public void OnLoadTracker() {
		if (_isLoadingTracker) {
			// Wait until previous request was completed.
			return;
		}
		if (_currentTracker != null) {
			Destroy(_currentTracker.gameObject);
		}

		_isLoadingTracker = true;

		GameObject trackerObject = new GameObject("ImageTracker");
		_currentTracker = trackerObject.AddComponent<ImageTracker>();
		_currentTracker.TargetSourceType = TargetSourceType.TargetCollectionResource;
		_currentTracker.TargetCollectionResource = new TargetCollectionResource();
		_currentTracker.TargetCollectionResource.UseCustomURL = true;
		_currentTracker.TargetCollectionResource.TargetPath = Url.text;

		_currentTracker.TargetCollectionResource.OnFinishLoading.AddListener(OnFinishLoading);
		_currentTracker.TargetCollectionResource.OnErrorLoading.AddListener(OnErrorLoading);

		_currentTracker.OnTargetsLoaded.AddListener(OnTargetsLoaded);
		_currentTracker.OnErrorLoadingTargets.AddListener(OnErrorLoadingTargets);

		GameObject trackableObject = GameObject.Instantiate(TrackablePrefab);
		trackableObject.transform.SetParent(_currentTracker.transform, false);
	}

	public override void OnTargetsLoaded() {
		base.OnTargetsLoaded();
		CarInstructions.SetActive(true);
		_isLoadingTracker = false;
	}

	public override void OnErrorLoadingTargets(int errorCode, string errorMessage) {
		base.OnErrorLoadingTargets(errorCode, errorMessage);
		_isLoadingTracker = false;
	}
}
