using UnityEngine;

public class MoveController : MonoBehaviour 
{
	private Transform _activeObject = null;
	
	private Vector3 _startObjectPosition;
	private Vector2 _startTouchPosition;
	private Vector2 _touchOffset;

	private InstantTrackingController _controller;

	private void Start() {
		_controller = GetComponent<InstantTrackingController>();
	}

	public Transform ActiveObject {
		get {
			return _activeObject;
		}
	}

	public void SetMoveObject(Transform newMoveObject) {
		if (_controller.ActiveModels.Contains(newMoveObject.gameObject)) {
			_activeObject = newMoveObject;
			_startObjectPosition = _activeObject.position;
			_startTouchPosition = Input.GetTouch(0).position;
			_touchOffset = Camera.main.WorldToScreenPoint(_startObjectPosition);
		}
	}

	void Update () {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			RaycastHit hit;

			if (_activeObject == null) {
				if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit)) {
					SetMoveObject(hit.transform);
				}
			}

			if (_activeObject != null) {
				var screenPosForRay = (touch.position - _startTouchPosition) + _touchOffset;
				Ray cameraRay = Camera.main.ScreenPointToRay(screenPosForRay);
				Plane p = new Plane(Vector3.up, Vector3.zero);
				
				float enter;
				if (p.Raycast(cameraRay, out enter)) {
					var position = cameraRay.GetPoint(enter);
					
					position.x = Mathf.Clamp(position.x, -15.0f, 15.0f);
					position.y = 0.0f;
					position.z = Mathf.Clamp(position.z, -15.0f, 15.0f);
					
					_activeObject.position = Vector3.Lerp(_activeObject.position, position, 0.25f);
				}
			}
		} else {
			_activeObject = null;
		}
	}
}
