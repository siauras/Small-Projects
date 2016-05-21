using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
	public float x;
	public float y;

	public bool cubeExists( float x2, float y2 ){
		if (x == x2 && y == y2) {
			return true;
		}
		//Else
		return false;
	}

	public Vector2 getPos( GameObject c ){
		if (c == this) {
			return new Vector2 (x, y);
		}

		return new Vector2(-9999, -9999);
	}
}
