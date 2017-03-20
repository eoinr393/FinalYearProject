using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	private Vector3 dist;
	// Use this for initialization
	void Start () {
		dist = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//transform.LookAt (target);
		transform.position = target.transform.position - dist;
	}
}
