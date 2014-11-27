using UnityEngine;
using System.Collections;
using Leap;

public class HandBehaviorScript : MonoBehaviour {

	Controller controller = new Controller();
	InteractionBox interactionBox = new InteractionBox();
	public GameObject LeftHand;
	public GameObject RightHand;
	//0~4 is leftfinger 5~9 is rightfinger
	public GameObject[] FingerObjects;
	HandFingerBehaviourScript[] fingerScripts;
	public GameObject KeysSceen;
	KeyBehaviourScript keyScript;
	HandDataScript LeftHandDataScript;
	HandDataScript RightHandDataScript;

	public string debug;
	public string debug2;

	// Use this for initialization
	void Start () {
		LeftHand.SetActive (false);
		RightHand.SetActive (false);
		LeftHandDataScript = LeftHand.GetComponent<HandDataScript> ();
		RightHandDataScript = RightHand.GetComponent<HandDataScript> ();
		fingerScripts = new HandFingerBehaviourScript[FingerObjects.Length];
		keyScript = KeysSceen.GetComponent<KeyBehaviourScript>();
		for(int i = 0; i<FingerObjects.Length; i++)
			fingerScripts[i] = FingerObjects[i].GetComponent<HandFingerBehaviourScript>();



	}
	//
	// Update is called once per frame
	void Update () {
				
		Frame frame = controller.Frame ();
				interactionBox = frame.InteractionBox;

				for (int i = 0; i < frame.Hands.Count; i++) {
						if (frame.Hands [i].IsLeft) {
								LeftHand.SetActive (true);
								ChangeLeftHandPosition (frame.Hands [i]);
								
						}
						if (frame.Hands [i].IsRight) {
								RightHand.SetActive (true);
								ChangeRightHandPosition (frame.Hands [i]);
						}
				}
		}


	void ChangeLeftHandPosition(Hand lefthand){
		//debug = HandAndFingerDistance (lefthand.PalmPosition, lefthand.Fingers [4].TipPosition, 1).ToString();
		Vector3 hand_v = getPositionForBox (lefthand.PalmPosition);
		if (LeftHandDataScript.CanMove (hand_v)) {
						ChangeHandPosition (hand_v, LeftHand);
				} else {
			//			LeftHand.renderer.material.color = Color.red;
				}

		for (int i = 0; i<lefthand.Fingers.Count; i++)
			ChangeFingersPosition (lefthand.Fingers [i],i,lefthand);
		}
	void ChangeRightHandPosition(Hand righthand){
		Vector3 hand_v = getPositionForBox (righthand.PalmPosition);
		if (RightHandDataScript.CanMove (hand_v)) {
						ChangeHandPosition (hand_v, RightHand);
				} else {
			//RightHand.renderer.material.color = Color.red;
				}

		for (int i = 0; i<righthand.Fingers.Count; i++)
			ChangeFingersPosition (righthand.Fingers [i],i,righthand);
	}

	void ChangeHandPosition(Vector3 hand_v,GameObject model_hand){
		model_hand.transform.localPosition 
			= new Vector3 (hand_v.x,
			               LeftHand.transform.localPosition.y,
			               LeftHand.transform.localPosition.z);
		//model_hand.renderer.material.color = Color.white;
		}


	void ChangeFingersPosition(Finger f,int i,Hand hand){


		if (f.Hand.IsRight) 
			i += 5; 
		//Vector3 fin = getPositionForBox(f.TipPosition);


		//whitekey push check
		//if (HandAndFingerDistance (hand.PalmPosition, f.TipPosition, 1) > 45) {
		if (!f.IsExtended){
				PushAction (i, true);
		} else if (HandAndFingerDistance (hand.PalmPosition, f.TipPosition, 1) < 0) {
				PushAction (i, false);
		}else if (fingerScripts [i].getIsPush ()) { 
				ReleseAction (i);
		}
 
	}



	void PushAction(int i,bool isWhite){
		fingerScripts [i].setIsWhite (isWhite);
		//debug = FingerObjects [i].transform.position.x.ToString();
		if (!fingerScripts [i].getIsPush ()) {//今押されたのなら
			PushNow (i,isWhite);
		} else if(fingerScripts [i].getkeyNumber() != -1) {
			Pushed (i,isWhite);
		}

	}
	void PushNow(int i,bool isWhite)
	{
		FingerObjects [i].transform.Rotate (-20, 0, 0);
		int key_number = -1;
		if(isWhite)
			key_number = keyScript.SearchWhiteKey(FingerObjects[i].transform.position,i);
		else
			key_number = keyScript.SearchBlackKey(FingerObjects[i].transform.position,i);

		fingerScripts[i].setParam(true,key_number);
	}
	void Pushed(int i,bool isWhite){
		//set and return isRotateDicect
		int CannotMoveDirect 
			= fingerScripts [i].SetRotateYAndIsRotate (FingerObjects [i].transform.eulerAngles.y);
		debug2 = fingerScripts [i].getkeyNumber().ToString();
		Vector3 keyPosition = keyScript.getKeyPosition(fingerScripts [i].getkeyNumber(),isWhite);
		FingerObjects[i].transform.rotation = 
			fingerScripts[i].FingerRotation(
				FingerObjects[i].transform.rotation,
				keyPosition
				);
		//cannot move to left
		if(CannotMoveDirect == 1){
			if(i < 5)
				LeftHandDataScript.setCannotMoveDirect(1,i);
			else
				RightHandDataScript.setCannotMoveDirect(1,i);
		//caonnot move to right
		}else if(CannotMoveDirect == -1){
			if(i < 5)
				LeftHandDataScript.setCannotMoveDirect(-1,i);
			else
				RightHandDataScript.setCannotMoveDirect(-1,i);
		}
	}

	void ReleseAction(int finN){
		//FingerObjects[i].transform.Rotate(0,0,0);//指を上げる
		FingerObjects [finN].transform.eulerAngles = new Vector3 (-60, 180, 0);
		keyScript.StopKey(fingerScripts[finN].getkeyNumber(),
		                  finN,
		                  fingerScripts[finN].getIsWhite());
		fingerScripts[finN].setParam(false,-1);
		fingerScripts[finN].setIsWhite(false);
		if(finN < 5)
			LeftHandDataScript.ReleaseFinger(finN);
		else
			RightHandDataScript.ReleaseFinger(finN);
	}

	float HandAndFingerDistance(Vector hand,Vector fing,int kind){
		if (kind == 0)
						return hand.x - fing.x;
				else if (kind == 1)
						return hand.y - fing.y;
				else if (kind == 2)
						return hand.z - fing.z;

		return 0;
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
