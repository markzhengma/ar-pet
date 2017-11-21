using UnityEngine;
using System;
using System.Collections;
using Wikitude;
using System.Runtime.InteropServices;

/// <summary>
/// Handles getting the camera frame from the device and forwarding it to the Wikitude SDK.
/// </summary>
public class SimpleInputPluginController : SampleController
{
	public WikitudeCamera WikitudeCam;

	protected WebCamTexture _feed;

	public const int FrameWidth = 640;
	public const int FrameHeight = 480;

	private int _frameDataSize = 0; 
	private int _frameIndex = 0;
	private Color32[] _pixels;

	public void OnInputPluginRegistered() {
		StartCoroutine(Initialize());
	}

	public void OnInputPluginFailure(int errorCode, string errorMessage) {
		Debug.Log("Input plugin failed with error code: " + errorCode + " message: " + errorMessage);
	}

	protected IEnumerator Initialize() {
		// Waiting for a frame can help on some devices, especially when initializing the camera when returning from  background
		yield return null;
		foreach (var device in WebCamTexture.devices) {
			if (!device.isFrontFacing) {
				_feed = new WebCamTexture(device.name, FrameWidth, FrameHeight);
				_feed.Play();
				break;
			}
		}

		if (_feed == null) {
			Debug.LogError("Could not find any cameras on the device.");
		}

		ResetBuffers(FrameWidth, FrameHeight, 4);

		// Wait a frame before getting the camera rotation, otherwise it might not be initialized yet 
		yield return null;
		if (Application.platform == RuntimePlatform.Android) {

			bool rotatedSensor = false;
			switch (Screen.orientation) {
				case ScreenOrientation.Portrait: {
					rotatedSensor = _feed.videoRotationAngle == 270;
					break;
				}
				case ScreenOrientation.LandscapeLeft: {
					rotatedSensor = _feed.videoRotationAngle == 180;
					break;
				}
				case ScreenOrientation.LandscapeRight: {
					rotatedSensor = _feed.videoRotationAngle == 0;
					break;
				}
				case ScreenOrientation.PortraitUpsideDown: {
					rotatedSensor = _feed.videoRotationAngle == 90;
					break;
				}
			}

			if (rotatedSensor) {
				// Normally, we use InvertedFrame = true, because textures in Unity are mirrored vertically, when compared with the ones the camera provides.
				// However, when we detect that the camera sensor is rotated by 180 degrees, as is the case for the Nexus 5X for example,
				// We turn off inverted frame and enable mirrored frame, which has the effect of rotating the frame upside down.
				// We use the MirroredFrame property and not the EnableMirroring property because the first one actually changes the data that
				// is being processed, while the second one only changes how the frame is rendered, leaving the frame data intact.
				WikitudeCam.InvertedFrame = false;
				WikitudeCam.MirroredFrame = true;
			}
		} 
	}

	protected virtual void ResetBuffers(int width, int height, int bytesPerPixel) {
		_frameDataSize = width * height * bytesPerPixel;
		_pixels = new Color32[width * height];

		WikitudeCam.InputFrameWidth = width;
		WikitudeCam.InputFrameHeight = height;
	}

	protected override void Update() {
		base.Update();
		if (_feed == null || !_feed.didUpdateThisFrame) {
			return;
		}

		if (_feed.width != FrameWidth || _feed.height != FrameHeight) {
			Debug.LogError("Camera feed has unexpected size.");
			return;
		}

		int newFrameDataSize = _feed.width * _feed.height * 4;
		if (newFrameDataSize != _frameDataSize) {
			ResetBuffers(_feed.width, _feed.height, 4);
		}

		_feed.GetPixels32(_pixels);
		SendNewCameraFrame();
	}

	private void SendNewCameraFrame() {
		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
			IntPtr frameData = handle.AddrOfPinnedObject();
			WikitudeCam.NewCameraFrame(++_frameIndex, _frameDataSize, frameData);
		} finally {
			if (handle != default(GCHandle)) {
				handle.Free();
			}
		}
	}

	protected virtual void Cleanup() {
		_frameDataSize = 0;
		if (_feed != null) {
			_feed.Stop();
			_feed = null;
		}
	}

	private void OnApplicationPause(bool paused) {
		if (paused) {
			Cleanup();
		} else {
			StartCoroutine(Initialize());
		}
	}

	private void OnDestroy() {
		Cleanup();
	}
}
