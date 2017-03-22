using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class SelectionMenu : MonoBehaviour {

	public float groupWidth = 300;
	public float groupHeight = 500;


	public static  float turnSpeed = 1.0f;
	public static float size = 1.0f;
	public static float stamina = 5.0f;
	public static float speed = 5.0f;
	public static string creatureName = "";

	public static bool herb = false;
	public static bool pred = false;

	private float maxVal = 10.0f;
	private float minVal = 0.5f;

	private float screenWidth = Screen.width;
	private float screenHeight = Screen.height;


	private bool enterName = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		GetComponent<Rigidbody> ().mass = size;

		if (herb && pred )
			pred = false;
		if (pred && herb)
			herb = false;
	}

	void OnGUI(){

		var groupX = 20;
		var groupY = 20;

		GUI.BeginGroup( new Rect( groupX, groupY, groupWidth, groupHeight ) );
		GUI.Box (new Rect (0, 0, groupWidth, groupHeight), "Creature Stats");

		GUI.Label( new Rect( 10, 30, 100, 30 ), "Speed" );
		speed = GUI.HorizontalSlider( new Rect( 120, 35, 200, 30 ), speed, minVal, maxVal );
		GUI.Label( new Rect( 330, 30, 50, 30 ), "(" + speed.ToString("f2") + ")");

		GUI.Label( new Rect( 10, 70, 100, 30 ), "Size" );
		size = GUI.HorizontalSlider( new Rect( 120, 75, 200, 30 ), size, minVal,  maxVal );
		GUI.Label( new Rect( 330, 70, 50, 30 ), "(" + size.ToString("f2") + ")");

		GUI.Label( new Rect( 10, 110, 100, 30 ), "Turn Speed" );
		turnSpeed = GUI.HorizontalSlider( new Rect( 120, 115, 200, 30 ), turnSpeed, minVal,  maxVal);
		GUI.Label( new Rect( 330, 110, 50, 30 ), "(" + turnSpeed.ToString("f2") + ")");

		GUI.Label( new Rect( 10, 150, 100, 30 ), "Stamina" );
		stamina = GUI.HorizontalSlider( new Rect( 120, 155, 200, 30 ), stamina, minVal,  maxVal );
		GUI.Label( new Rect( 330, 150, 50, 30 ), "(" + stamina.ToString("f2") + ")");


		herb = GUI.Toggle (new Rect (groupWidth/3, 190, 100, 30), herb, "Herbivore");
		pred = GUI.Toggle (new Rect (groupWidth/3, 220, 100, 30), pred, "Predator");

		if (GUI.Button (new Rect (groupWidth / 2, 300, 150, 50), "Begin Evolution")) {
			//go to next scene
			//SceneManager.LoadScene ("Car scene");
			enterName = true;
			Time.timeScale = 0;
		}

		GUI.EndGroup(); 

		//CreatureName
		if (enterName) {
			GUI.BeginGroup (new Rect (Screen.width/2 -100, Screen.height/2, 200, 100));

			creatureName = GUI.TextField (new Rect (10, 10, 200, 30), creatureName, 20);

			if (GUI.Button (new Rect (100, 50, 30, 30), "Go!")) {
				if(Regex.IsMatch(creatureName, @"^[a-zA-Z]+$")){
					Time.timeScale = 1;
					SceneManager.LoadScene ("Car scene");
				}
			}

			GUI.EndGroup ();

		}
	}



}
