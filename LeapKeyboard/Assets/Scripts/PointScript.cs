using UnityEngine;
using System.Collections;
using Leap;

public class PointScript : MonoBehaviour {
	public GameObject PointSphere;
	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();
	Frame frame;

	public GameObject SettingSwitchObject;
	public GameObject SettingObject;

	SettingScript stc;

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
		for (int i = 0; i<frame.Hands.Count; i++) {
			if(frame.Hands[i].PalmPosition.y > 180 || stc.IsKeyBoardMoving){
				PointMove(frame.Hands[i]);
				break;
			}
		}
	}

	void PointMove(Hand hand){
		PointSphere.SetActive (true);
		PointSphere.transform.localPosition = getPositionForPoint (hand.PalmPosition);
		SettingSwitchCheck ();

		stc.CheckPosition ();
	}

	void SettingSwitchCheck(){
		if (Vector3.Distance (PointSphere.transform.position, SettingSwitchObject.transform.position) < 2.0 && IsCircle()) {
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


}
