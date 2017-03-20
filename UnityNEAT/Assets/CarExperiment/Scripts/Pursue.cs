using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue{

	Steering parent;

	public Pursue(Steering parent){
		this.parent = parent;
	}

	public Vector3 Calculate(){
		Rigidbody trb = parent.enemy.GetComponent<Rigidbody> ();
		float distance = Vector3.Distance(parent.enemy.transform.position, parent.transform.position);
		float time = distance / parent.maxSpeed;
		Vector3 futurePos = parent.enemy.transform.position + trb.velocity * time;

		return Seek(futurePos, parent.velocity);
	}

	//steer toward a point
	Vector3 Seek(Vector3 target, Vector3 velocity){

		Vector3 desired = target - parent.transform.position;
		desired.Normalize ();
		desired *= parent.maxSpeed;
		return desired - velocity;

	}
}
