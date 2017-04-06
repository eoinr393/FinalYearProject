using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour {

	public float slowingRad = 10.0f;
	public float circleSize = 10.0f;
	public float smallCircle = 2.0f;


	public Vector3 wander(Vector3 forward, Vector3 velocity, Vector3 steering, float maxSpeed, float mass){

		Vector3 targetPos = steering + forward * 0.5f;
		targetPos = new Vector3 (targetPos.x + Random.Range(-smallCircle, smallCircle), 0, targetPos.z + Random.Range(-smallCircle, smallCircle));
		Vector3.Normalize(targetPos);

		targetPos = targetPos * circleSize;
		steering = targetPos + forward * 0.5f;
		steering = Vector3.ClampMagnitude (steering, maxSpeed);
		steering = steering / mass;

		return Vector3.Lerp(velocity,steering - velocity, maxSpeed/2);
	}

}
