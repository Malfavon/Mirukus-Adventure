using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	// Public variables that will show up in the Editor
	public float Acceleration = 50f;
	public float MaxSpeed = 20f;

	private Rigidbody rb;


	// Use this for initialization
	void Start () {
		rb = GetComponent <Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Get the player's input axes
		float xSpeed = Input.GetAxis("Horizontal");
		float zSpeed = Input.GetAxis("Vertical");
		// Get the movement vector
		Vector3 velocityAxis = new Vector3(xSpeed, 0, zSpeed);
		// Rotate the movement vector based on the camera
		velocityAxis = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y,Vector3.up) * velocityAxis;

		// Move the player
		rb.AddForce(velocityAxis.normalized * Acceleration);

		LimitVelocity();
	}

	/// <summary>
	/// Keeps the player's velocity limited so it will not go too fast.
	/// </summary>
	private void LimitVelocity() {
		Vector2 xzVel = new Vector2(rb.velocity.x, rb.velocity.z);
		if (xzVel.magnitude > MaxSpeed) {
			xzVel = xzVel.normalized * MaxSpeed;
			rb.velocity = new Vector3(xzVel.x, rb.velocity.y, xzVel.y);
		}
	}

}
