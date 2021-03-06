using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRandom {
	int Range (int min, int max);
}

public class DefaultRandom : IRandom {
	public int Range(int min, int max) {
		return UnityEngine.Random.Range (min, max);
	}
}

[Serializable]
public class PersonalityData {
	public SerializableDictionary foodEaten = new SerializableDictionary ();
}

// Personality holds a model of a character's personality.
//
// The model consists of a set of data, e.g. foods eaten,
// methods that model actions performed by the character
// which *modify* that data, and methods to make decisions
// based on the data.
public class Personality : MonoBehaviour {

	public SerializableDictionary foodEaten {
		get {
			return _data.foodEaten;
		}

		set {
			_data.foodEaten = value;
		}
	}

	public float saveInterval = 0f;

	private PersonalityData _data = new PersonalityData();
	private IRandom _random;
	private float _timeSinceLastSave = 0f;

	// Temporary hack to locally save characters
	private static List<byte[]> _savedPersonalities = new List<byte[]> ();

	public void SetRandom(IRandom random) {
		_random = random;
	}

	public IRandom GetRandom() {
		if (_random == null) {
			_random = new DefaultRandom ();
		}

		return _random;
	}

	public void Save() {
		var stream = new MemoryStream ();
		var bf = new BinaryFormatter ();

		bf.Serialize (stream, _data);
		_savedPersonalities.Add(stream.ToArray());
		stream.Close ();
	}

	public void Load() {
		var data = _savedPersonalities[GetRandom().Range(0, _savedPersonalities.Count)];
		var stream = new MemoryStream (data);
		var bf = new BinaryFormatter ();

		_data = bf.Deserialize (stream) as PersonalityData;
		stream.Close ();
	}

	public static void Reset() {
		_savedPersonalities.Clear ();
	}

	/// <summary>
	/// Returns the number of saved personalities.
	/// </summary>
	/// <returns>The count.</returns>
	public static int SavedCount() {
		return _savedPersonalities.Count;
	}

	public void EatFood(Food food) {
		if (!foodEaten.ContainsKey (food.type)) {
			foodEaten.Add (food.type, 0);
		}

		foodEaten [food.type]++;
	}

	// Returns the name of the person's favorite food
	public string FavoriteFood() {
		var best = new KeyValuePair<string, int> ("None", 0);

		foreach (KeyValuePair<string, int> entry in foodEaten) {
			if (entry.Value > best.Value) {
				best = entry;
			}
		}

		return best.Key;
	}

	// Choose food takes a list of food objects and returns a choice
	// based on the character's food preferences.
	public GameObject ChooseFood(List<GameObject> foods) {
		if (foods.Count == 0) {
			return null;
		}

		var values = new SortedList<int, string> ();
		int sum = 0;

		foreach (KeyValuePair<string, int> entry in foodEaten) {
			sum += entry.Value;

			values.Add (sum, entry.Key);
		}

		int result = GetRandom ().Range (0, sum);

		foreach (var entry in values) {
			if (result <= entry.Key) {
				foreach (var food in foods) {
					if (food.GetComponent<Food> ().type == entry.Value) {
						return food;
					}
				}
			}
		}

		return foods[0];
	}

	public void Update() {
		// Save at intervals if the interval is not zero.
		if (saveInterval > 0f) {
			if (_timeSinceLastSave > saveInterval) {
				Save ();

				_timeSinceLastSave = 0f;
			}

			_timeSinceLastSave += Time.deltaTime;
		}
	}
}
