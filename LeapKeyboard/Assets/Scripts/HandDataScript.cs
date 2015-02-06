using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class HandDataScript : MonoBehaviour {
	//0 OK 1 not to right -1 not to left
	int CannotMoveDirect;
	float BeforePosX;
	// list of StoppingFingerNumber
	List<int> LimitRotateFingerList = new List<int>();
	public string debug;

	// Use this for initialization
	void Start () {
		CannotMoveDirect = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (LimitRotateFingerList.Count == 0)
						CannotMoveDirect = 0;
	}

	public void setCannotMoveDirect(int can_move,int fingerNumber){
		if(LimitRotateFingerList.IndexOf(fingerNumber) == -1)
			LimitRotateFingerList.Add (fingerNumber);
		this.CannotMoveDirect = can_move;
	}
	public int getCannotMoveDirect(){
		return this.CannotMoveDirect;
	}
	public bool CanMove(Vector3 WillHandPosition){
		if (CannotMoveDirect == -1 && BeforePosX > WillHandPosition.x) {
						return false;
				} else if (CannotMoveDirect == 1 && BeforePosX < WillHandPosition.x) {
						return false;
				}

		CannotMoveDirect = 0;
		BeforePosX = WillHandPosition.x;
		return true;
	}

	public void ReleaseFinger(int fingerNumber){
		LimitRotateFingerList.RemoveAll(i => i == fingerNumber);
	}


}
