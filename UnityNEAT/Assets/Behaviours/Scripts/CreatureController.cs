using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;

/// <summary>
/// 
///The script used to control the movements and evaluation of the
///evolving Creatures
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class CreatureController : UnitController {

	public float maxSpeed = 5f;
	public float Speed = 5f;
	public float TurnSpeed = 180f;
	public float stamina = 5.0f;
	public float curstamina = 5.0f;
	bool IsRunning;

	IBlackBox box;

	//scanning values
	public int rayCount = 24;//number of raycasts
	public float sightLength = 30;//length of raycast
	public float fov = 15;//degrees between each raycast
	private GameObject enemy;
	public int searchObjectCount = 3;//how many different things the creature is looking for
	float[] sensorVals;//values of all the raycasts
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);//angle to start the raycasts from
	Quaternion raySpace;//spaces between the raycasts, makes rays line up with creature

	//evals
	public int predatorHit = 0;
	private float startTime;
	private float survivedTime;
	public int foodEaten = 0;
	public int WallHits;

	//checking tags
	public string predstr = "Predator";
	public string foodstr = "Food";
	public string wallstr = "Wall";

	// Use this for initialization
	void Start () {
		raySpace = Quaternion.AngleAxis(fov, Vector3.up);
		sensorVals = new float[rayCount * searchObjectCount];
		startTime = Time.time;
	}

	//Setters
	public void setTraits(float maxSpeed, float Speed, float TurnSpeed, float stamina){
		this.maxSpeed = maxSpeed;
		this.Speed = Speed;
		this.TurnSpeed = TurnSpeed;
		this.stamina = stamina;
		this.curstamina = stamina;
	}

	public void setRays(float sightLength,  float fov){
		this.sightLength = sightLength;
		this.fov = fov;
	}



	// Update is called once per frame
	void FixedUpdate()
	{

		if (IsRunning)
		{
			int rayCounter = 0;
			// check for Predators
			//creates multiple raycasts around the creature,
			//if the raycast hits something its looking for, then add a value to the neural network inputs
			RaycastHit hit;
			Quaternion angle = transform.rotation * startingAngle;
			Vector3 rayDir = angle * Vector3.forward;
			for(int i = 0; i < rayCount; i++)
			{
				if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
				{
					GameObject collider = hit.collider.gameObject;
					if (collider.gameObject.tag == predstr) {
						//Debug.Log ("Seen the Predator");
						sensorVals [i] = 1 - hit.distance / sightLength;
					} else {
						sensorVals [i] = -1;
					}
				}
				rayDir = raySpace * rayDir;
				rayCounter++;
			}

			//check for Food
			for(int i = rayCounter + 1; i < rayCount * 2; i++)
			{
				if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
				{
					GameObject collider = hit.collider.gameObject;
					if (collider.gameObject.tag == foodstr) {
						//Debug.Log ("Seen Food");
						sensorVals [i] = 1 - hit.distance / sightLength;
					} else {
						sensorVals [i] = -1;
					}
				}
				rayDir = raySpace * rayDir;
				rayCounter++;
			}
			//check for Walls
			for(int i = rayCounter + 1; i < rayCount * 3; i++)
			{
				if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
				{
					GameObject collider = hit.collider.gameObject;
					if(collider.gameObject.tag == wallstr)
					{
						//Debug.Log ("Seen Wall");
						sensorVals [i] = 1 - hit.distance / sightLength;
					}
					else {
						sensorVals [i] = -1;
					}
				}
				rayDir = raySpace * rayDir;
			}

			///////////////////////////
			//Input Array for Black Box
			///////////////////////////

			//create array
			ISignalArray inputArr = box.InputSignalArray;

			for(int i = 0; i < rayCount * searchObjectCount; i++) {
				inputArr [i] = sensorVals [i];
			}

			box.Activate();

			ISignalArray outputArr = box.OutputSignalArray;


			//how the creatures are controlled
			float power = (float)outputArr [1] * 2 - 1;
			float steering = (float)outputArr[0] * 2 - 1;

			float forward = power * Speed * Time.deltaTime;
			float turn = power * steering * TurnSpeed * Time.deltaTime ;
			float possibleMax = maxSpeed * ((int)curstamina / stamina);//limit max speed based on stamina

			//increase stamia
			if (curstamina < stamina - curstamina / 10) {
				curstamina += curstamina / 30;
			}
			//turn creature and move forward
			transform.Rotate(new Vector3(0, turn, 0));
			transform.Translate(Vector3.forward * Mathf.Clamp(forward,0,possibleMax));

			//decrease by amount moved
			curstamina -= Mathf.Clamp (forward, 0, possibleMax) / 3;
		}
	}

	public override void Stop()
	{
		this.IsRunning = false;
	}

	public override void Activate(IBlackBox box)
	{
		this.box = box;
		this.IsRunning = true;
	}

	public override float GetFitness()
	{
		Debug.Log("Food Eaten = " + foodEaten + " || Predator hits = " + predatorHit);
		float fit = foodEaten - (float)(WallHits * 0.5) - (predatorHit);

		if (fit > 0) {
			return fit;
		}
		return 0;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == predstr) {
			predatorHit++;
			//survivedTime = Time.time - startTime;
		}
		if (collision.collider.tag == wallstr) {
			WallHits++;
		}
		if (collision.collider.tag == foodstr) {
			foodEaten++;
			Debug.Log ("Destroyed object " + foodstr);

			//Destroy(collision.collider.gameObject);

			if (curstamina < stamina - 1)
				curstamina++;
		}

	}

}
