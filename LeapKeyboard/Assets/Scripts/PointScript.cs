using UnityEngine;
using System.Collections;
using System;
using Leap;

public class PointScript : MonoBehaviour {
	public GameObject PointSphere;
	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();
	Frame frame;

	public GameObject[] Modes;
	SceneScript sc;


	// Use this for initialization
	void Start () {
		//PointSphere.SetActive (false);
		sc = this.GetComponent<SceneScript> ();
		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

	}
	
	// Update is called once per frame
	void Update () {
		frame = controller.Frame ();
		interactionBox = frame.InteractionBox;
		for (int i = 0; i<frame.Hands.Count; i++) {
			if(frame.Hands[i].PalmPosition.y > 180){
				PointMove(frame.Hands[i]);
				break;
			}
		}
	}

	void PointMove(Hand hand){
		PointSphere.SetActive (true);
		PointSphere.transform.localPosition = getPositionForPoint (hand.PalmPosition);
		CheckPosition ();
	}

	void CheckPosition(){
		for(int i =0; i<Modes.Length; i++){
			if(Vector2DistanceToMode(
			PointSphere.transform.position,
			Modes[i].transform.position)){
				Modes[i].renderer.material.color = Color.green;
				ChangeSceneCheck(i);
			}else if(Modes[i].renderer.material.color.Equals(Color.green))
			         {
				Modes[i].renderer.material.color = Color.white;
			}
		}
	}
	
	bool Vector2DistanceToMode(Vector3 sphere, Vector3 mode){
		Debug.Log (Modes[0].renderer.bounds.size.z.ToString ());
		return(Math.Abs(sphere.x - mode.x) < Modes[0].renderer.bounds.size.x &&
		Math.Abs(sphere.z - mode.z) < Modes[0].renderer.bounds.size.z );
	}

	void ChangeSceneCheck(int i){
		//if (IsCircle ()) {
			Modes[i].renderer.material.color = Color.white;
						sc.ChangeScene (i + 1);
		//		}
	}

	bool IsCircle(){
		//本当はサークルがヒットしたらtrueにするつもりだったけどめんどくさいからGestureがなんかあったらで
		return (frame.Gestures ().Count > 0);
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


}
