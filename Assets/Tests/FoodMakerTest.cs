using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class FoodMakerTest {

	private List<GameObject> foodMakers = new List<GameObject>();
	
	[UnityTest]
	public IEnumerator MakesFood() {
		var foodMaker = new GameObject ().AddComponent<FoodMaker> ();
		foodMakers.Add (foodMaker.gameObject);
		var food = Resources.Load ("Prefabs/Apple") as GameObject;
		foodMaker.foods.Add(food);
		foodMaker.interval = 0.01f;

		yield return new WaitForSeconds(0.1f);

		var foods = GameObject.FindGameObjectsWithTag ("Food");

		Assert.NotZero (foods.Length);
	}

	[UnityTest]
	public IEnumerator MakesFood_MoreThanOne() {
		var foodMaker = new GameObject ().AddComponent<FoodMaker> ();
		foodMakers.Add (foodMaker.gameObject);

		var food = Resources.Load ("Prefabs/Apple") as GameObject;
		foodMaker.foods.Add (food);

		foodMaker.interval = 0.1f;
		foodMaker.foodAtOnce = 5;

		yield return new WaitForSeconds (0.2f);

		var foods = GameObject.FindGameObjectsWithTag ("Food");

		Assert.AreEqual (foodMaker.foodAtOnce, foods.Length);
	}

	[TearDown]
	public void AfterEveryTest() {
		foreach (var gameObject in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy(gameObject);
		}

		foreach (var gameObject in foodMakers) {
			GameObject.Destroy (gameObject);
		}
	}
}
