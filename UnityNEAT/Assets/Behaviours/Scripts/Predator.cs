using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Component driven script for controlling creatures
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class Predator : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public float curspeed = 0;

	private Vector3 force = Vector3.zero;
	private Vector3 acceleration = Vector3.zero;
	private Vector3 velocity = Vector3.zero;

	private Vector3 steering = new Vector3();
	private Vector3 look;
	private Vector3 desiredVelocity;
	//scan
	private float rayCount = 24;
	public float sightLength = 45;
	private GameObject enemy;
	private bool enemyFound = false;
	public float avoidForce = 5.0f;
	public float fov = 5;
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);
	Quaternion raySpace;

	//behaviours 
	Wander wander;
	Pursue pursue;

	// Use this for initialization
	void Start () {
		this.wander = new Wander ();
		this.pursue = new Pursue ();

		raySpace = Quaternion.AngleAxis(fov, Vector3.up);
	}

	// Update is called once per frame
	void FixedUpdate () {

		force = getForce ();
		acceleration = force / 1;
		velocity += acceleration * Time.deltaTime;

		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		if (velocity.magnitude > float.Epsilon)
		{
			transform.forward = velocity;
		}

		transform.position += velocity * Time.deltaTime;

	}
	//get the right force to add to the creature
	Vector3 getForce(){
		force = Vector3.zero;
		if (enemyFound) {
			if (enemy != null) {
				force += pursue.Pursuit (enemy, velocity, transform.position, maxSpeed);
			} else
				enemyFound = false;

		} else {
			force += wander.wander(transform.forward, velocity, steering, maxSpeed, 1);
		}

		force += Scan ();

		return force;
	}

	//search for enemy avoid walls
	Vector3 Scan()
	{
		Vector3 accForce = Vector3.zero;
		RaycastHit hit;
		Quaternion angle = transform.rotation * startingAngle;
		Vector3 rayDir = angle * Vector3.forward;

		for(int i = 0; i < rayCount; i++)
		{
			Debug.DrawRay (transform.position, rayDir * sightLength, Color.red);

			if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
			{
				GameObject collider = hit.collider.gameObject;
				if(collider.gameObject.tag == "Prey")
				{
					enemyFound = true;
					enemy = collider.gameObject;
					return accForce;
				}
				//avoid obsticles
				if (collider.gameObject.tag == "Wall") {
					accForce += hit.normal * (avoidForce / hit.distance);
				}
				if (collider.gameObject.tag == "Food") {
					accForce += hit.normal * (avoidForce / hit.distance);
				}
			}
			rayDir = raySpace * rayDir;
		}

		return accForce;
	}


	//if they somehow glitch outside of the boundaries
	void checkDist(){
		if (Vector3.Distance (this.gameObject.transform.position, new Vector3 (0, 0, 0)) > 100)
			Destroy (this.gameObject);
	}

}
