using UnityEngine;
using System.Collections;
using System;
using Leap;

public class PointScript : MonoBehaviour {
	public GameObject PointSphere;
	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();

	public GameObject[] Modes;
	SceneScript sc;


	// Use this for initialization
	void Start () {
		//PointSphere.SetActive (false);
		sc = this.GetComponent<SceneScript> ();
		//sc.ChangeScene(1);

	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
		interactionBox = frame.InteractionBox;
		for (int i = 0; i<frame.Hands.Count; i++) {
			if(frame.Hands[i].PalmPosition.y > 200){
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
				//move to other scene
				sc.ChangeScene(i+1);
			}
		}
	}
	
	bool Vector2DistanceToMode(Vector3 sphere, Vector3 mode){
		Debug.Log (Modes[0].renderer.bounds.size.z.ToString ());
		return(Math.Abs(sphere.x - mode.x) < Modes[0].renderer.bounds.size.x &&
		Math.Abs(sphere.z - mode.z) < Modes[0].renderer.bounds.size.z );
	}

	Vector3 getPositionForBox(Vector v){
		Vector normalizedPosition = interactionBox.NormalizePoint(v);
		normalizedPosition *= 30;
		//normalizedPosition.x += 10;
		return ToVector3 (normalizedPosition);
	}
	
	Vector3 ToVector3( Vector v )
	{
		return new UnityEngine.Vector3( v.x, v.y, v.z );
	}


}
