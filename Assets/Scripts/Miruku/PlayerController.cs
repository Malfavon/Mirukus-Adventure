using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public CharacterController MyController;
	public float GroundSpeed;
	public float AerialSpeed;
	public float GravityForce;
	public float JumpStrenght;
	public Transform CameraTransform;
	bool onGround = false;
	bool onWall = false;

	float verticalVelocity;
	Vector3 groundedVelocity;
	Vector3 characterVelocity; 
	Vector3 normal; //bounce in direction of the wall face normal


	// Update is called once per frame
	void Update () 
	{
		Vector3 velAxis = Vector3.zero;

		//Get Player's Inputs
		Vector3 input = Vector3.zero;
			input.x = Input.GetAxis ("Horizontal");
			input.z = Input.GetAxis ("Vertical");
			input = Vector3.ClampMagnitude (input, 1f);
			// Rotate the movement vector based on the camera
			Quaternion inputRotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (CameraTransform.forward, Vector3.up),Vector3.up);
			input = inputRotation * input;

			//velAxis = Quaternion.AngleAxis (Camera.main.transform.eulerAngles.y, Vector3.up) * velAxis;
			// Rotate the player's model to show direction
			if (input.magnitude > 0) 
			{
			transform.rotation = Quaternion.LookRotation (input);
			}
		if (MyController.isGrounded) 
		{
			velAxis = input;
			velAxis *= GroundSpeed;
		} 
		else 
		{
			velAxis = groundedVelocity;
			velAxis += input*AerialSpeed;
					
		}
		velAxis = Vector3.ClampMagnitude (velAxis, GroundSpeed);
		velAxis *= Time.deltaTime;

		//Set the Gravity with a Y velocity
		verticalVelocity = verticalVelocity - GravityForce*Time.deltaTime;
		if (Input.GetButtonDown ("Jump")) 
		{
			if (onWall) 
			{
				Vector3 reflection = Vector3.Reflect (characterVelocity, normal);
				Vector3 projected = Vector3.ProjectOnPlane (reflection, Vector3.up);
				groundedVelocity = projected.normalized*GroundSpeed+normal*AerialSpeed;
			}
			//add a vertical velocity to Jump
			if (onGround) 
			{
				verticalVelocity += JumpStrenght;
			}
		}

		//add Acceleration
		velAxis.y = verticalVelocity*Time.deltaTime;
		characterVelocity = velAxis / Time.deltaTime;	//characterVelocity keeps the Velocity.y of the character	
		//Inputs to move the character
		CollisionFlags flags = MyController.Move(velAxis);
		//Flags do a groundcheck for jump once a time
		if ((flags & CollisionFlags.Below) != 0) 
		{
			groundedVelocity = Vector3.ProjectOnPlane (characterVelocity, Vector3.up);		
			onGround = true;
			verticalVelocity = -3f;
			onWall = false;
		}
		else if ((flags & CollisionFlags.Sides) != 0)
		{
			onGround = true;
			onWall = true;
		}
		else 
		{
			onGround = false;
			onWall = false;
		}

	}

	//Now can Do Wall Jumps
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		normal = hit.normal;


	}
}
