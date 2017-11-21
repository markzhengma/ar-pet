using System.Collections.Generic;
using Wikitude;

public class MultipleTargetsController : SampleController 
{
	private HashSet<Dinosaur> _visibleDinosaurs = new HashSet<Dinosaur>();

	public void OnImageRecognized(ImageTarget target) {
		_visibleDinosaurs.Add(target.Drawable.transform.GetChild(0).GetComponent<Dinosaur>());
	}

	public void OnImageLost(ImageTarget target) {
		var lostDinosaur = target.Drawable.transform.GetChild(0).GetComponent<Dinosaur>();
		_visibleDinosaurs.Remove(lostDinosaur);

		foreach (var dinosaur in _visibleDinosaurs) {
			if (dinosaur.AttackingDinosaur == lostDinosaur) {
				dinosaur.OnAttackerDisappeared();
			} else if (dinosaur.TargetDinosaur == lostDinosaur) {
				dinosaur.OnTargetDisappeared();
			}
		}
	}

	protected override void Update() {
		base.Update();

		if (_visibleDinosaurs.Count > 1) {
			Dinosaur availableDinosaur = null;
			foreach (var dinosaur in _visibleDinosaurs) {
				if (!dinosaur.InBattle) {
					if (availableDinosaur == null) {
						availableDinosaur = dinosaur;
					} else {
						availableDinosaur.Attack(dinosaur);
						availableDinosaur = null;
					}
				}
			}
		}
	}
}
