namespace FirebaseTest
{
	using Firebase;
	using Firebase.Database;
	using Firebase.Unity.Editor;
	using UnityEngine;
	using UnityEngine.UI;


	public class FirebaseClickHandler : MonoBehaviour
	{
		private DatabaseReference _counterRef;

		[SerializeField] private Text _counterText;
		private int _count = -1;

		private void Awake()  
		{
			Debug.Log("firebase initialized");

			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ar-pet.firebaseio.com/");

			_counterRef = FirebaseDatabase.DefaultInstance.GetReference("counter");

			_counterRef.ValueChanged += OnCountUpdated;
		}

		private void Update()
		{
			_counterText.text = _count.ToString("N0");
		}

		private void OnCountUpdated(object sender, ValueChangedEventArgs e)  
		{
			// throw new System.NotImplementedException();

			if (e.DatabaseError != null)  
			{
				Debug.LogError(e.DatabaseError.Message);
				return;
			}

			if (e.Snapshot == null || e.Snapshot.Value == null) _count = 0;  
			else _count = int.Parse(e.Snapshot.Value.ToString());  
		}

		public void IncrementClickCounter()  
		{
			Debug.Log("add score");
			_counterRef.SetValueAsync(_count + 1);
		}

		private void OnDestroy()  
		{
			_counterRef.ValueChanged -= OnCountUpdated;
		}

	}
}
