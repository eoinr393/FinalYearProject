using UnityEngine;
using System.Collections;

public class SteeringReynolds : MonoBehaviour {

	public float maxSpeed = 1.0f;
	public GameObject target;
	Vector3 look;
	Rigidbody rb;
	Vector3 desiredVelocity;
	public float slowingRad = 10.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		//Steer ();
		Flee ();
	}

	void Steer(){
		
		float distance = Vector3.Magnitude (target.transform.position - transform.position);
		desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;

		if (distance < slowingRad)
			desiredVelocity = desiredVelocity * (distance / slowingRad);

		Vector3 steering = desiredVelocity - rb.velocity;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;
		
		rb.velocity = Vector3.ClampMagnitude (rb.velocity + steering, maxSpeed);
		transform.LookAt (target.transform.position);
	}

	void Flee(){
		desiredVelocity = ((target.transform.position - transform.position).normalized * maxSpeed) * -1;

		Vector3 steering = desiredVelocity - rb.velocity;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;

		rb.velocity = Vector3.ClampMagnitude (rb.velocity + steering, maxSpeed);

		Quaternion rotation = Quaternion.LookRotation (transform.position - target.transform.position);
		transform.rotation = rotation;
	}
}
