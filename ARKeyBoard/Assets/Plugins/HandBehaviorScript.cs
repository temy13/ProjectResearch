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


	// Use this for initialization
	void Start () {
		LeftHand.SetActive (false);
		RightHand.SetActive (false);
		LeftHandDataScript = LeftHand.GetComponent<HandDataScript> ();
		RightHandDataScript = RightHand.GetComponent<HandDataScript> ();
		keyScript = KeysSceen.GetComponent<KeyBehaviourScript>();
		fingerScripts = new HandFingerBehaviourScript[FingerObjects.Length];
		for(int i = 0; i<FingerObjects.Length; i++)
			fingerScripts[i] = FingerObjects[i].GetComponent<HandFingerBehaviourScript>();
	}
	//
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame ();
				interactionBox = frame.InteractionBox;

		for(int i = 0; i<frame.Hands.Count; i++){
			if(frame.Hands[i].IsLeft && HandPositionYCheck(frame.Hands[i],true)){
				LeftHand.SetActive (true);
				ChangeLeftHandPosition (frame.Hands[i]);
			}
			if(frame.Hands[i].IsRight && HandPositionYCheck(frame.Hands[i],false)){
				RightHand.SetActive (true);
				ChangeRightHandPosition (frame.Hands[i]);
			}
		}

//		if (frame.Hands.Leftmost.IsLeft && HandPositionYCheck(frame.Hands.Leftmost)) {
//						LeftHand.SetActive (true);
//						ChangeLeftHandPosition (frame.Hands.Leftmost);
//				} else {
//						LeftHand.SetActive (false);
//				}
//		if (frame.Hands.Count > 0 && frame.Hands.Rightmost.IsRight && HandPositionYCheck(frame.Hands.Rightmost)) {
//						RightHand.SetActive (true);
//						ChangeLeftHandPosition (frame.Hands.Rightmost);
//		} else {
//			RightHand.SetActive (false);
//		}
		}

	bool HandPositionYCheck(Hand hand,bool isleft){

		if (hand.PalmPosition.y > 200) {
						if (isleft) {
								LeftHand.SetActive (false);
								
						} else {
								RightHand.SetActive (false);
						}
						return false;
				} else {
						return true;
				}
		}


	void ChangeLeftHandPosition(Hand lefthand){
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
		//i is fingernumber
		if (f.Hand.IsRight) 
			i += 5; 
		//whitekey push check
		//if (HandAndFingerDistance (hand.PalmPosition, f.TipPosition, 1) > 45) {
		if (!f.IsExtended && HandAndFingerDistance (hand.PalmPosition, f.TipPosition, 1) > 30) {
						PushAction (i, true);
		} else if (HandAndFingerDistance (hand.PalmPosition, f.TipPosition, 1) < -5) {
				PushAction (i, false);
		}else if (fingerScripts [i].getIsPush ()) { 
				ReleseAction (i);
		}
	}
	
	void PushAction(int i,bool isWhite){
		//fingerScripts [i].setIsWhite (isWhite);
		if (!fingerScripts [i].getIsPush ()) {//今押されたのなら
			PushNow (i,isWhite);
		} else if(fingerScripts [i].getkeyNumber() != -1) {
			Pushed (i,isWhite);
		}
	}
	void PushNow(int i,bool isWhite){
		FingerObjects [i].transform.Rotate (-20, 0, 0);
		int key_number = -1;
		if (isWhite) {
						key_number = keyScript.SearchWhiteKey (FingerObjects [i].transform.position, i);
						fingerScripts [i].setParam (true, key_number, 1);
				} else {
						key_number = keyScript.SearchBlackKey (FingerObjects [i].transform.position, i);
						fingerScripts [i].setParam (true, key_number, 2);
				}
	}
	void Pushed(int i,bool isWhite){
		//もし白の鍵盤の番号になっていたらやらない
		if (i >= 25 && !isWhite)
			return;
		//set and return isRotateDicect
		int CannotMoveDirect 
			= fingerScripts [i].SetRotateYAndIsRotate (FingerObjects [i].transform.eulerAngles.y);

		Vector3 keyPosition = keyScript.getKeyPosition(fingerScripts [i].getkeyNumber(),isWhite);
		FingerObjects[i].transform.rotation = 
			fingerScripts[i].FingerRotation(
				FingerObjects[i].transform.rotation,
				keyPosition
				);
		//cannot move to left or right
		if(CannotMoveDirect == 1 || CannotMoveDirect == -1){
			if(i < 5){
				LeftHandDataScript.setCannotMoveDirect(CannotMoveDirect,i);
			}else{
				RightHandDataScript.setCannotMoveDirect(CannotMoveDirect,i);
			}
		}
	}

	void ReleseAction(int finN){
		//FingerObjects[i].transform.Rotate(0,0,0);//指を上げる
		FingerObjects [finN].transform.eulerAngles = new Vector3 (-60, 180, 0);
		keyScript.StopKey(fingerScripts[finN].getkeyNumber(),
		                  finN,
		                  fingerScripts[finN].getIsWhite());
		fingerScripts[finN].setParam(false,-1,0);
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
