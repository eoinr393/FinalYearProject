using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;

public class CarController : UnitController {

    public float maxSpeed = 5f;
	public float Speed = 5f;
    public float TurnSpeed = 180f;
	public float stamina = 5.0f;
	public float curstamina = 5.0f;
    bool IsRunning;
     
    IBlackBox box;

	//scanning values
	public int rayCount = 24;//number of rays
	public float sightLength = 30;//length of ray
	public float fov = 15;//degrees between each raycast
	private GameObject enemy;
	public int searchObjectCount = 3;//how many different things the creature is looking for
	float[] sensorVals;//values of all the raycasts
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);//angle to start the raycasts from
	Quaternion raySpace;//spaces between the raycasts

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
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (IsRunning)
        {
			int rayCounter = 0;
            // check for Predators
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

            float steer = (float)outputArr[0] * 2 - 1;
            float gas = (float)outputArr[1] * 2 - 1;

            float moveDist = gas * Speed * Time.deltaTime;
            float turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

			float possibleMax = maxSpeed * ((int)curstamina / stamina);//limit max speed based on stamina

			//increase stamia
			if (curstamina < stamina - curstamina / 10) {
				curstamina += curstamina / 10;
			}

            transform.Rotate(new Vector3(0, turnAngle, 0));
			transform.Translate(Vector3.forward * Mathf.Clamp(moveDist,0,possibleMax));

			//decrease by amount moved

			curstamina -= Mathf.Clamp (moveDist, 0, possibleMax) / 5;
        }
    }

	private float AngleBetween(Vector3 vec1, Vector3 vec2){

		float angle = Vector3.Angle(vec1,vec2);
		Vector3 cross = Vector3.Cross (vec1, vec2);
		return (cross.y < 0) ? -angle : angle;

	}

	private ISignalArray insertNeuronValues(ISignalArray inputArr, SortedList<float,float> list){
		int inputCount = 0;

		int tempInt = -999;
		while (list.Count < 3) {
			list.Add (-tempInt, -999);
			tempInt++;
		}
			
		for (int i = list.Count - 1; i > list.Count - 4; i--) {
			inputArr [inputCount] = list.Keys [i];
			inputArr [inputCount = + 1] = list.Values [i];
			inputCount += 2;
		}
		return inputArr;
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
		//float fit = foodEaten + (float)(survivedTime * 0.2) - (float)(WallHits * 0.2) - (predatorHit);
		print("Food Eaten = " + foodEaten + " || Predator hits = " + predatorHit);
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
			survivedTime = Time.time - startTime;
		}
		if (collision.collider.tag == wallstr) {
			WallHits++;
		}
		if (collision.collider.tag == foodstr) {
			foodEaten++;
			Destroy(collision.collider.gameObject);

			if (curstamina < stamina - 1)
				curstamina++;
		}
			
    }
    
}
