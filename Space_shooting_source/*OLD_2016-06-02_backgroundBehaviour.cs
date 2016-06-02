using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class backgroundBehaviour : MonoBehaviour {

	//Small star
	public GameObject small_star;
	int small_star_totalCount = 10;
	int small_star_currentCount = 0;
	float small_star_spawnCooldown;
	GameObject small_star_prefab;

	// List implementation
	List<GameObject> small_star_list = new List<GameObject>();
	List<GameObject> stars_to_delete = new List<GameObject>(); // Because you cannot use foreach and edit the list you currently looping.

	//Background constants
	float bottomEdgeY;
	float topEdgeY;
	float leftEdgeX;
	float rightEdgeX;

	void Start () {
		small_star_currentCount = 0; // Just in case FixedUpdate doesn't get prime value

		bottomEdgeY = -6f;
		topEdgeY = 5f;
		leftEdgeX = -6.00f;
		rightEdgeX = 6.00f;
	}

	void FixedUpdate () {
		
		//Background sliding down mechanic
		transform.position = transform.position + new Vector3 (0, -0.0001f, 0);

		//Small star spawning mechanic
		if (small_star_currentCount < small_star_totalCount && small_star_spawnCooldown < Time.time) {

			// Generating random X coordinate for spawning small star
			float randomX = Random.Range ( leftEdgeX, rightEdgeX ); 

			// Instantiating new small star prefab at random X axis value
			small_star_prefab = (GameObject)Instantiate (small_star, new Vector3(randomX,topEdgeY,-1f), Quaternion.identity);
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
		}
	}
}
