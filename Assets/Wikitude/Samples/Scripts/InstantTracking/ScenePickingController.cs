using UnityEngine;
using UnityEngine.UI;
using Wikitude;
using System.Collections.Generic;

public class ScenePickingController : SampleController 
{
	public InstantTracker Tracker;
	public GameObject Augmentation;

	public Button ToggleStateButton;
	public Text ToggleStateButtonText;

	private InstantTrackingState _currentTrackerState = InstantTrackingState.Initializing;
	private bool _changingState = false;
	private GridRenderer _gridRenderer;
	private List<GameObject> _augmentations = new List<GameObject>();
	private InstantTrackable _trackable;
	private bool _isTracking = false;
	

	private void Awake() {
		_gridRenderer = GetComponent<GridRenderer>();
		_gridRenderer.enabled = false;

		_trackable = Tracker.GetComponentInChildren<InstantTrackable>();
		Tracker.OnScreenConversionComputed.AddListener(OnScreenConversionComputed);
	}

	public void OnStateChanged(InstantTrackingState newState) {
		_currentTrackerState = newState;

		if (_currentTrackerState == InstantTrackingState.Initializing) {
			ToggleStateButtonText.text = "Start Tracking";
		} else {
			ToggleStateButtonText.text = "Start Initialization";
		}

		_changingState = false;
	}

	public void OnScreenConversionComputed(bool success, Vector2 screenCoordinate, Vector3 pointCloudCoordinate) {
		if (success) {
			var newAugmentation = GameObject.Instantiate(Augmentation, _trackable.transform) as GameObject;
			// The pointCloudCoordinate values are in the local space of the trackable.
			newAugmentation.transform.localPosition = pointCloudCoordinate;
			newAugmentation.transform.localScale = Vector3.one;
			_augmentations.Add(newAugmentation);
		}
	}

	public void OnToggleStateButtonPressed() {
		if (!_changingState) {
			ToggleStateButtonText.text = "Switching State...";
			_changingState = true;

			if (_currentTrackerState == InstantTrackingState.Initializing) {
				Tracker.SetState(InstantTrackingState.Tracking);
			} else {
				// Clear all the previous augmentations
				foreach (var augmentation in _augmentations) {
					Destroy(augmentation);
				}
				_augmentations.Clear();

				Tracker.SetState(InstantTrackingState.Initializing);
			}
		}
	}

	public void OnInitializationStarted(InstantTarget target) {
		SetSceneEnabled(true);
	}

	public void OnInitializationStopped(InstantTarget target) {
		SetSceneEnabled(false);
	}

	public void OnSceneRecognized(InstantTarget target) {
		SetSceneEnabled(true);
		_isTracking = true;
	}

	public void OnSceneLost(InstantTarget target) {
		SetSceneEnabled(false);
		_isTracking = false;
	}

	private void SetSceneEnabled(bool enabled) {
		_gridRenderer.enabled = enabled;
		// Because the InstantTrackable has the Auto Toggle Visibility option enabled
		// and because all the augmentations are set as children to it, we don't need to hide them.
	}

	protected override void Update() {
		base.Update();
		if (_isTracking && Input.GetMouseButtonUp(0)) {
			Tracker.ConvertScreenCoordinate(Input.mousePosition);
		}
	}
}
