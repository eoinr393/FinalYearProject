using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyScript : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public GameObject manualTarget;
	//wander
	public float slowingRad = 10.0f;
	public float circleSize = 5.0f;
	public float smallCircle = 4.0f;
	//force
	private Vector3 force = Vector3.zero;
	private Vector3 acceleration = Vector3.zero;
	private Vector3 velocity = Vector3.zero;

	private Vector3 steering = new Vector3();
	private Vector3 look;
	private Rigidbody rb;
	private Vector3 desiredVelocity;
	//scan
	private float rayCount = 24;
	public float sightLength = 30;
	private GameObject enemy;
	private bool enemyFound = false;
	public float avoidForce = 5.0f;
	public float fov = 15;
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);
	Quaternion raySpace;


	// Use this for initialization
	void Start () {
		if (rb == null) {
			rb = GetComponent<Rigidbody> ();
		}
		raySpace = Quaternion.AngleAxis(fov, Vector3.up);
	}

	// Update is called once per frame
	void FixedUpdate () {

		force = getForce ();
		acceleration = force / rb.mass;
		velocity += acceleration * Time.deltaTime;

		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		if (velocity.magnitude > float.Epsilon)
		{
			transform.forward = velocity;
		}

		transform.position += velocity * Time.deltaTime;
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
				if(collider.gameObject.tag == "Food")
				{
					//Debug.Log ("Found Prey");
					enemyFound = true;
					enemy = collider.gameObject;
					return accForce;
				}
				//avoid obsticles
				if (collider.gameObject.tag == "Wall") {
					accForce += hit.normal * (avoidForce / hit.distance);
				}
				/*if (collider.gameObject.tag == "Predator") {
					accForce += hit.normal * (avoidForce / hit.distance);
				}*/
			}
			rayDir = raySpace * rayDir;
		}

		return accForce;
	}

	//steer toward a point
	Vector3 Seek(Vector3 target){

		Vector3 desired = target - transform.position;
		desired.Normalize ();
		desired *= maxSpeed;
		return desired - velocity;

	}

	//pursue prey
	Vector3 Pursuit(GameObject target){
		Rigidbody trb = target.GetComponent<Rigidbody> ();
		float distance = Vector3.Distance(target.transform.position, transform.position);
		float time = distance / maxSpeed;
		Vector3 futurePos = target.transform.position + trb.velocity * time;

		return Seek(futurePos);
	}

	//wander around scene
	Vector3 Wander(){

		Vector3 targetPos = steering - transform.forward * 0.5f;
		targetPos = new Vector3 (targetPos.x + Random.Range(-smallCircle, smallCircle), 0, targetPos.z + Random.Range(-smallCircle, smallCircle));
		Vector3.Normalize(targetPos);

		targetPos = targetPos * circleSize;
		steering = targetPos + transform.forward * 0.5f;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;

		return steering - velocity ;
	}

	Vector3 getForce(){
		force = Vector3.zero;
		if (enemyFound) {
			if (enemy != null) {
				force += Seek (enemy.transform.position);
			} else
				enemyFound = false;

		} else {
			force += Wander ();
		}

		force += Scan ();

		return force;
	}
}
