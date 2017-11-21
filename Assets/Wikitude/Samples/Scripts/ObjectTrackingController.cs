using UnityEngine;
using Wikitude;

public class ObjectTrackingController : SampleController 
{
	private const string MarkerObjectName = "marker";
	private const string SirenMarkerObjectName = "marker_siren";
	private const string PlayTriggerName = "Play Instructions";
	private const string SirenTriggerName = "Play Siren";
	private const string IdleTriggerName = "Play Idle";

	private bool _isInstructionsAnimationPlaying = false;
	private bool _isSirenAnimationPlaying = false;

	protected override void Start() {
		QualitySettings.shadowDistance = 8.0f;
	}

	public void OnObjectRecognized(ObjectTarget recognizedTarget) {
		_isInstructionsAnimationPlaying = false;
		_isSirenAnimationPlaying = false;
	}

	protected override void Update() {
		base.Update();

		if (Input.GetMouseButtonUp(0)) {
			var touchRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(touchRay, out hitInfo)) {
				if (hitInfo.collider.gameObject.name == MarkerObjectName) {
					var instructions = hitInfo.collider.transform.parent.gameObject;
					var animator = instructions.GetComponent<Animator>();
					if (!_isInstructionsAnimationPlaying) {
						animator.SetTrigger(PlayTriggerName);
						animator.ResetTrigger(IdleTriggerName);
					} else {
						animator.SetTrigger(IdleTriggerName);
						animator.ResetTrigger(PlayTriggerName);
					}
					_isInstructionsAnimationPlaying = !_isInstructionsAnimationPlaying;
				} else if (hitInfo.collider.gameObject.name == SirenMarkerObjectName) {
					var sirenAnimator = hitInfo.collider.transform.parent.GetComponent<Animator>();
					if (!_isSirenAnimationPlaying) {
						sirenAnimator.SetTrigger(SirenTriggerName);
						sirenAnimator.ResetTrigger(IdleTriggerName);
					} else {
						sirenAnimator.SetTrigger(IdleTriggerName);
						sirenAnimator.ResetTrigger(SirenTriggerName);
					}
					_isSirenAnimationPlaying = !_isSirenAnimationPlaying;
				}
			}
		}
	}
}
