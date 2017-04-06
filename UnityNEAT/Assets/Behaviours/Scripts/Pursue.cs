using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// The purse behaviour used to make a creature 
/// pursue another
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class Pursue : MonoBehaviour {

	private float maxSpeed;

	public Vector3 Seek(Vector3 target, Vector3 velocity, Vector3 position){
		
		Vector3 desired = target - position;
		desired.Normalize ();
		desired *= maxSpeed;
		return desired - velocity;
	}

	//pursue prey
	public Vector3 Pursuit(GameObject target, Vector3 velocity, Vector3 position, float maxSpeed){

		this.maxSpeed = maxSpeed;

		Rigidbody trb = target.GetComponent<Rigidbody> ();
		float distance = Vector3.Distance(target.transform.position, position);
		float time = distance / maxSpeed;
		Vector3 futurePos = target.transform.position + trb.velocity * time;

		return Seek(futurePos, velocity, position);
	}
}
