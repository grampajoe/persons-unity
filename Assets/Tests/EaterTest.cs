using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EaterTest {

	[UnityTest]
	public IEnumerator OnTriggerEnter_EatsFood() {
		var eater = new GameObject ().AddComponent<Eater> ();
		eater.gameObject.AddComponent<Personality> ();

		var food = Resources.Load ("Prefabs/Apple") as GameObject;
		var foodObject = Object.Instantiate (food);

		// Allow the eater to initialize
		yield return null;

		eater.SendMessage ("OnTriggerEnter", foodObject.GetComponent<Collider> ());

		// Advance a frame to allow destroys to complete
		yield return null;

		// The food should be destroyed
		Assert.IsFalse (foodObject);
	}

	[UnityTest]
	public IEnumerator OnTriggerEnter_NotFood() {
		var eater = new GameObject ().AddComponent<Eater> ();
		eater.gameObject.AddComponent<Personality> ();

		var notFood = new GameObject ().AddComponent<SphereCollider> ();

		yield return null;

		eater.SendMessage ("OnTriggerEnter", notFood);

		// The thing shouldn't be destroyed
		Assert.IsTrue (notFood.gameObject);
	}

	[TearDown]
	public void AfterEachTest() {
		foreach (var food in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy (food);
		}
	}
}
