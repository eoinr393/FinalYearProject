using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Script to make a camera follow an attaches object
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class CameraFollow : MonoBehaviour {

	public GameObject target;
	private Vector3 dist;
	// Use this for initialization
	void Start () {
		dist = target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position - dist;
	}
}
