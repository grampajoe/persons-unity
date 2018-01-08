using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

public class FoodSeekerTest {

	[Test]
	public void GetNearbyFood() {
		var person = new GameObject ().AddComponent<Personality> ();
		var seeker = person.gameObject.AddComponent<FoodSeeker> ();
		var pos = seeker.gameObject.transform;

		var foodPrefab = Resources.Load ("Prefabs/Apple");

		var nearFood = GameObject.Instantiate (foodPrefab, pos) as GameObject;
		GameObject.Instantiate (foodPrefab, new Vector3(10f, 10f, 10f), Quaternion.identity);

		var foods = seeker.GetNearbyFood ();

		Assert.AreEqual(new List<GameObject> { nearFood }, foods);
	}

	[Test]
	public void ChooseFood() {
		var person = new GameObject ().AddComponent<Personality> ();
		var seeker = person.gameObject.AddComponent<FoodSeeker> ();

		person.EatFood (FoodTest.CreateFood ("Apple"));

		var foods = new List<GameObject> { FoodTest.CreateFood ("Apple").gameObject, FoodTest.CreateFood("Pear").gameObject };
		var chosen = seeker.ChooseFood (foods);

		Assert.AreEqual ("Apple", chosen.GetComponent<Food> ().type);
	}

	[TearDown]
	public void AfterEachTest() {
		foreach (var food in GameObject.FindGameObjectsWithTag ("Food")) {
			GameObject.Destroy (food);
		}
	}
}

public class FoodSeekerControllerTest {
	[Test]
	public void MoveTowardsNearestFood() {
		var food = FoodTest.CreateFood ("Apple").gameObject;
		var foodGetter = GetMockFoodGetter (new List<GameObject> { food });
		var foodChooser = GetMockFoodChooser (food);

		var seeker = GetMockSeeker ();

		var controller = new FoodSeekerController ();
		controller.SetFoodGetter (foodGetter);
		controller.SetSeeker (seeker);
		controller.SetFoodChooser (foodChooser);

		controller.MoveTowardsNearestFood ();

		seeker.Received ().MoveTowards (food);
	}

	[Test]
	public void MoveTowardsNearestFood_NoFood() {
		var foodGetter = GetMockFoodGetter (new List<GameObject> { });
		var seeker = GetMockSeeker ();

		var controller = new FoodSeekerController ();
		controller.SetFoodGetter (foodGetter);
		controller.SetSeeker (seeker);

		controller.MoveTowardsNearestFood ();

		seeker.DidNotReceive ().MoveTowards (Arg.Any<GameObject> ());
	}

	[Test]
	public void MoveTowardsNearestFood_Sticky() {
		var food = FoodTest.CreateFood ("Apple").gameObject;
		var foodGetter = GetMockFoodGetter (new List<GameObject> { food });
		var seeker = GetMockSeeker ();
		var foodChooser = GetMockFoodChooser (food);

		var controller = new FoodSeekerController ();
		controller.SetFoodGetter (foodGetter);
		controller.SetSeeker (seeker);
		controller.SetFoodChooser (foodChooser);

		controller.MoveTowardsNearestFood ();

		seeker.Received ().MoveTowards (food);

		var newFood = FoodTest.CreateFood ("Orange").gameObject;
		foodGetter.GetNearbyFood ().Returns (new List<GameObject> { newFood });

		seeker.ClearReceivedCalls ();
		controller.MoveTowardsNearestFood ();

		seeker.Received ().MoveTowards (food);
	}

	[Test]
	public void MoveTowardsNearestFood_Picky() {
		var apple = FoodTest.CreateFood ("Apple").gameObject;
		var pear = FoodTest.CreateFood ("Pear").gameObject;

		var foodGetter = GetMockFoodGetter (new List<GameObject> { apple, pear });
		var seeker = GetMockSeeker ();
		var foodChooser = GetMockFoodChooser (pear);

		var controller = new FoodSeekerController ();
		controller.SetFoodGetter (foodGetter);
		controller.SetSeeker (seeker);
		controller.SetFoodChooser (foodChooser);

		controller.MoveTowardsNearestFood ();

		seeker.Received ().MoveTowards (pear);
	}

	private IFoodGetter GetMockFoodGetter(List<GameObject> foods) {
		var foodGetter = Substitute.For<IFoodGetter> ();
		foodGetter.GetNearbyFood ().Returns (foods);

		return foodGetter;
	}

	private ISeeker GetMockSeeker() {
		return Substitute.For<ISeeker> ();
	}

	private IFoodChooser GetMockFoodChooser(GameObject food) {
		var foodChooser = Substitute.For<IFoodChooser> ();
		foodChooser.ChooseFood (Arg.Any<List<GameObject>>()).Returns (food);

		return foodChooser;
	}

	[TearDown]
	public void AfterEachTest() {
		foreach (var food in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy (food);
		}
	}
}