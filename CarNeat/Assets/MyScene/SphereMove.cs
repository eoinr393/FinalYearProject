using UnityEngine;
using System.Collections;

public class SphereMove : MonoBehaviour {
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate (Vector3.forward);	
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate (Vector3.back);	
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Translate (Vector3.left);	
		if (Input.GetKey (KeyCode.RightArrow))
			transform.Translate (Vector3.right);	
	}
}
