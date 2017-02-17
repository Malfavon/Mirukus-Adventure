using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	Vector3 velocity = Vector3.zero;

	public GameObject Target; // The target in which to follow
	public Vector3 CameraOffset = new Vector3(0,10,-20); // This will allow us to offset the camera for the player's view.
	public float CameraSpeed = 10f; // How fast the camera will track the target

	private Camera _camera;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Check to see if the camera and the target exist
		if (_camera != null && Target != null) {

			Vector3 targetPos = Target.transform.position;			
			Vector3 offset = CameraOffset;
			
			float cameraAngle = _camera.transform.eulerAngles.y;
			float targetAngle = Target.transform.eulerAngles.y;

			// Prevent the camera from rotating 180 degrees when moving backwards
			if (Input.GetAxisRaw("Vertical") < 0.2f) {
				targetAngle = cameraAngle;
			}

			targetAngle = Mathf.LerpAngle(cameraAngle, targetAngle, CameraSpeed * Time.deltaTime);			
			offset = Quaternion.Euler(0,targetAngle,0) * offset;
			
			_camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPos + offset,ref velocity, CameraSpeed );
			_camera.transform.LookAt(targetPos);
		}
	}
}
