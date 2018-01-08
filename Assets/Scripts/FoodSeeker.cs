using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public interface IFoodGetter {
	List<GameObject> GetNearbyFood();
}

public interface ISeeker {
	void MoveTowards(GameObject food);
}

public interface IFoodChooser {
	GameObject ChooseFood (List<GameObject> food);
}

public class FoodSeeker : MonoBehaviour, IFoodGetter, ISeeker, IFoodChooser {

	public float detectRadius = 3f;
	public float thrust = 5f;
	public float rotateSpeed = 1f;
	public float speed = 5f;
	public float acceleration = 5f;

	public FoodSeekerController controller;
	public Personality personality;

	private Rigidbody rb;

	void OnDrawGizmos() {
		// Draw a sphere for the detection radius
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, detectRadius);
	}

	void OnEnable() {
		controller = new FoodSeekerController ();
		controller.SetFoodGetter (this);
		controller.SetSeeker (this);
		controller.SetFoodChooser (this);

		personality = GetComponent<Personality> ();
	}

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		controller.MoveTowardsNearestFood ();
	}

	/// <summary>
	/// Gets the nearby food.
	/// </summary>
	/// <returns>The nearby food.</returns>
	public List<GameObject> GetNearbyFood() {
		Collider[] foodColliders = Physics.OverlapSphere (transform.position, detectRadius);

		return (from collider in foodColliders
				where collider.gameObject.CompareTag("Food")
		        select collider.gameObject).ToList ();
	}

	public void MoveTowards(GameObject food) {
		if (rb) {
			var direction = food.transform.position - transform.position;
			var rotation = Quaternion.LookRotation (direction);
			transform.rotation = Quaternion.Lerp (transform.rotation, rotation, rotateSpeed * Time.fixedDeltaTime);

			rb.velocity = Vector3.Lerp (rb.velocity, direction.normalized * speed, acceleration * Time.fixedDeltaTime);
		}
	}

	public GameObject ChooseFood(List<GameObject> foods) {
		return personality.ChooseFood (foods);
	}
}

public class FoodSeekerController {
	private IFoodGetter _foodGetter;
	private ISeeker _seeker;
	private IFoodChooser _foodChooser;

	private GameObject _currentTarget;

	public void SetFoodGetter(IFoodGetter foodGetter) {
		_foodGetter = foodGetter;
	}

	public void SetSeeker(ISeeker seeker) {
		_seeker = seeker;
	}

	public void SetFoodChooser(IFoodChooser foodChooser) {
		_foodChooser = foodChooser;
	}

	public void MoveTowardsNearestFood() {
		if (!_currentTarget) {
			var foods = _foodGetter.GetNearbyFood ();

			if (foods.Count > 0) {
				_currentTarget = _foodChooser.ChooseFood (foods);
			}
		}

		if (_currentTarget) {
			_seeker.MoveTowards (_currentTarget);
		}
	}
}