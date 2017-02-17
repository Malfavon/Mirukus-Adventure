using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {
	public float dampTime = 0.15f;
	Vector3 velocity = Vector3.zero;
	public Transform Target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(Target.position);
			Vector3 delta = Target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.23f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	}
}
	}
