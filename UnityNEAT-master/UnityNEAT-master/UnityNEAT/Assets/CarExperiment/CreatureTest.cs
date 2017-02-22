using UnityEngine;
using System.Collections;

public class CreatureTest : MonoBehaviour {

	public float torque = 10;
	private float targetAngle;
	public Vector3 Vec1;
	private Quaternion moveQ;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float turn = Input.GetAxis ("Horizontal");

		foreach(Transform child in transform) {
		
			if (child.tag == "Body") {
				
				ConfigurableJoint joint = child.GetComponent<ConfigurableJoint> ();

				if(child.GetComponent<Rigidbody>().IsSleeping())
					child.GetComponent<Rigidbody> ().WakeUp ();

				if (Input.anyKey) {

					float h = Input.GetAxis ("Horizontal");
					float w = Input.GetAxis ("Vertical");

					Vec1 [0] = Vec1 [0] + h;
					Vec1 [2] = Vec1 [1] + w;
				}

				moveQ = Quaternion.Euler (Vec1);

				joint.targetRotation = moveQ;

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

