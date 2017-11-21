using UnityEngine;
using System;
using System.Collections;
using Wikitude;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// Handles forwarding the camera frame to the custom renderer.
/// </summary>
public class CustomCameraController : SampleController
{
	private struct InputFrameData {
		public long Index;
		public Texture2D Texture;

		public InputFrameData(long index, Texture2D texture) {
			Index = index;
			Texture = texture;
		}
	}

	public WikitudeCamera WikitudeCam;

	protected WebCamTexture _feed;

	public const int FrameWidth = 640;
	public const int FrameHeight = 480;

	private int _frameDataSize = 0; 
	private int _frameIndex = 0;
	
	private int _bufferWriteIndex = 0;
	private int _bufferReadIndex = 0;
	private int _bufferCount = 5;
	private List<InputFrameData> _ringBuffer;
	private Color32[] _colorData;

	public CustomCameraRenderer Renderer;

	public void OnInputPluginRegistered() {
		StartCoroutine(Initialize(true));
	}

	public void OnInputPluginFailure(int errorCode, string errorMessage) {
		Debug.Log("Input plugin failed with error code: " + errorCode + " message: " + errorMessage);
	}

	public void OnEnterFieldOfVision(string targetName) {
		Renderer.IsEffectVisible = false;
	}

	public void OnExitFieldOfVision(string targetName) {
		Renderer.IsEffectVisible = true;
	}

	private IEnumerator Initialize(bool firstStart) {
		if (!firstStart) {
			// If we are resuming from background, we wait a frame to make sure that everything is initialized
			// before starting the camera again.
			yield return null;
		}

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

				// Additionally, because we are doing the rendering in Unity, we need to instruct the renderer to flip the image.
				Renderer.FlipImage = true;
			}
		} 
	}

	private void ResetBuffers(int width, int height, int bytesPerPixel) {
		_frameDataSize = width * height * bytesPerPixel;
		_ringBuffer = new List<InputFrameData>(10);
		for (int i = 0; i < _bufferCount; ++i) {
			_ringBuffer.Add(new InputFrameData(-1 , new Texture2D(width, height)));
		}

		_colorData = new Color32[width * height];

		WikitudeCam.InputFrameWidth = width;
		WikitudeCam.InputFrameHeight = height;

		Renderer.CurrentFrame = _ringBuffer[0].Texture;
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

		_feed.GetPixels32(_colorData);
		_ringBuffer[_bufferWriteIndex].Texture.SetPixels32(_colorData);
		_ringBuffer[_bufferWriteIndex].Texture.Apply();
		SendNewCameraFrame();
		var data = _ringBuffer[_bufferWriteIndex];
		data.Index = _frameIndex;
		_ringBuffer[_bufferWriteIndex] = data;
		
		long presentableIndex = WikitudeCam.GetPresentableInputFrameIndex();
		// Default to the last written buffer
		_bufferReadIndex = _bufferWriteIndex;
		if (presentableIndex != -1) {
			for (int i = 0; i < _bufferCount; ++i) {
				if (_ringBuffer[i].Index == presentableIndex) {
					_bufferReadIndex = i;
				}
			}
		}

		Renderer.CurrentFrame = _ringBuffer[_bufferReadIndex].Texture;
		_bufferWriteIndex = (_bufferWriteIndex + 1) % _bufferCount;
	}

	private void SendNewCameraFrame() {
		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc(_colorData, GCHandleType.Pinned);
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

		if (Renderer) {
			Renderer.CurrentFrame = null;
		}
	}

	private void OnApplicationPause(bool paused) {
		if (paused) {
			Cleanup();
		} else {
			StartCoroutine(Initialize(false));
		}
	}

	private void OnDestroy() {
		Cleanup();
	}
}
