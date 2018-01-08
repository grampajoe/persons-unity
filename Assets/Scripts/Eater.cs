using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {

	private Personality personality;

	// Use this for initialization
	void Start () {
		personality = GetComponent<Personality> ();
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other)
    {
		Food food = other.GetComponent<Food> ();

		if (food != null) {
			personality.EatFood (food);
			Destroy (other.gameObject);
		}
    }
}
