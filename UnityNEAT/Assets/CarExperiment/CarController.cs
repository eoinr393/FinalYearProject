﻿using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;

public class CarController : UnitController {

    public float Speed = 5f;
    public float TurnSpeed = 180f;
    public int Lap = 1;
    public int CurrentPiece, LastPiece;
    bool MovingForward = true;
    bool IsRunning;
    public float SensorRange = 10;
    int WallHits; 
    IBlackBox box;

	//scanning values
	private int rayCount = 12;
	public float sightLength = 100;
	public float fov = 30;
	private GameObject enemy;
	float[] sensorVals;
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);
	Quaternion raySpace;

	//evals
	private int predatorHit = 0;
	private float startTime;
	private float survivedTime;
	private int foodEaten = 0;

	// Use this for initialization
	void Start () {
		raySpace = Quaternion.AngleAxis(fov, Vector3.up);
		sensorVals = new float[rayCount * 2];
		startTime = Time.time;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
		

        if (IsRunning)
        {
			int rayCounter = 0;
            // check for predators
            RaycastHit hit;
			Quaternion angle = transform.rotation * startingAngle;
			Vector3 rayDir = angle * Vector3.forward;
			for(int i = 0; i < rayCount; i++)
			{
				Debug.DrawRay (transform.position, rayDir * sightLength, Color.blue);

				if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
				{
					GameObject collider = hit.collider.gameObject;
					if(collider.name == "Predator")
					{
						Debug.Log ("Seen the Predator");
						sensorVals [i] = 1 - hit.distance / sightLength;
					}
				}
				rayDir = raySpace * rayDir;
				rayCounter++;
			}

			//check for food
			for(int i = rayCounter + 1; i < rayCount * 2; i++)
			{
				Debug.DrawRay (transform.position, rayDir * sightLength, Color.blue);

				if(Physics.Raycast(transform.position, rayDir, out hit, sightLength))
				{
					GameObject collider = hit.collider.gameObject;
					if(collider.name == "Food")
					{
						Debug.Log ("Seen Food");
						sensorVals [i] = 1 - hit.distance / sightLength;
					}
				}
				rayDir = raySpace * rayDir;
			}


            ISignalArray inputArr = box.InputSignalArray;
			for(int i = 0; i < rayCount * 2; i++) {
				inputArr [i] = sensorVals [i];
			}
        
            box.Activate();

            ISignalArray outputArr = box.OutputSignalArray;

            var steer = (float)outputArr[0] * 2 - 1;
            var gas = (float)outputArr[1] * 2 - 1;

            var moveDist = gas * Speed * Time.deltaTime;
            var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

            transform.Rotate(new Vector3(0, turnAngle, 0));
            transform.Translate(Vector3.forward * moveDist);
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

    public void NewLap()
    {        
        if (LastPiece > 2 && MovingForward)
        {
            Lap++;            
        }
    }

    public override float GetFitness()
    {
        /*if (Lap == 1 && CurrentPiece == 0)
        {
            return 0;
        }
        int piece = CurrentPiece;
        if (CurrentPiece == 0)
        {
            piece = 17;
        }
        float fit = Lap * piece - WallHits * 0.2f;
      //  print(string.Format("Piece: {0}, Lap: {1}, Fitness: {2}", piece, Lap, fit));
        if (fit > 0)
        {
            return fit;
        }
        return 0;*/
		float fit = foodEaten + (float)(survivedTime * 0.2) - (float)(WallHits * 0.2) - (predatorHit);

		if (fit > 0) {
			return fit;
		}

		return 0;

    }

    void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.tag.Equals("Road"))
        {
            RoadPiece rp = collision.collider.GetComponent<RoadPiece>();
          //  print(collision.collider.tag + " " + rp.PieceNumber);
            
            if ((rp.PieceNumber != LastPiece) && (rp.PieceNumber == CurrentPiece + 1 || (MovingForward && rp.PieceNumber == 0)))
            {
                LastPiece = CurrentPiece;
                CurrentPiece = rp.PieceNumber;
                MovingForward = true;                
            }
            else
            {
                MovingForward = false;
            }
            if (rp.PieceNumber == 0)
            {
                CurrentPiece = 0;
            }
        }
        else if (collision.collider.tag.Equals("Wall"))
        {
            WallHits++;
        }*/

		if (collision.collider.tag == "Predator") {
			predatorHit++;
			survivedTime = Time.time - startTime;
		}
		if (collision.collider.tag == "Wall") {
			WallHits++;
		}
		if (collision.collider.tag == "Food") {
			foodEaten++;
			Destroy(collision.collider.gameObject);
		}
			
    }



    //void OnGUI()
    //{
    //    GUI.Button(new Rect(10, 200, 100, 100), "Forward: " + MovingForward + "\nPiece: " + CurrentPiece + "\nLast: " + LastPiece + "\nLap: " + Lap);
    //}
    
}
