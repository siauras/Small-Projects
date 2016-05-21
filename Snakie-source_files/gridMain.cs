using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
public class gridMain : MonoBehaviour {

	public GameObject gridCube;
	public Camera mainCamera;
	public int gridSize;
	public Light snakeLight;
	public Light foodLight;
	public Text score;
	int Score;

	GameObject[,] cubeHolder;
	GameObject[,] snakeParts;
	Vector2[] snakeParts2;

	List<snakePart> sparts = new List<snakePart>();

	private int snakex;
	private int snakey;

	private int old_snakex;
	private int old_snakey;

	private float timeHolder = 0 ;

	private int direction = 0; // 0 stationary, 1 - up, 2 - down, 3 - right, 4 - left;

	//FOOD
	private Vector2 food;

	// GAME STATE
	string State = "Running";

	// Use this for initialization
	void Start () {
		Score = 0;
		if (gridSize < 1) {
			gridSize = 10;
		}

		cubeHolder = new GameObject[gridSize, gridSize];
		snakeParts = new GameObject[gridSize, gridSize]; // Don't know why I'm setting this one to grid size but currently I dont have enought experience to tell what I actually need in C#
		snakeParts2 = new Vector2[gridSize*gridSize];

		// We need to run this only once, not every frame.
		for (int y = 0; y < gridSize; y++) {
			for (int x = 0; x < gridSize; x++) {
				cubeHolder[x,y] = (GameObject)Instantiate (gridCube, new Vector3 (x, y), Quaternion.identity);
				Cube c = cubeHolder [x, y].GetComponent<Cube> ();
				c.x = x;
				c.y = y;
			}
		}

		snakex = gridSize / 2;
		snakey = gridSize / 2;
		old_snakex = snakex;
		old_snakey = snakey;

		snakeParts2 [0] = new Vector2 (snakex, snakey);
		snakeParts2 [1] = new Vector2 (snakex - 1, snakey);
		//LIST:
		sparts.Add( new snakePart( sparts.Count+1, snakex, snakey, "head" ) );
		sparts.Add( new snakePart( sparts.Count+1, snakex-1, snakey, "body" ) );
		sparts.Add( new snakePart( sparts.Count+1, snakex-2, snakey, "body" ) );

		///////////

		mainCamera.transform.position = new Vector3 ( (gridSize)/2, (gridSize)/2, -1000 );
		mainCamera.orthographicSize = gridSize / 2;

		// FOOOOOOOOOOOOOD
		food = new Vector2( Random.Range(0, gridSize), Random.Range(0,gridSize)); 
	}


	// Update is called once per frame
	void FixedUpdate () {
		if ( State != "Running"){
			return;
		}
		float time_	= (float)Time.time;
		cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.red;
		if (timeHolder < time_) {
			timeHolder = time_ + 0.1f;

			//MOVEMENT TO UPWARD
			if (direction == 1) {
				if (snakey + 1 == gridSize) {
					cubeHolder [snakex, gridSize-1].GetComponent<Renderer> ().material.color = Color.white;
					old_snakey = snakey;
					old_snakex = snakex;
					snakey = 0;
				} else {
					cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakey = snakey;
					old_snakex = snakex;
					snakey++;
				}
			}

			//MOVEMENT TO DOWNWARD
			if (direction == 2) {
				if (snakey - 1 == -1) {
					cubeHolder [snakex, 0].GetComponent<Renderer> ().material.color = Color.white;
					old_snakey = snakey;
					old_snakex = snakex;
					snakey = gridSize-1;
				} else {
					cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakey = snakey;
					old_snakex = snakex;
					snakey--;
				}
			}

			//MOVEMENT TO RIGHT
			if (direction == 3) {
				if (snakex + 1 == gridSize) {
					cubeHolder [gridSize-1, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakex = snakex;
					old_snakey = snakey;
					snakex = 0;
				} else {
					cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakex = snakex;
					old_snakey = snakey;
					snakex++;
				}
			}

			//MOVEMENT TO LEFT
			if (direction == 4) {
				if (snakex - 1 == -1) {
					cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakex = snakex;
					old_snakey = snakey;
					snakex = gridSize-1;
				} else {
					cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.white;
					old_snakex = snakex;
					old_snakey = snakey;
					snakex--;
				}
			}
			// SNKADE COLLISION WITH BODY
			if (cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color == Color.green) {
				State = "DEAD";
			}

			// SNAKE BODY PART
			int snakePartCount = snakeParts2.Length;

			if (direction != 0) {
				/*Debug.Log (snakePartCount);
				for (int i = 1; i < snakePartCount; i++) {
					Vector2 oldPos = new Vector2 (snakeParts2[i].x, snakeParts2 [i].y);

					if (i == 1) {
						cubeHolder [old_snakex, old_snakey].GetComponent<Renderer> ().material.color = Color.green;
						snakeParts2 [i] = new Vector2 ( old_snakex, old_snakey );
					}



					if (snakePartCount == i) { // Cuz they count the zero one
						cubeHolder [(int)oldPos.x, (int)oldPos.y].GetComponent<Renderer> ().material.color = Color.white;
					}
				}*/
				//Debug.Log (sparts.Count);
				int partCount = sparts.Count;
				foreach (snakePart part in sparts) {
					Vector2 oldPos = new Vector2 ( part.x, part.y );

					if (part.count == 2) {
						cubeHolder [old_snakex, old_snakey].GetComponent<Renderer> ().material.color = Color.green;
						part.oldx = part.x;
						part.oldy = part.y;
						part.x = old_snakex;
						part.y = old_snakey;
						//Debug.Log ("Setting first part of body coordinates to " + part.x + ", " + part.y);
					} else if (part.count > 2) {
						cubeHolder [part.x, part.y].GetComponent<Renderer> ().material.color = Color.green;
						part.oldx = part.x;
						part.oldy = part.y;
						part.x = sparts [part.count - 2].oldx;
						part.y = sparts [part.count - 2].oldy;
						//Debug.Log ("Setting " + part.count+ " part of body coordinates to " + part.x + ", " + part.y);
					}
						
					if (partCount == part.count) {
						cubeHolder [(int)oldPos.x, (int)oldPos.y].GetComponent<Renderer> ().material.color = Color.white;
					}
				}
			}
				
		}

		if (Input.GetKey ("w") && direction != 2 ) {
			direction = 1;
		}else if (Input.GetKey ("s") && direction != 1 ) {
			direction = 2;
		}else if (Input.GetKey ("d") && direction != 4 ) {
			direction = 3;
		}else if (Input.GetKey ("a") && direction != 3 ) {
			direction = 4;
		}
		//Debug.Log (direction);
		//Debug.Log ( "Snakex: " + snakex + " Snakey: "+snakey+" Old snakex: " + old_snakex + " Old snakey: "+old_snakey );
	
		// FOOD
			//RENDERING
		cubeHolder [(int)food.x, (int)food.y].GetComponent<Renderer> ().material.color = Color.yellow;
			//EATING
		if (snakex == food.x && snakey == food.y) {
			Score++;
			score.text = ("Score: " + Score);
			sparts.Add( new snakePart( sparts.Count+1, sparts[sparts.Count-1].x, sparts[sparts.Count-1].y, "body" ) );
			cubeHolder [snakex, snakey].GetComponent<Renderer> ().material.color = Color.green; // For glitch that appears after picking food
			food = new Vector2( Random.Range(0, gridSize), Random.Range(0,gridSize)); 
		}

		// Lighting
		snakeLight.transform.position = new Vector3( snakex, snakey, -1.1f );
		foodLight.transform.position = new Vector3 (food.x, food.y, -1.1f);
	}
}
