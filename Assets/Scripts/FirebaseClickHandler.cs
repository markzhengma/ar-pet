// namespace FirebaseTest
// {
	using Firebase;
	using Firebase.Database;
	using Firebase.Unity.Editor;
	using UnityEngine;
	using UnityEngine.UI;


	public class FirebaseClickHandler : MonoBehaviour
	{
		private DatabaseReference _counterRef;

		[SerializeField] private Text _counterText;
		private int _count = 0;

		public GameObject character;

		float tempTime;

		private void Awake()  
		{
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ar-pet.firebaseio.com/");

			_counterRef = FirebaseDatabase.DefaultInstance.GetReference("counter");

			_counterRef.ValueChanged += OnCountUpdated;
		}

		private void Update()
		{
			_counterText.text = _count.ToString("N0");
			tempTime += Time.deltaTime;
        	if (tempTime > 3.0)
			{
				tempTime = 0;
				if (_count > 0 && character.active){
					DecreaseCounter();
				}
			}
		}

		private void OnCountUpdated(object sender, ValueChangedEventArgs e)  
		{
			if (e.DatabaseError != null)  
			{
				Debug.LogError(e.DatabaseError.Message);
				return;
			}

			if (e.Snapshot == null || e.Snapshot.Value == null) 
				_count = 0;  
			else 
				_count = int.Parse(e.Snapshot.Value.ToString());  

			// throw new System.NotImplementedException();
		}

		public void IncrementClickCounter()  
		{
			_counterRef.RunTransaction(data => {
				data.Value = _count + 1;
				return TransactionResult.Success(data);
			}).ContinueWith(task => {
				if (task.Exception != null) 
					Debug.Log(task.Exception.ToString());
			});
		}

		private void DecreaseCounter()  
		{
			_counterRef.RunTransaction(data => {
				data.Value = _count - 1;
				return TransactionResult.Success(data);
			}).ContinueWith(task => {
				if (task.Exception != null) 
					Debug.Log(task.Exception.ToString());
			});
		}

		private void OnDestroy()  
		{
			_counterRef.ValueChanged -= OnCountUpdated;
		}

	}
// }
