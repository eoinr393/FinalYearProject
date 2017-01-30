using UnityEngine;
using System.Collections;

public class SteeringReynolds : MonoBehaviour {

	public float maxSpeed = 5.0f;
	public GameObject manualTarget;

	public float slowingRad = 10.0f;
	public float circleSize = 5.0f;

	private Vector3 steering = new Vector3();
	private Vector3 look;
	private Rigidbody rb;
	private Vector3 desiredVelocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		//Steer (manualTarget.transform.position);
		//Flee (manualTarget.transform.position);
		Wander();
		//Evade(manualTarget);
		//Pursuit (manualTarget);
	}

	void Steer(Vector3 target){
		
		float distance = Vector3.Magnitude (target - transform.position);
		desiredVelocity = (target - transform.position).normalized * maxSpeed;

		if (distance < slowingRad)
			desiredVelocity = desiredVelocity * (distance / slowingRad);

		steering = desiredVelocity - rb.velocity;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;
		
		rb.velocity = Vector3.ClampMagnitude (rb.velocity + steering, maxSpeed);
		transform.LookAt (target);
	}

	void Flee(Vector3 target){
		desiredVelocity = ((target - transform.position).normalized * maxSpeed) * -1;

		steering = desiredVelocity - rb.velocity;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;

		rb.velocity = Vector3.ClampMagnitude (rb.velocity + steering, maxSpeed);

		Quaternion rotation = Quaternion.LookRotation (transform.position - target);
		transform.rotation = rotation;
	}

	void Wander(){

		float smallCircle = 4.0f;

		Vector3 targetPos = steering - transform.forward * 0.5f;
		targetPos = new Vector3 (targetPos.x + Random.Range(-smallCircle, smallCircle), 0, targetPos.z + Random.Range(-smallCircle, smallCircle));
		Vector3.Normalize(targetPos);

		targetPos = targetPos * circleSize;
		steering = targetPos + transform.forward * 0.5f;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / rb.mass;

		rb.velocity = Vector3.ClampMagnitude( rb.velocity + steering.normalized, maxSpeed);
		Quaternion rotation = Quaternion.LookRotation (rb.velocity);
		transform.rotation = rotation;
	}

	void Pursuit(GameObject target){
		Vector3 distance = target.transform.position - transform.position;
		Rigidbody trb = target.GetComponent<Rigidbody> ();
		float targetPos = distance.magnitude / trb.velocity.magnitude;
		Vector3 futurePos = target.transform.position + trb.velocity * targetPos;

		Steer(futurePos);
	}

	void Evade(GameObject target){
		Vector3 distance = target.transform.position - transform.position;
		Rigidbody trb = target.GetComponent<Rigidbody> ();
		float targetPos = distance.magnitude / trb.velocity.magnitude;
		Vector3 futurePos = target.transform.position + trb.velocity * targetPos;

		Flee(futurePos);
	}
}
