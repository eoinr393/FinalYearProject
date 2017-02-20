using UnityEngine;
using System.Collections;

public class CreatureTest : MonoBehaviour {

	public float torque = 10;
	private float targetAngle;
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			if (child.tag == "Body") {
				targetAngle = 0;
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float turn = Input.GetAxis ("Horizontal");

		targetAngle += turn * Time.deltaTime * torque;
		Quaternion targetRotation = Quaternion.AngleAxis( targetAngle, Vector3.up );

		if(Input.GetKey(KeyCode.RightArrow)){
			foreach(Transform child in transform) {
				if (child.tag == "Body") {
					print ("target angle = " + targetAngle);
					child.GetComponent<ConfigurableJoint> ().targetRotation = targetRotation;
				}
			}
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
