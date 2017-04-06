using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using SharpNeat.Phenomes;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.Neat;

/// <summary>
/// 
/// Script to controll an evolved behaviour
/// that is only using the network, not evolving it
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class ControllerSkeleton : MonoBehaviour {

	public TextAsset ta;
	private XmlDocument network = new XmlDocument ();

	private float maxSpeed = 5f;
	private float Speed = 5f;
	private float TurnSpeed = 180f;
	private float stamina = 5.0f;
	private float curstamina = 5.0f;

	IBlackBox box;

	//scanning values
	private int rayCount = 24;//number of rays
	private float sightLength = 30;//length of ray
	private float fov = 15;//degrees between each raycast
	private const int searchObjectCount = 3;//how many different things the creature is looking for
	private float[] sensorVals;//values of all the raycasts
	Quaternion startingAngle = Quaternion.AngleAxis(-65, Vector3.up);//angle to start the raycasts from
	Quaternion raySpace;//spaces between the raycasts

	//checking tags
	private string predstr = "Predator";
	private string foodstr = "Food";
	private string wallstr = "Wall";

	private bool Running = false;

	// Use this for initialization
	void Start () {
		raySpace = Quaternion.AngleAxis(fov, Vector3.up);
		sensorVals = new float[rayCount * searchObjectCount];
		CreateNetwork ();
	}

	//Setters
	public void setTraits(float maxSpeed, float Speed, float TurnSpeed, float stamina){
		this.maxSpeed = maxSpeed;
		this.Speed = Speed;
		this.TurnSpeed = TurnSpeed;
		this.stamina = stamina;
		this.curstamina = stamina;
	}
	//set the creatures lines of sight
	public void setRays(float sightLength,  float fov){
		this.sightLength = sightLength;
		this.fov = fov;
	}

	//set what the creature views as food, walls, predators etc
	public void setTags(string pred, string food, string wall){
		this.predstr = pred;
		this.wallstr = wall;
		this.foodstr = food;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Running) {
			int rayCounter = 0;
			// check for Predators
			RaycastHit hit;
			Quaternion angle = transform.rotation * startingAngle;
			Vector3 rayDir = angle * Vector3.forward;
			for (int i = 0; i < rayCount; i++) {
				if (Physics.Raycast (transform.position, rayDir, out hit, sightLength)) {
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
			for (int i = rayCounter + 1; i < rayCount * 2; i++) {
				if (Physics.Raycast (transform.position, rayDir, out hit, sightLength)) {
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
			for (int i = rayCounter + 1; i < rayCount * 3; i++) {
				if (Physics.Raycast (transform.position, rayDir, out hit, sightLength)) {
					GameObject collider = hit.collider.gameObject;
					if (collider.gameObject.tag == wallstr) {
						//Debug.Log ("Seen Wall");
						sensorVals [i] = 1 - hit.distance / sightLength;
					} else {
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

			for (int i = 0; i < rayCount * searchObjectCount; i++) {
				inputArr [i] = sensorVals [i];
			}

			box.Activate ();

			ISignalArray outputArr = box.OutputSignalArray;

			// float steer = (float)outputArr[0] * 2 - 1;
			//float gas = (float)outputArr[1] * 2 - 1;

			float steer = (float)outputArr [0] * 2 - 1;
			float gas = (float)outputArr [1] * 2 - 1;

			float moveDist = gas * Speed * Time.deltaTime;
			float turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

			float possibleMax = maxSpeed * ((int)curstamina / stamina);//limit max speed based on stamina

			//increase stamia
			if (curstamina < stamina - curstamina / 10) {
				curstamina += curstamina / 10;
			}

			transform.Rotate (new Vector3 (0, turnAngle, 0));
			transform.Translate (Vector3.forward * Mathf.Clamp (moveDist, 0, possibleMax));


			//decrease by amount moved

			curstamina -= Mathf.Clamp (moveDist, 0, possibleMax) / 3;
		}
	}

	private void Activate(IBlackBox box)
	{
		this.box = box;
		this.Running = true;
		Debug.Log ("Activated");
	}

	public void CreateNetwork(){
		if (ta != null) {

			network.LoadXml (ta.text);

			NeatGenome genome = null;

			//allways true because we are always using acyclic activiation
			NeatGenomeParameters ngp = new NeatGenomeParameters();
			ngp.FeedforwardOnly = true;

			//temp, need to make more malleable
			int inputs = 72;
			int outputs = 2;

			NeatGenomeFactory ngf = new NeatGenomeFactory (inputs, outputs, ngp);
			// Try to load the genome from the XML document.
			using (XmlReader xr = new XmlNodeReader(network))
				genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)ngf)[0];

			// Get a genome decoder that can convert genomes to phenomes.
			NeatGenomeDecoder genomeDecoder = new NeatGenomeDecoder(NetworkActivationScheme.CreateAcyclicScheme());

			// Decode the genome into a phenome (neural network).
			IBlackBox phenome = genomeDecoder.Decode(genome);
					
			//send the network as a black box to the 
			Activate(phenome);
		}
	}

}
