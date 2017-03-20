using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scan{

	Steering parent;

	public Scan(Steering parent){
		this.parent = parent;
	}

	public Vector3 Calculate()
	{
		Vector3 accForce = Vector3.zero;
		RaycastHit hit;
		Quaternion angle = parent.transform.rotation * parent.startingAngle;
		Vector3 rayDir = angle * Vector3.forward;

		for(int i = 0; i < parent.rayCount; i++)
		{
			Debug.DrawRay (parent.transform.position, rayDir * parent.sightLength, Color.red);

			if(Physics.Raycast(parent.transform.position, rayDir, out hit, parent.sightLength))
			{
				GameObject collider = hit.collider.gameObject;
				if(collider.gameObject.tag == "Prey")
				{
					Debug.Log ("Found Prey");
					parent.enemyFound = true;
					parent.enemy = collider.gameObject;
					return accForce;
				}
				//avoid obsticles
				if (collider.gameObject.tag == "Wall") {
					accForce += hit.normal * (parent.avoidForce / hit.distance);
				}
				if (collider.gameObject.tag == "Food") {
					accForce += hit.normal * (parent.avoidForce / hit.distance);
				}
			}
			rayDir = parent.raySpace * rayDir;
		}

		return accForce;
	}
}
