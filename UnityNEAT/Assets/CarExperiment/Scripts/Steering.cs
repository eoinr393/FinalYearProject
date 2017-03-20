using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {

	private Vector3 force = Vector3.zero;
	private Vector3 acceleration = Vector3.zero;
	public Vector3 velocity = Vector3.zero;
	public Vector3 steering = Vector3.zero;

	public float maxSpeed = 5f;
	public float slowingRad = 10.0f;
	public float circleSize = 5.0f;
	public float smallCircle = 4.0f;

	//scan
	public float rayCount = 24;
	public Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);
	public Quaternion raySpace;
	public float sightLength = 75;
	public float avoidForce = 5.0f;
	public float fov = 5;
	public bool enemyFound = false;
	public GameObject enemy;
	public Rigidbody rb;

	//steering
	Pursue pursue;
	Scan scan;
	Wander wander;

	public Steering(Rigidbody rb){
		this.rb = rb;
	}

	public Steering(Rigidbody rb, Quaternion startingAngle, float sightLength, float fov){
		this.startingAngle = startingAngle;
		this.sightLength = sightLength;
		this.fov = fov;
		this.rb = rb;


	}
	// Use this for initialization
	void Start () {
		pursue = new Pursue (this);
		scan = new Scan (this);
		wander = new Wander (this);

		rb = GetComponent<Rigidbody> ();
	}

	public void setScangles(Quaternion startingAngle, float fov, float sightLength, float rayCount){
		this.startingAngle = startingAngle;
		this.sightLength = sightLength;
		this.fov = fov;
		this.rayCount = rayCount;
		raySpace = Quaternion.AngleAxis(fov, Vector3.up);

		Debug.Log ("scAngles Set");
	}

	public Vector3 getForce(){
		force = Vector3.zero;
		if (enemyFound) {
			if (enemy != null) {
				force += pursue.Calculate();
			} else
				enemyFound = false;

		} else {
			force += wander.Calculate();
		}

		force += scan.Calculate();

		return force;
	}
}
