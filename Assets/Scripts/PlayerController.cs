using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
        {
            // Move up
            this.transform.position = new Vector2(
                this.transform.position.x,
                this.transform.position.y + (speed * Time.deltaTime)
            );
        }

        if (Input.GetKey(KeyCode.A))
        {
            // Move left
            this.transform.position = new Vector2(
                this.transform.position.x - (speed * Time.deltaTime),
                this.transform.position.y
            );
        }

        if (Input.GetKey(KeyCode.S))
        {
            // Move down
            this.transform.position = new Vector2(
                this.transform.position.x,
                this.transform.position.y - (speed * Time.deltaTime)
            );
        }

        if (Input.GetKey(KeyCode.D))
        {
            // Move right
            this.transform.position = new Vector2(
                this.transform.position.x + (speed * Time.deltaTime),
                this.transform.position.y
            );
        }
	}
}
