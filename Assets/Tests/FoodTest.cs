using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FoodTest {

	// Creates a test food object.
	public static Food CreateFood(string type) {
		var food = new GameObject ().AddComponent<Food> ();
		food.type = type;
		food.gameObject.tag = "Food";

		return food;
	}

	[Test]
	public void TestToString() {
		var food = CreateFood ("Test");

		Assert.AreEqual ("Test", food.ToString ());
	}

	[TearDown]
	public void AfterEachTest() {
		foreach (var food in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy (food);
		}
	}
}
