using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class TestRandom : IRandom {
	private int _result = 0;

	public TestRandom(int result) {
		_result = result;
	}

	public int Range(int min, int max) {
		return _result;
	}
}

public class PersonalityTest {

	[Test]
	public void FavoriteFood() {
		var person = new GameObject ().AddComponent<Personality> ();

		person.EatFood (FoodTest.CreateFood ("Apple"));

		Assert.AreEqual ("Apple", person.FavoriteFood ());
	}

	[Test]
	public void ChooseFood_ChoosesFood() {
		Personality foodChooser = new GameObject ().AddComponent<Personality> ();

		var apple = FoodTest.CreateFood ("Apple");
		var orange = FoodTest.CreateFood ("Orange");

		var foods = new List<GameObject> { apple.gameObject, orange.gameObject };

		foodChooser.EatFood (apple);
		foodChooser.EatFood (orange);

		Assert.Contains (foodChooser.ChooseFood (foods), foods);
	}

	[Test]
	public void ChooseFood_ChoosesByWeight() {
		Personality foodChooser = new GameObject ().AddComponent<Personality> ();

		var apple = FoodTest.CreateFood ("Apple");
		var orange = FoodTest.CreateFood ("Orange");

		var foods = new List<GameObject> { apple.gameObject, orange.gameObject };

		foodChooser.EatFood (apple);
		foodChooser.EatFood (orange);
		foodChooser.EatFood (orange);

		foodChooser.SetRandom (new TestRandom (0));

		Assert.AreEqual (apple.gameObject, foodChooser.ChooseFood (foods));

		foodChooser.SetRandom (new TestRandom (3));

		Assert.AreEqual (orange.gameObject, foodChooser.ChooseFood (foods));
	}

	[Test]
	public void ChooseFood_DefaultsToFirst() {
		Personality foodChooser = new GameObject ().AddComponent<Personality> ();

		var apple = FoodTest.CreateFood ("Apple");
		var orange = FoodTest.CreateFood ("Orange");
		var pear = FoodTest.CreateFood ("Pear");

		var foods = new List<GameObject> { apple.gameObject, orange.gameObject };

		foodChooser.EatFood(pear);

		Assert.AreEqual(apple.gameObject, foodChooser.ChooseFood(foods));
	}

	[Test]
	public void ChooseFood_NoFood_ReturnsNull() {
		Personality foodChooser = new GameObject ().AddComponent<Personality> ();

		var apple = FoodTest.CreateFood ("Apple");
		foodChooser.EatFood (apple);

		var foods = new List<GameObject> { };

		Assert.Null(foodChooser.ChooseFood(foods));
	}

	[Test]
	public void SavePersonality() {
		Personality person = new GameObject ().AddComponent<Personality> ();

		person.EatFood (FoodTest.CreateFood ("Apple"));

		person.Save ();

		var newPerson = new GameObject ().AddComponent<Personality>();
		newPerson.Load ();

		Assert.AreEqual (person.foodEaten, newPerson.foodEaten);
	}

	[UnityTest]
	public IEnumerator Personality_SaveAtIntervals() {
		var person = new GameObject ().AddComponent<Personality> ();
		person.saveInterval = 0.5f;

		yield return new WaitForSeconds (0.6f);

		Assert.AreEqual (1, Personality.SavedCount ());
	}

	[UnityTest]
	public IEnumerator Personality_NoInterval_NoSave() {
		new GameObject ().AddComponent<Personality> ();

		yield return new WaitForSeconds (0.6f);

		Assert.AreEqual (0, Personality.SavedCount ());
	}

	[Test]
	public void Personality_SavedCount() {
		var person = new GameObject ().AddComponent<Personality> ();
		person.Save ();

		Assert.AreEqual (1, Personality.SavedCount ());
	}

	[Test]
	public void Personality_Reset() {
		var person = new GameObject ().AddComponent<Personality> ();
		person.Save ();

		Personality.Reset ();

		Assert.AreEqual (0, Personality.SavedCount ());
	}

	[TearDown]
	public void AfterEachTest() {
		Personality.Reset ();

		foreach (var food in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy (food);
		}
	}
}
