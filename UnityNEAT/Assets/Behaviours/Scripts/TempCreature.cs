using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Script to controll the behaviour for the temporary creature
/// seen in the attribute selection scene
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class TempCreature : MonoBehaviour {

	SelectionMenu refScript;

	//wander
	public float slowingRad = 10.0f;
	public float circleSize = 5.0f;
	public float smallCircle = 4.0f;
	//force
	private Vector3 force = Vector3.zero;
	private Vector3 acceleration = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private Vector3 steering = new Vector3();

	private float speed = 5.0f;
	private float turnspeed = 18.0f;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		refScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<SelectionMenu>();
		rb = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		UpdateCreature ();

		force = getForce ();
		acceleration = force / rb.mass;
		velocity += acceleration * Time.deltaTime;

		velocity = Vector3.ClampMagnitude(velocity, speed);
		if (velocity != Vector3.zero)
		{
			//print (velocity.x + "," + velocity.z);
			transform.forward = velocity;
		}

		transform.position += velocity * Time.deltaTime;

	}

	void UpdateCreature(){

		transform.position = new Vector3(transform.position.x, transform.position.y  + (SelectionMenu.size - transform.localScale.y)/2, transform.position.z);
		transform.localScale = new Vector3 (SelectionMenu.size, SelectionMenu.size, SelectionMenu.size);

		speed = SelectionMenu.speed;
	}

	Vector3 getForce(){
		force = Vector3.zero;

		force += Wander ();

		return force;
	}

	Vector3 Wander(){

		Vector3 targetPos = steering - transform.forward * 0.5f;
		targetPos = new Vector3 (targetPos.x + Random.Range(-smallCircle, smallCircle), 0, targetPos.z + Random.Range(-smallCircle, smallCircle));
		Vector3.Normalize(targetPos);

		targetPos = targetPos * circleSize;
		steering = targetPos + transform.forward * 0.5f;
		steering = Vector3.ClampMagnitude (steering, speed);
		steering = steering / rb.mass;

		return steering - velocity ;
	}
}
