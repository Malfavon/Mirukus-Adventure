using UnityEngine;
using System.Collections;

public class PlayerAnt : MonoBehaviour {

	public enum stateMachine{					//Lista de Estados
		Idle,
		Run,
		Attack,
		Jump,
		Hurt,
		Dead
	}
	public stateMachine CurrentState;			//Guarda el estado actual


	private float h;
	private float v;

	private Animator anim;
	private Rigidbody MyBody;
	public bool grounded;

	public float fuerza;
	public float velocidad;
	private bool jumping = false;
	private bool isRunning = false;

	void Start () {
		CurrentState = stateMachine.Idle;		//Se inicia con el Idle (Default)

		anim = GetComponent<Animator> ();
		MyBody = GetComponent<Rigidbody> ();
	}
	


	void Update () {

		switch (CurrentState){					//En el switch se hacen todos los casos de la FSM
		case stateMachine.Idle:
			Idle ();
			break;
		case stateMachine.Run:
			Run ();
			break;
		case stateMachine.Jump:
			Jump ();
			break;
		case stateMachine.Attack:
			Attack ();
			break;
		case stateMachine.Hurt:
			GetHurt ();
			break;
		case stateMachine.Dead:
			Death ();
			break;


		}
	}

	void FixedUpdate () {
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		if (isRunning){
			MyBody.velocity = new Vector3 (0, 0, velocidad * Time.deltaTime);
		}

		if (jumping == true) {
			MyBody.AddForce (transform.up * fuerza * Time.deltaTime);
			anim.Play ("Jump");
		}
	}

	void Idle (){
		//anim.SetBool ("Jump", false);
		jumping = false;

		if (v != 0 || h != 0) {
			CurrentState = stateMachine.Run;
		}

		if (Input.GetButtonDown ("Jump")){
			CurrentState = stateMachine.Jump;
		}

		if (Input.GetButtonDown ("Fire1")){
			CurrentState = stateMachine.Attack;
		}
	}

	void Run (){
		if (grounded) {
			anim.SetBool ("IsRunning", true);
		}
		isRunning = true;
		if (v == 0 && h == 0) {
				isRunning = false;
			anim.SetBool ("IsRunning", false);
			CurrentState = stateMachine.Idle;
		}

		if (Input.GetButtonDown ("Jump")){
			CurrentState = stateMachine.Jump;
		}

		if (Input.GetButtonDown ("Fire1")){
			CurrentState = stateMachine.Attack;
		}


	}

	void Jump (){
		if (grounded) {
			Debug.Log (":V");
			jumping = true;

			CurrentState = stateMachine.Idle;
		}
	}


	void Attack (){
		anim.SetTrigger ("Attack");
		CurrentState = stateMachine.Idle;
	}

	void GetHurt (){

	}

	void Death (){

	}




	void OnCollisionStay (){

		CheckIfGrounded ();
	}

	void OnCollisionExit (){
		grounded = false;
	}

	private void CheckIfGrounded()
	{
		RaycastHit[] hits;

		//We raycast down 1 pixel from this position to check for a collider
		Vector3 positionToCheck = transform.position;
		hits = Physics.RaycastAll (positionToCheck, new Vector3 (0, -1, 0), 100.0f);

		//if a collider was hit, we are grounded
		if (hits.Length > 0) {
			grounded = true;
		}
	}
}
