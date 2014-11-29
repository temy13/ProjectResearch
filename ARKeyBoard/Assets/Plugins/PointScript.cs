﻿using UnityEngine;
using System.Collections;
using System;
using Leap;

public class PointScript : MonoBehaviour {
	public GameObject PointSphere;
	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();

	public GameObject[] Modes;


	// Use this for initialization
	void Start () {
		PointSphere.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		interactionBox = frame.InteractionBox;
		for (int i = 0; i<frame.Hands.Count; i++) {
			if(frame.Hands[i].PalmPosition.y > 250){
				PointMove(frame.Hands[i]);
				break;
			}
		}
	}

	void PointMove(Hand hand){
		PointSphere.SetActive (true);
		PointSphere.transform.localPosition = getPositionForBox (hand.PalmPosition);
		CheckPosition ();
	}

	void CheckPosition(){
		for(int i =0; i<Modes.Length; i++){
			if(Vector2DistanceToMode(
			PointSphere.transform.localPosition,
			Modes[i].transform.localPosition)){
			
			}
			
		}
	}
	
	bool Vector2DistanceToMode(Vector3 sphere, Vector3 mode){
		return(Math.Abs(sphere.x - mode.x) < Modes[0].renderer.bounds.size.x &&
		Math.Abs(sphere.y - mode.y) < Modes[0].renderer.bounds.size.y );
	}

	Vector3 getPositionForBox(Vector v){
		Vector normalizedPosition = interactionBox.NormalizePoint(v);
		normalizedPosition *= 20;
		//normalizedPosition.x += 10;
		return ToVector3 (normalizedPosition);
	}
	
	Vector3 ToVector3( Vector v )
	{
		return new UnityEngine.Vector3( v.x, v.y, v.z );
	}


}
