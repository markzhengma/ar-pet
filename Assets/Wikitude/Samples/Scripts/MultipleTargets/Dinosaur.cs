using UnityEngine;
using System.Collections;

public class Dinosaur : MonoBehaviour 
{
	private const string AttackParameterName = "Attack";
	private const string CelebrateParameterName = "Celebrate";
	private const string HitParameterName = "Hit";
	private const string WalkingSpeedParameterName = "Walking Speed";
    private const float AngleThreshold = 1.0f;
    private const float WalkingSpeed = 1.0f;
    private const float ToWalkingTransitionTime = 0.5f;
    private const float DistanceThreshold = 6.0f;

	public enum State {
		Idle,
		RotateToTarget,
		MoveToTarget,
		WaitingForAttacker,
		Fight,
		Defeated,
		Celebrate,
		MoveToOrigin
	}

	private Coroutine _sequenceCoroutine = null;
	private Coroutine _walkingSpeedCoroutine = null;
	public Dinosaur TargetDinosaur {
		get;
		private set;
	}
	public Dinosaur AttackingDinosaur {
		get;
		private set;
	}
	private Animator _animator = null; 

	public float RotationSpeed = 140.0f;
	public float MovementSpeed = 5.0f;

	public bool InBattle {
		get;
		private set;
	}

	private State _currentState;
	public State CurrentState {
		get {
			return _currentState;
		}
		private set {
			_currentState = value;
		}
	}

	public void Attack(Dinosaur targetDinosaur) {
		TargetDinosaur = targetDinosaur;
		TargetDinosaur.DefendFrom(this);
		InBattle = true;
		_sequenceCoroutine = StartCoroutine(StartAttackSequence());
	}

	private IEnumerator StartAttackSequence() {
		CurrentState = State.RotateToTarget;
		yield return RotateTowards(TargetDinosaur.transform);
		CurrentState = State.MoveToTarget;
		yield return MoveTowards(TargetDinosaur.transform);
		
		while (TargetDinosaur.CurrentState != State.WaitingForAttacker) {
			yield return null;
		}

		Attack();
		while (TargetDinosaur.CurrentState != State.Defeated) {
			yield return null;
		}

		Celebrate();

		while (CurrentState != State.MoveToOrigin) {
			yield return null;
		}

		yield return StartCoroutine(WalkBackSequence());
	}

	private IEnumerator WalkBackSequence() {
		yield return RotateTowards(transform.parent);
		yield return MoveTowards(transform.parent, 0.1f);
		SetWalkingSpeed(0.0f, 0.2f);
		CurrentState = State.Idle;
		InBattle = false;
	}

	public void DefendFrom(Dinosaur attackingDinosaur) {
		AttackingDinosaur = attackingDinosaur;
		InBattle = true;
		_sequenceCoroutine = StartCoroutine(StartDefendSequence());
	}

	private void StopCoroutines() {
		if (_sequenceCoroutine != null) {
			StopCoroutine(_sequenceCoroutine);
		}
		if (_walkingSpeedCoroutine != null) {
			StopCoroutine(_walkingSpeedCoroutine);
		}
	}

	public void OnAttackerDisappeared() {
		StopCoroutines();
		if (CurrentState != State.Defeated) {
			CurrentState = State.Idle;
			InBattle = false;
		}
	}

	public void OnTargetDisappeared() {		
		StopCoroutines();

		StartCoroutine(WalkBackSequence());
	}

	private IEnumerator StartDefendSequence() {
		CurrentState = State.RotateToTarget;
		yield return RotateTowards(AttackingDinosaur.transform);
		SetWalkingSpeed(0.0f, 0.5f);
		CurrentState = State.WaitingForAttacker;
		while (CurrentState != State.Fight) {
			yield return null;
		}
		_animator.SetTrigger(HitParameterName);
	}

	private void Attack() {
		_animator.SetTrigger(AttackParameterName);
		CurrentState = State.Fight;
	}



	private void Hit() {
		CurrentState = State.Fight;
	}

	private void Celebrate() {
		_animator.SetTrigger(CelebrateParameterName);
	}

	private void OnAttackAnimationEvent() {
		TargetDinosaur.Hit();
	}

	private void OnDefeatedAnimationEvent() {
		CurrentState = State.Defeated;
	}

	private void OnCelebrateEndAnimationEvent() {
		CurrentState = State.MoveToOrigin;
	}

	private float GetFacingAngleToTarget(Transform target) {
		var direction = Quaternion.FromToRotation(transform.forward, (target.position - transform.position).normalized);
		return direction.eulerAngles.y;
	}

    private IEnumerator RotateTowards(Transform rotationTarget) {
		
		var targetRotation = Quaternion.LookRotation((rotationTarget.position - transform.position).normalized, transform.up);
		var angleToTarget = Quaternion.Angle(targetRotation, transform.rotation);

		if (angleToTarget > AngleThreshold) {
			SetWalkingSpeed(WalkingSpeed, ToWalkingTransitionTime);

			while (angleToTarget > AngleThreshold && rotationTarget != null) {
				float maxAngle = RotationSpeed * Time.deltaTime;
				float maxT = Mathf.Min(1.0f, maxAngle / angleToTarget);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxT);

				targetRotation = Quaternion.LookRotation((rotationTarget.position - transform.position).normalized, transform.up);
				angleToTarget = Quaternion.Angle(targetRotation, transform.rotation);

				yield return null;
			}
		}
	}

	private float GetDistanceToTarget(Transform target) {
		return (transform.position - target.position).magnitude;
	}

    private IEnumerator MoveTowards(Transform moveTarget, float distanceThreshold = DistanceThreshold) {
		float distanceToTarget = GetDistanceToTarget(moveTarget);
		if (distanceToTarget > DistanceThreshold) {
			SetWalkingSpeed(WalkingSpeed, ToWalkingTransitionTime);
			while (distanceToTarget > distanceThreshold && moveTarget != null) {
				transform.LookAt(moveTarget);

				Vector3 direction = (moveTarget.position - transform.position).normalized;
				transform.position += direction * MovementSpeed * Time.deltaTime;
				distanceToTarget = GetDistanceToTarget(moveTarget);
				
				yield return null;
			}
		}
	}

	private void SetWalkingSpeed(float walkingSpeed, float transitionTime) {
		_walkingSpeedCoroutine = StartCoroutine(SetWalkingSpeedCoroutine(walkingSpeed, transitionTime));
	}

	private IEnumerator SetWalkingSpeedCoroutine(float walkingSpeed, float transitionTime) {
		float startingSpeed = _animator.GetFloat(WalkingSpeedParameterName);
		float currentTime = 0.0f;
		while (currentTime < transitionTime) {
			_animator.SetFloat(WalkingSpeedParameterName, Mathf.Lerp(startingSpeed, walkingSpeed, currentTime / transitionTime));
			currentTime += Time.deltaTime;
			yield return null;
		}
		_animator.SetFloat(WalkingSpeedParameterName, walkingSpeed);
	}

	private void Awake() {
		_animator = GetComponent<Animator>();
		CurrentState = State.Idle;
	}
	
	private void Update() {
	
	}
}
