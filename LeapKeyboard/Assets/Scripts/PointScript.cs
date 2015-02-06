﻿using UnityEngine;
using System.Collections;
using Leap;
using System;

public class PointScript : MonoBehaviour {
	public GameObject PointSphere;
	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();
	Frame frame;

	public GameObject SettingSwitchObject;
	public GameObject SettingObject;

	SettingScript stc;

	private Vector3 mouse_position;

	// Use this for initialization
	void Start () {
		//PointSphere.SetActive (false);
		stc = SettingObject.GetComponent<SettingScript> ();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		interactionBox = frame.InteractionBox;
		if(frame.Hands.Count == 0)
			PointMoveByMouse();
		for (int i = 0; i<frame.Hands.Count; i++) {
			if(frame.Hands[i].PalmPosition.y > 180 || stc.IsKeyBoardMoving){
				PointMoveByHand(frame.Hands[i]);
				break;
			}else{
				PointMoveByMouse();
			}
		}

	}
	void PointMoveByMouse(){
		mouse_position = Input.mousePosition;
		mouse_position.z = 10f;
		mouse_position = Camera.main.ScreenToWorldPoint (mouse_position);
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		PointSphere.transform.localPosition = new Vector3 (mouse_position.x*3-50, 0, mouse_position.z*4-25);
		SettingSwitchCheck ();
		
		stc.CheckPosition ();
	}

	void PointMoveByHand(Hand hand){
		//PointSphere.SetActive (true);
		PointSphere.transform.localPosition = getPositionForPoint (hand.PalmPosition);
		SettingSwitchCheck ();

		stc.CheckPosition ();
	}

	void SettingSwitchCheck(){

		if (Vector2DistanceToSwitch (PointSphere.transform.position, SettingSwitchObject.transform.position) && (IsCircle() || Input.GetMouseButton(0))) {
			Debug.Log ("switch");
			if(SettingObject.activeSelf)
				SettingObject.SetActive(false);
			else
				SettingObject.SetActive(true);
		}
	}

	bool IsCircle(){
		//本当はサークルがヒットしたらtrueにするつもりだったけどめんどくさいからGestureがなんかあったらで
		if (frame.Gestures ().Count > 0)
			return true;
		else
			return false;
	}

	Vector3 getPositionForPoint(Vector v){
		Vector normalizedPosition = interactionBox.NormalizePoint(v);
		normalizedPosition *= 35;
		//normalizedPosition.x += 10;
		normalizedPosition.y = 0;
		return ToVector3 (normalizedPosition);
	}
	
	Vector3 ToVector3( Vector v )
	{
		return new UnityEngine.Vector3( v.x, v.y, v.z );
	}
	bool Vector2DistanceToSwitch(Vector3 sphere, Vector3 switchbutton){
		//Debug.Log (string.Format ("Modesize:{0}:{1}", AllKeyMode.renderer.bounds.size.x, AllKeyMode.renderer.bounds.size.z));
		//Debug.Log (string.Format ("Abs:{0}:{1}",Math.Abs(sphere.x - mode.x).ToString(),Math.Abs(sphere.z - mode.z)));
		return(Math.Abs(sphere.x - switchbutton.x) < (SettingSwitchObject.renderer.bounds.size.x)/2 &&
		       Math.Abs(sphere.z - switchbutton.z) < (SettingSwitchObject.renderer.bounds.size.z)/2 );
	}
	
}
