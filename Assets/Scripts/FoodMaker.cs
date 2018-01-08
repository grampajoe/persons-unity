using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMaker : MonoBehaviour {

	public List<GameObject> foods = new List<GameObject> ();
	public float interval = 1.0f;
	public int foodAtOnce = 1;
	public Vector2 minPos = new Vector2(-1, -1);
	public Vector2 maxPos = new Vector2(1, 1);

	private float timeSinceFood = 0f;

	void Update () {
		if (timeSinceFood >= interval) {
			for (var i = 0; i < foodAtOnce; i++) {
				var food = foods [Random.Range (0, foods.Count)];
				var pos = new Vector3 (
					         Random.Range (minPos.x, maxPos.x),
					         Random.Range (minPos.y, maxPos.y),
					         0f
				         );
				Instantiate (food, pos, Quaternion.identity);
			}

			timeSinceFood = 0f;
		}

		timeSinceFood += Time.deltaTime;
	}
}
