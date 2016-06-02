using UnityEngine;
using System.Collections;

public class asteroidBehaviour : MonoBehaviour {

	public int asteroidSpeed = 1;

	Rigidbody2D body = new Rigidbody2D();

	void Start () {
		asteroidSpeed = 5;
		body = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		if (this != null) {
			body.AddForce ( Vector3.down*asteroidSpeed );
		}
	}
}
