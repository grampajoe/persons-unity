using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CharacterMakerTest {
	[UnityTest]
	public IEnumerator CharacterMaker_MakesCharacters() {
		var maker = new GameObject ().AddComponent<CharacterMaker> ();
		maker.interval = 0.5f;

		var person = new GameObject ().AddComponent<Personality> ();
		person.EatFood (FoodTest.CreateFood ("Apple"));
		person.Save ();

		yield return new WaitForSeconds(0.6f);

		var character = GameObject.FindGameObjectWithTag ("Character");

		Assert.IsNotNull (character);
		Assert.IsNotNull (character.GetComponent<Personality> ());
		Assert.AreEqual ("Apple", character.GetComponent<Personality> ().FavoriteFood ());
	}

	[UnityTest]
	public IEnumerator CharacterMaker_NoCharacters() {
		var maker = new GameObject ().AddComponent<CharacterMaker> ();
		maker.interval = 0.5f;

		yield return new WaitForSeconds (0.6f);

		var characters = GameObject.FindGameObjectsWithTag ("Character");

		Assert.IsEmpty (characters);
	}

	[TearDown]
	public void AfterEachTest() {
		foreach (var obj in GameObject.FindGameObjectsWithTag("Character")) {
			GameObject.Destroy (obj);
		}

		foreach (var food in GameObject.FindGameObjectsWithTag("Food")) {
			GameObject.Destroy (food);
		}

		Personality.Reset ();
	}
}
