using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		float buttonX = Screen.width/2 - 100;
		float buttonY = Screen.height / 2 + 100;
		if (GUI.Button (new Rect (buttonX, buttonY, 200, 50), "New Evolution")) {
			SceneManager.LoadScene ("Selection scene");
		}
		if (GUI.Button (new Rect (buttonX, buttonY + 75, 200, 50), "Export a Behaviour")) {
			SceneManager.LoadScene ("ExportBehaviours");
		}

	}
}
