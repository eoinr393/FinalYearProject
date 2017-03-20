using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander{

	Steering parent;

	public Wander(Steering parent){
		this.parent = parent;		
	}

	//wander around scene
	public Vector3 Calculate(){

		Vector3 targetPos = parent.steering - parent.transform.forward * 0.5f;
		targetPos = new Vector3 (targetPos.x + Random.Range(-parent.smallCircle, parent.smallCircle), 0, targetPos.z + Random.Range(-parent.smallCircle, parent.smallCircle));
		Vector3.Normalize(targetPos);

		targetPos = targetPos * parent.circleSize;
		parent.steering = targetPos + parent.transform.forward * 0.5f;
		parent.steering = Vector3.ClampMagnitude (parent.steering, parent.maxSpeed);
		parent.steering = parent.steering / parent.rb.mass;

		return parent.steering - parent.velocity ;
	}
}
