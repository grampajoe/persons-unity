using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMaker : MonoBehaviour {

	public float interval = 10f;
	public Vector2 minPos = new Vector2 (-1, -1);
	public Vector2 maxPos = new Vector2 (1, 1);

	private float _timeSinceLast = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_timeSinceLast >= interval) {
			if (Personality.SavedCount () > 0) {
				var prefab = Resources.Load ("Prefabs/Character");

				var pos = new Vector3 (
					          Random.Range (minPos.x, maxPos.x),
					          Random.Range (minPos.y, maxPos.y),
					          0
				          );

				var character = Instantiate (prefab, pos, Quaternion.identity) as GameObject;
				character.GetComponent<Personality> ().Load ();

				_timeSinceLast = 0f;
			}
		}

		_timeSinceLast += Time.deltaTime;
	}
}
