using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


	public float Acceleration = 50f;
	public float MaxSpeed = 20f;
	public float JumpStrength = 500f;

	Rigidbody rb;

	bool _onGround = false;

	float xSpeed;
	float zSpeed;
	Vector3 velocityAxis;

	bool isJumping = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		// Get the player's input axes
		xSpeed = Input.GetAxis("Horizontal");
		zSpeed = Input.GetAxis("Vertical");

		// Get the movement vector
		velocityAxis = new Vector3(xSpeed, 0, zSpeed);

		// Check the player's input
		if (Input.GetButtonDown("Jump")){
			//Jump();
			isJumping = true;
		}

		LimitVelocity();
	}



	void FixedUpdate () {
		if (isJumping) {
			if (_onGround) {
				rb.AddForce (new Vector3 (0, JumpStrength, 0), ForceMode.Impulse);
				}
			isJumping = false;
		}

		// Rotate the movement vector based on the camera
		velocityAxis = Quaternion.AngleAxis (Camera.main.transform.eulerAngles.y, Vector3.up) * velocityAxis;

		// Rotate the player's model to show direction
		if (velocityAxis.magnitude > 0) {
			transform.rotation = Quaternion.LookRotation (velocityAxis);
		}

		// Move the player
		rb.AddForce (velocityAxis.normalized * Acceleration);

		/*if (xSpeed == 0 && zSpeed == 0) {
			rb.velocity = new Vector3 (0, rb.velocity.y, 0);
		}*/

	}

	//Put a limit to the Velocity
	private void LimitVelocity() {					
		Vector2 xzVel = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z);
		if (xzVel.magnitude > MaxSpeed) {
			xzVel = xzVel.normalized * MaxSpeed;
			GetComponent<Rigidbody>().velocity = new Vector3(xzVel.x, GetComponent<Rigidbody>().velocity.y, xzVel.y);
		}
	}


	void OnCollisionStay (){

		CheckIfGrounded ();
	}

	void OnCollisionExit (){
		_onGround = false;
	}

	//Check the player collisions
	void CheckIfGrounded()
	{
		RaycastHit[] hits;

		//We raycast down 1 pixel from this position to check for a collider
		Vector3 positionToCheck = transform.position;
		hits = Physics.RaycastAll (positionToCheck, new Vector3 (0, -1, 0), 100.0f);

		//if a collider was hit, we are grounded
		if (hits.Length > 0) {
			_onGround = true;
		}
	}


	// Applies force to the player's rigidbody to make him jump.
	/*private void Jump(){
		if (_onGround){
			
			GetComponent<Rigidbody>().AddForce(new Vector3(0,JumpStrength,0));
		}
	}*/

}
