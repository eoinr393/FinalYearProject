using UnityEngine;
using System.Collections;

public class SphereMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.UpArrow))
			this.gameObject.transform.Translate(Vector3.forward);	
		if (Input.GetKeyDown (KeyCode.DownArrow))
			this.gameObject.transform.Translate (Vector3.back);	
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			this.gameObject.transform.Translate (Vector3.left);	
		if (Input.GetKeyDown (KeyCode.RightArrow))
			this.gameObject.transform.Translate (Vector3.right);	
	}
}
