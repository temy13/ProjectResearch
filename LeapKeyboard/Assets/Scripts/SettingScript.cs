using UnityEngine;
using System.Collections;
using Leap;
using System;

public class SettingScript : MonoBehaviour {

	public GameObject pointObject;
	public GameObject KeyBoardMoveSwitch;
	public bool IsKeyBoardMoving = false;
	public GameObject AllKeyMode;
	public GameObject KeyBoard;
	HandBehaviorScript hbs;

	SceneScript sc;
	public GameObject[] Modes;

	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();
	Frame frame;

	Vector3 BeforePointPosition;
	
	// Use this for initialization
	void Start () {
		hbs = KeyBoard.GetComponent<HandBehaviorScript> ();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		interactionBox = frame.InteractionBox;
		CheckSwitchPosition ();
		if (IsKeyBoardMoving)
			KeyBoardMove();

		if (Input.GetKeyDown (KeyCode.Return)) {
			IsKeyBoardMoving = false;
		}
	}

	public void CheckPosition(){
		if (!IsCircle ())
			return;
		
		for(int i =0; i<Modes.Length; i++){
			if(Vector2DistanceToMode(
				pointObject.transform.position,
				Modes[i].transform.position)){
				Modes[i].renderer.material.color = Color.green;
				ChangeScene(i);
			}else if(Modes[i].renderer.material.color.Equals(Color.green))
			{
				Modes[i].renderer.material.color = Color.white;
			}
		}

	}
	
	bool Vector2DistanceToMode(Vector3 sphere, Vector3 mode){
		//Debug.Log (string.Format ("Modesize:{0}:{1}", Modes [0].renderer.bounds.size.x, Modes [0].renderer.bounds.size.z));
		//Debug.Log (string.Format ("Abs:{0}:{1}",Math.Abs(sphere.x - mode.x).ToString(),Math.Abs(sphere.z - mode.z)));
		return(Math.Abs(sphere.x - mode.x) < (Modes[0].renderer.bounds.size.x)/2 &&
		       Math.Abs(sphere.z - mode.z) < (Modes[0].renderer.bounds.size.z)/2 );
	}
	
	void ChangeScene(int i){
		
		Modes[i].renderer.material.color = Color.white;
		sc.ChangeScene (i + 1);
		
	}

	void CheckSwitchPosition()
	{
		if (pointObject.activeSelf && Vector3.Distance (pointObject.transform.position, AllKeyMode.transform.position) < 2.0)
			ChangeAllKeyMode ();
		if (KeyBoardMoveSwitch.activeSelf && Vector3.Distance (pointObject.transform.position, KeyBoardMoveSwitch.transform.position) < 2.0) 
						ChangeKeyBoardMoveMode ();
	}

	void ChangeAllKeyMode(){
		if (!IsCircle ())
						return;
		if (hbs.allmode) {
						hbs.allmode = false;
						AllKeyMode.renderer.material.color = Color.white;
				} else {
						hbs.allmode = true;
						AllKeyMode.renderer.material.color = Color.blue;
				}
	}
	void ChangeKeyBoardMoveMode(){
		IsKeyBoardMoving = !IsKeyBoardMoving;
		BeforePointPosition = pointObject.transform.localPosition;
	}

	void KeyBoardMove(){
		Vector3 nowkeyboard = KeyBoard.transform.localPosition;
		nowkeyboard.x += (pointObject.transform.localPosition.x - BeforePointPosition.x);
		nowkeyboard.y += (pointObject.transform.localPosition.y - BeforePointPosition.y);
		nowkeyboard.z += (pointObject.transform.localPosition.z - BeforePointPosition.z);
		KeyBoard.transform.localPosition = nowkeyboard;
		BeforePointPosition = pointObject.transform.localPosition;
	}

	bool IsCircle(){
		//本当はサークルがヒットしたらtrueにするつもりだったけどめんどくさいからGestureがなんかあったらで
		if (frame.Gestures ().Count > 0)
						return true;
		else
			return false;
	}

}
