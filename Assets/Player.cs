using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float hAcceleration = 60;
	float hFriction = 6;
	float dashAmount = 25;

	enum Move {
		None,
		Dash
	}

	Rigidbody2D rb;

	void Start () {
		rb = Utils.TryAddComponent<Rigidbody2D>(this);
		rb.gravityScale = 0;
	}

	void FixedUpdate () {

		// Movement
		//

		float dt = Time.fixedDeltaTime;
		float vx = rb.velocity.x;
		float vy = rb.velocity.y; 

		float vxAbs = Mathf.Abs(vx);

		float fric = hFriction * dt * vxAbs;
		float acc = hAcceleration * dt;

		if(vxAbs < fric)
			vx = 0;
		else if(vx < 0)
			vx += fric;
		else
			vx -= fric;

		if(MyInput.Button(Button.Left).held)
			vx -= acc;
		if(MyInput.Button(Button.Right).held)
			vx += acc;

		rb.velocity = new Vector2(vx, vy);
	}

	Move CheckMoves(MyInput.ButtonData b1, MyInput.ButtonData b2) {
		
		float doubleTapMaxTime = .26f;

		if(b1.down){
			// Debug.Log(b1.timeSinceDown);
		}

		if(b1.down && b1.timeSinceDown < doubleTapMaxTime && b2.timeSinceDown > doubleTapMaxTime){
			return Move.Dash;
		}

		return Move.None;
	}

	void Update () {

		// Check Moves
		//

		MyInput.ButtonData left = MyInput.Button(Button.Left);
		MyInput.ButtonData right = MyInput.Button(Button.Right);

		Move leftMove = CheckMoves(left, right);
		Move rightMove = CheckMoves(right, left);

		if(leftMove != Move.None && rightMove != Move.None){
			Debug.LogError("Trying to do two moves: " + 
				leftMove.ToString() + " / " + rightMove.ToString());
		}

		int dir = 0;
		Move move = Move.None;

		if(leftMove != Move.None){
			dir = -1;
			move = leftMove;
		} else if (rightMove != Move.None){
			dir = 1;
			move = rightMove;
		}

		if(move != Move.None){
			MyInput.DidMove();
		}

		// Do Moves
		//

		// float vx = rb.velocity.x;
		float vy = rb.velocity.y; 

		if(move != Move.None){
			// Debug.Log("MOVE: " + move.ToString());
		}

		if(move == Move.Dash){
			rb.velocity = new Vector2(dir * dashAmount, vy);
		}
	}
}
