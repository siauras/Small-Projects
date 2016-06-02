using UnityEngine;
using System.Collections;

public class boltBehaviour : MonoBehaviour {

	int speed = 50;

	Rigidbody2D body = new Rigidbody2D();

	//For score tracking
	GameObject ship;
	shipBehaviour shipScript;

	//Background script
	GameObject bg;
	backgroundBehaviour bgScript;

	// Use this for initialization
	void Start () {
		speed = 10;
		body = GetComponent<Rigidbody2D> ();

		//Rotate sprite around Z axis ( roll ), so the bolt point upward!
		transform.Rotate( 0,0,90f );

		//Ship control
		ship = GameObject.Find ("ship");
		shipScript = ship.GetComponent<shipBehaviour>();

		//Background control
		bg = GameObject.Find("Background");
		bgScript = bg.GetComponent<backgroundBehaviour> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.gameObject != null) {
			body.AddForce ( Vector2.up*speed );
		}

		if (transform.position.y > 5) {
			Destroy (transform.gameObject);
		}
	}

	void OnCollisionEnter2D( Collision2D col){
		if (col.gameObject.tag == "asteroid") {
			Destroy (col.transform.gameObject);
			Destroy (transform.gameObject);

			//Adding score!
			shipScript.addScore ( 10 );

			//Adjusting current asteroid count. Because if we won't change it the asteroid script which is responsible for 
			//Spawning asteroids will stuck. It needs to know when asteroids gets removed.
			//Substracting because asteroid gets removed!
			bgScript.asteroid_currentCount -= 1;
		}
	}
}
