using UnityEngine;
using UnityEngine.UI;
using Wikitude;
using System.Collections;

public class CameraSettingsController : SampleController 
{
	public WikitudeCamera Camera;
	public ImageTracker CurrentImageTracker;
	public GameObject ConfirmationTab;
	public GameObject ControlsLayout;
	public Dropdown PositionDropdown;
	public Dropdown FocusModeDropdown;
	public GameObject AutoFocusRestrictionLayout;
	public Dropdown AutoFocusRestrictionDropdown;
	public Dropdown FlashModeDropdown;
	public GameObject ZoomLayout;
	public Scrollbar ZoomScrollbar;
	public GameObject ManualFocusLayout;

	public GameObject WikitudeCameraPrefab;
	public GameObject ImageTrackerPrefab;
	public GameObject WikitudeEyePrefab;

	protected override void Start() {
		base.Start();
		PositionDropdown.value = (int)Camera.DevicePosition;
		FocusModeDropdown.value = (int)Camera.FocusMode;
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			AutoFocusRestrictionDropdown.value = (int)Camera.AutoFocusRestriction;
		} else {
			AutoFocusRestrictionLayout.SetActive(false);
		}
		FlashModeDropdown.value = (int)Camera.FlashMode;
		if (Mathf.Approximately(Camera.MaxZoomLevel, 1.0f)) {
			ZoomLayout.SetActive(false);
		} else {
			ZoomScrollbar.value = (Camera.ZoomLevel - 1.0f) / (Camera.MaxZoomLevel - 1.0f);
		}
		ManualFocusLayout.SetActive(false);

	}

	public void OnCameraControlsButtonClicked() {
		ControlsLayout.SetActive(!ControlsLayout.activeSelf);
	}

	public void OnPositionChanged(int newPosition) {
		Camera.DevicePosition = (CaptureDevicePosition)newPosition;
	}

	public void OnFocusModeChanged(int newFocusMode) {
		Camera.FocusMode = (CaptureFocusMode)newFocusMode;

		if (Camera.FocusMode == CaptureFocusMode.Locked) {
			if (Camera.IsManualFocusAvailable) {
				ManualFocusLayout.SetActive(true);
			}
		} else {
			ManualFocusLayout.SetActive(false);
		}
	}

	public void OnAutoFocusChanged(int newAutoFocus) {
		Camera.AutoFocusRestriction = (CaptureAutoFocusRestriction)newAutoFocus;
	}

	public void OnFlashModeChanged(int newFlashMode) {
		Camera.FlashMode = (CaptureFlashMode)newFlashMode;
	}

	public void OnZoomLevelChanged(float newZoomLevel) {
		Camera.ZoomLevel = newZoomLevel * (Camera.MaxZoomLevel - 1.0f) + 1.0f;
	}

	public void OnManualFocusChanged(float manualFocus) {
		Camera.ManualFocusDistance = manualFocus;
	}

	public void OnCameraFailure() {
		// Try to restart using Camera 1 API if running on Android
		// WikitudeCamera properties cannot be changed while it is running,
		// so we need to destroy it and create it back from prefabs.
		if (Application.platform == RuntimePlatform.Android && Camera.EnableCamera2) {
			ShowRestartConfirmationTab();
		}
	}

	private void ShowRestartConfirmationTab() {
		ConfirmationTab.SetActive(true);
	}

	private void HideRestatConfirmationTab() {
		ConfirmationTab.SetActive(false);
	}

	public void OnRestartButtonPressed() {
		HideRestatConfirmationTab();
		StartCoroutine(Restart());
	}

	public void OnCancelRestartButtonPressed() {
		HideRestatConfirmationTab();
	}

	private IEnumerator Restart() {
		Destroy(CurrentImageTracker.gameObject);
		Destroy(Camera.gameObject);
		
		// Wait a frame before recreating everything again
		yield return null;

		// WikitudeCamera prefab has Camera 1 API enabled by default
		// If that were not the case, it could be changed on the prefab,
		// before creating the actual GameObject
		Camera = GameObject.Instantiate(WikitudeCameraPrefab).GetComponent<WikitudeCamera>();
		CurrentImageTracker = GameObject.Instantiate(ImageTrackerPrefab).GetComponent<ImageTracker>();
		GameObject.Instantiate(WikitudeEyePrefab).transform.SetParent(CurrentImageTracker.transform.GetChild(0));
	}
}
