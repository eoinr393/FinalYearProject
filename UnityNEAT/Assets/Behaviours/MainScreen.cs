using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 
/// Script to control gui for Main Screen
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class MainScreen : MonoBehaviour {

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
