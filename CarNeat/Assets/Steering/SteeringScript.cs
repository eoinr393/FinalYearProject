using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringScript : MonoBehaviour {

	public GameObject goal;
	public List<GameObject> targets;
	Rigidbody rb;
	Perceptron p;
	public Vector3 acceleration;
	public float maxForce = 0.5f;
	public float maxSpeed = 4.0f;

	void Start(){
		p = new Perceptron (targets.Count);
		//rb = (Rigidbody)this.gameObject.rigidbody;
		rb = GetComponent<Rigidbody> ();
		acceleration = new Vector3(0, 0, 0);
	}

	void FixedUpdate(){
		
		rb.velocity += acceleration;
		acceleration = new Vector3(0,0,0);
		Steer ();
	}

	void ApplyForce(Vector3 force){
		acceleration += force;
	}

	void Steer(){
		
		Vector3[] forces = new Vector3[targets.Count];

		for(int i = 0; i< forces.Length; i++) {
			forces[i] = Seek (targets[i].transform.position);
		}

		Vector3 result = p.FeedForward (forces);

		ApplyForce (result);

		Vector3 desired = goal.transform.position;
		Vector3 error = desired - this.gameObject.transform.position;
		p.Train (forces, error);

	}

	Vector3 Seek(Vector3 target){
		Vector3 desired = target - this.gameObject.transform.position;
		desired = desired.normalized * maxSpeed;
		Vector3 steer = desired - rb.velocity;
		steer = Vector3.ClampMagnitude (steer, maxForce);
		return steer;
	}

}