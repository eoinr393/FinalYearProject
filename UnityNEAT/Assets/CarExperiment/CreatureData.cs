using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[Serializable]
class CreatureData{
	public float maxSpeed = 5f;
	public float Speed = 5f;
	public float TurnSpeed = 180f;
	public float stamina = 5.0f;
	public float curstamina = 5.0f;
	public float sightLength = 30;//length of ray
	public float fov = 15;//degrees between each raycast
	public string predstr = "Predator";
	public string foodstr = "Food";
	public string wallstr = "Wall";
	public float mass = 1.0f;
}
