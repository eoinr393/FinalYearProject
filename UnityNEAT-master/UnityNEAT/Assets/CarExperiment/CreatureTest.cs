using UnityEngine;
using System.Collections;

public class CreatureTest : MonoBehaviour {

	public float torque = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float turn = Input.GetAxis ("Horizontal");

		if (Input.GetKey (KeyCode.UpArrow)) {
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddTorque (transform.up * torque * turn);

			print ("adding torque");

			foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					print ("found Leg");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, torque, ForceMode.Force);
				}
			}
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			print ("Down Key Pressed");
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddTorque (transform.up * (-1 * torque) * turn);

			foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					print ("found Leg");
					child.GetComponent<Rigidbody> ().AddTorque (0, 0, -torque, ForceMode.Force);
				}
			};
		}
	
	}
}
