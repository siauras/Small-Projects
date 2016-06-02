using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class backgroundBehaviour : MonoBehaviour {

	//Small star
	public GameObject small_star;
	int small_star_totalCount = 10;
	int small_star_currentCount = 0;
	float small_star_spawnCooldown;
	GameObject small_star_prefab;

	//Asteroid behaviour
	public GameObject asteroid;
	int asteroid_totalCount = 10;
	public int asteroid_currentCount = 0; // We need to set this to public because we need to access it from other script!
	float asteroid_spawnCooldown;
	GameObject asteroid_prefab;

	// List implementation
	List<GameObject> small_star_list = new List<GameObject>();
	List<GameObject> stars_to_delete = new List<GameObject>(); // Because you cannot use foreach and edit the list you currently looping.

	List<GameObject> asteroid_list = new List<GameObject> ();
	List<GameObject> asteroid_to_delete = new List<GameObject> (); // For the same reason why we used it for sall stars.

	//Background constants
	float bottomEdgeY;
	float topEdgeY;
	float leftEdgeX;
	float rightEdgeX;

	//Score drawing
	//Presetting variblaes for ship game object holder and ship script holder.
	GameObject ship;
	shipBehaviour shipScript;
	int SCORE;
	public Text scoreText;

	void Start () {
		small_star_currentCount = 0; // Just in case FixedUpdate doesn't get prime value

		bottomEdgeY = -6f;
		topEdgeY = 5f;
		leftEdgeX = -6.00f;
		rightEdgeX = 6.00f;

		//Finding and setting ship variables. Getting score value from ship script and setting it to local
		//backround variable SCORE.
		ship = GameObject.Find("ship");
		shipScript = ship.GetComponent<shipBehaviour> ();
		SCORE = shipScript.SCORE;
	}

	void FixedUpdate () {
		
		//Background sliding down mechanic
		transform.position = transform.position + new Vector3 (0, -0.0001f, 0);

		//#############################################################################
		//#####							SMALL STAR PART							  #####
		//#############################################################################

		//Small star spawning mechanic
		if (small_star_currentCount < small_star_totalCount && small_star_spawnCooldown < Time.time) {

			// Generating random X coordinate for spawning small star and depth correction
			float randomX = Random.Range ( leftEdgeX, rightEdgeX ); 
			float depth = -1f;

			// Instantiating new small star prefab at random X axis value
			small_star_prefab = (GameObject)Instantiate (small_star, new Vector3(randomX,topEdgeY,depth), Quaternion.identity);
			small_star_prefab.transform.parent = transform;

			// List implementation
			small_star_list.Add (small_star_prefab);

			small_star_spawnCooldown = Time.time + 1; // Setting small star spawning cooldown
			small_star_currentCount += 1; // Adding small star count
		}

		//Small star "falling" mechanic
		if (small_star_currentCount > 0) {
			foreach (GameObject starlet in small_star_list) {
				
				// It looks like C# doesn't recognize when foreach loop works with nulls? It is not enought to just remove 
				// item from list, when working with foreach loop. Thats why we have to check for nulls I guess?
				if (starlet != null) {
					starlet.transform.position = starlet.transform.position + new Vector3 (0, -0.01f, 0);
					if (starlet.transform.position.y < bottomEdgeY) {
						Destroy (starlet, 0.1f); // Removes starlet after 1 sec. gets passed.
						if (starlet != null) {
							stars_to_delete.Add (starlet);
						}

						small_star_currentCount -= 1;
					}
				}
			}

			// Starting second loop for removing old refabs
			// The second loop is required because the way C# works I guess. Simply you cannot edit the list you currently looping
			// Thats why I made a second list. From first loop I loop through all stars and check which one is need to be deleted
			// After finding one I add it to second list. And when second loop starts it deletes first List content.
			foreach (GameObject starlet in stars_to_delete) {

				// Well, I have to check here again for nulls despite that I used function .Remove. Stangely it only occurs when I try to
				// use Debug.log()...
				if (starlet != null) {
					small_star_list.Remove (starlet);
				}
			}
				
			//#############################################################################
			//#####							ASTEROID PART							  #####
			//#############################################################################

			//Asteroid generating
			// Checking whether current count doesnt overpass total count and making sure that cooldown is respected
			if (asteroid_currentCount < asteroid_totalCount && asteroid_spawnCooldown < Time.time && asteroid != null) {

				//Random X coordinate and depth correction.
				float randomX = Random.Range ( leftEdgeX, rightEdgeX );
				float depth = -2.06f;

				//Asteroid spawning
				asteroid_prefab = (GameObject)Instantiate ( asteroid, new Vector3( randomX,topEdgeY, depth ), Quaternion.identity );

				//Set parent to background
				asteroid_prefab.transform.parent = transform;

				//Add to list
				asteroid_list.Add ( asteroid_prefab );

				//Increase current asteroid count
				asteroid_currentCount += 1;

				// Asteroid spawn cooldown
				asteroid_spawnCooldown = Time.time + Random.Range( 0.0f, 2.0f );
			}

			if (asteroid_currentCount > 0) {
				foreach (GameObject a in asteroid_list) {
					
					// Working only with existing objects!
					if (a != null) {
						
						// Checking asteroid positions. Because our prefab already has pre-codded movement.
						// Don't know whether it's better to have precodded prefab or to code everything in one.
						// But I think having cleaner codes is more appreciated so in future I'll try to precode prefabs as much as possible.
						if (a.transform.position.y < bottomEdgeY) {

							//Setting timed destruction. After .1 sec. asteroid prefab will be deleted.
							//But until then we have to clear our list!
							Destroy( a, 0.1f );

							//Clearing old list where we store asteroid for later destruction because I've encoutered that strnage glitch starts to appear
							//after asteroids gets deleted instead of multiple asteroids only one starts to reappearing...
							asteroid_to_delete.Clear();

							// If current asteroid matches our criteria we sending it to destroy. Because we don't want to spam our world void
							// with constantly falling asteroids, don't we?
							if (a != null) {
								asteroid_to_delete.Add (a);
							}

							// Decreasing current count. If this one gets wrong placed it ruins entire asteroid mechanic...
							asteroid_currentCount -= 1;
						}
					}
				}
			}

			if (asteroid_to_delete.Count > 0) {
				foreach (GameObject a in asteroid_to_delete) {
					
					// Working only with existing objects.
					if (a != null) {
						
						// Deleting element from primary list.
						asteroid_list.Remove (a);
					}
				}
			}

			//#############################################################################
			//#####							SCORE PART								  #####
			//#############################################################################
			SCORE = shipScript.SCORE;
			scoreText.text = "Score: "+SCORE;
		}
	}
}
