using UnityEngine;
using System.Collections;

public class HandFingerBehaviourScript : MonoBehaviour {

	bool Ispushing = false;
	int pushingKeyNumber = -1;
	public string debug;
	float Rotation_Y;
	bool PushingKeyIsWhite;


	// Use this for initialization
	void Start () {
		Ispushing = false;
		pushingKeyNumber = -1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setParam(bool is_pushing,int KeyNumber){
		Ispushing = is_pushing;
		pushingKeyNumber = KeyNumber;
	}
	public int getkeyNumber(){
		return pushingKeyNumber;
	}
	public void setIsWhite(bool is_white){
		PushingKeyIsWhite = is_white;
		}
	public bool getIsWhite(){
		return PushingKeyIsWhite;
	}
	public bool getIsPush(){
		return Ispushing;
	}
	//finger rotate
	public Quaternion FingerRotation( Quaternion q,Vector3 direction )
	{
		Quaternion fq = Quaternion.LookRotation(direction - this.transform.position);
		Quaternion digq = Quaternion.AngleAxis (180f, Vector3.up);
		return fq * digq;
	}
	public int SetRotateYAndIsRotate(float rot_y){
		Rotation_Y = rot_y;
		if (Rotation_Y > 210) {
			return -1;
				}
		else if(Rotation_Y < 150){
			return 1;
		}
		return 0;
	}

}
