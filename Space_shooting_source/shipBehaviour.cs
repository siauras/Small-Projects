using UnityEngine;
using System.Collections;

public class shipBehaviour : MonoBehaviour {
	// Only for testing purposes
	bool GodMode = false;

	//Score
	public int SCORE = 0;

	//physics
	Rigidbody2D body = new Rigidbody2D();

	// Ship movement speed
	int shipSpeed = 20;

	// Weaponry settings
	public GameObject weapon;
	float weaponry_intervalCooldoown = 0.1f;
	float weaponry_coolDown = 1f;
	float weaponry_currentCooldown;
	float weaponry_currentIntervalCooldown;
	int projectile_count;
	int projectile_limit = 5;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void FixedUpdate () {

		// Just ship x coordinates
		float x = transform.position.x;

		//Ship movement. Added drag for smoother border stopping. 
		//To prevent ship going out of borders.
		if (Input.GetKey ("a") && x > -3.8f) {
			body.drag = 5;
			body.AddForce (Vector3.left * shipSpeed);
		} else if (Input.GetKey ("d") && x < 3.8f) {
			body.drag = 5;
			body.AddForce (Vector3.right * shipSpeed);
		}else{
			body.drag = 10;
		}

		//Weaponry control

		//Check for input
		if ( Input.GetKey("space")){
			
			// Only working with existing objects
			if (weapon != null) {
				
				//If main cooldown is over
				if (weaponry_currentCooldown < Time.time) {
					
					// Check the interval cooldown
					if (weaponry_currentIntervalCooldown < Time.time) {
						
						//Spawn projectiles and count them
						GameObject projectile = (GameObject)Instantiate ( weapon, transform.position, Quaternion.identity);
						projectile_count += 1;
						weaponry_currentIntervalCooldown = Time.time + weaponry_intervalCooldoown;

						//Check whether projectiles have reached their limit
						if (projectile_count >= projectile_limit) {
							projectile_count = 0;
							weaponry_currentCooldown = Time.time + weaponry_coolDown;
						}
					}
				}
			}
		}
		
	}

	// Checking for collision.
	// Implemented godmode
	void OnCollisionEnter2D( Collision2D coll){
		if (coll.gameObject.tag == "asteroid" && GodMode == false) {
			Destroy (transform.gameObject);
		}
	}

	// Score part
	public void setScore( int score ){
		SCORE = score;
	}

	public void addScore( int score ){
		SCORE += score;
	}

}
