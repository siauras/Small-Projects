using UnityEngine;
using System.Collections;

public class snakePart {
	public int count;
	public int x;
	public int y;
	public string type;
	public int oldx;
	public int oldy;

	public snakePart( int newcount, int newx, int newy, string newtype){
		count = newcount; // number in queue
		x = newx;
		y = newy;
		type = newtype;
	}

	public Vector2 getPos( int num ){
		if (num == count) {
			return new Vector2 (x, y);
		} else {
			return new Vector2 (0, 0);
		}
	}

	public bool isBody( int x2, int y2 ){
		if (x2 == x && y2 == y) {
			return true;
		}else{
			return false;	
		}
	}

}
