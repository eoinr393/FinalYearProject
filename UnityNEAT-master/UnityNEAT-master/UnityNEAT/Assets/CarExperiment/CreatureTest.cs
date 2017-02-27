using UnityEngine;
using System.Collections;

public class CreatureTest : MonoBehaviour {

	public float torque = 10;
	private float targetAngle;

	public float frequency = 20f;
	public float amplitude = 0.5f;
	public float period = 10.0f;
	public float angle = 5.0f;
	float t;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		t = t + Time.deltaTime;
		float phase = Mathf.Sin (t / period);


		GameObject body = null;
		foreach(Transform child in transform) {
			if (child.tag == "Body") {
				Debug.Log ("Body Set");
				body = child.gameObject;
			}
		}
			
		foreach(HingeJoint comp in body.GetComponents<HingeJoint>()){
			Debug.Log ("Rotating Leg");
			Rigidbody leg = comp.connectedBody;
			float rotamt = angle * phase;
			//leg.transform.localRotation = Quaternion.Euler((new Vector3 (0,0,rotamt)));
			//comp.transform.Rotate((new Vector3 (0,0,rotamt)));
			Debug.Log("rotmat :" + rotamt);
			leg.AddTorque((new Vector3 (0,0,rotamt)));
		}
		/*if (Input.GetKey (KeyCode.UpArrow)) {
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddTorque (transform.up * torque * turn);

			//print ("adding torque");

			foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					//print ("found Leg");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, torque, ForceMode.Force);
				}

				else if (child.tag == "Arm") {
					print ("found Arm");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, torque, ForceMode.Force);
				}
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			print ("Down Key Pressed");
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddTorque (transform.up * (-1 * torque) * turn);

			foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					//print ("found Leg");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, -torque, ForceMode.Force);
				}
				else if (child.tag == "Arm") {
					print ("found Arm");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, -torque, ForceMode.Force);
				}
			};
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {

			foreach (Transform child in transform) {
				if (child.tag == "Arm") {
					print ("found Arm");
					child.GetComponent<Rigidbody> ().AddTorque (-torque,0 , 0, ForceMode.Force);
				}
			}
		}
		if (Input.GetKey (KeyCode.RightArrow)) {

			foreach (Transform child in transform) {
				if (child.tag == "Arm") {
					print ("found Arm");
					child.GetComponent<Rigidbody> ().AddTorque (torque,0 , 0, ForceMode.Force);
				}
			}
		}*/
	
	}

}

