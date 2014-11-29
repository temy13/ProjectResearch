using UnityEngine;
using System.Collections;
using Leap;


public class KeyBehaviourScript : MonoBehaviour {

	public GameObject[] WhiteKeyObjects;
	public GameObject[] BlackKeyObjects;
	KeyDataScript[] WhitekeydataScripts;
	KeyDataScript[] BlackkeydataScripts;
	public string debug;
	public GameObject KeyBoardObject;



	// Use this for initialization
	void Start () {
		WhitekeydataScripts = new KeyDataScript[WhiteKeyObjects.Length];
		BlackkeydataScripts = new KeyDataScript[BlackKeyObjects.Length];
		for (int i = 0; i<WhiteKeyObjects.Length; i++) {
			WhitekeydataScripts[i] = WhiteKeyObjects [i].GetComponent<KeyDataScript> ();
		}
		for (int i = 0; i<BlackKeyObjects.Length; i++) {
			BlackkeydataScripts[i] = BlackKeyObjects [i].GetComponent<KeyDataScript> ();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int IsPush(int keyN,float pos_x_dif,int fingerNumber,bool isWhite){
		if (Mathf.Abs(pos_x_dif) < 0.80) {
			SoundKey(keyN,fingerNumber,isWhite); 
			return keyN;
		}
		return -1;
		
	}
	//sound
	void SoundKey(int i,int fingerNumber,bool isWhite){
		if (isWhite) {
						WhiteKeyObjects [i].renderer.material.color = Color.red;
						WhitekeydataScripts [i].PlaySound (fingerNumber);
				} else {
						BlackKeyObjects [i].renderer.material.color = Color.red;
						BlackkeydataScripts [i].PlaySound (fingerNumber);
				}
	}

	public void StopKey(int i,int fingerNumber,bool isWhite){
		//debug = i.ToString ();
		Debug.Log(string.Format ("{0},{1},{2}",i.ToString(),fingerNumber.ToString(),isWhite.ToString()));
		if (isWhite) {
						if (i != -1 && WhitekeydataScripts [i].OnFingerNumber == fingerNumber) {
								WhiteKeyObjects [i].renderer.material.color = Color.white;
								WhitekeydataScripts [i].StopSound ();
						}
				} else {
						if (i != -1 && i < 25 && BlackkeydataScripts [i].OnFingerNumber == fingerNumber) {
							BlackKeyObjects [i].renderer.material.color = Color.black;
							BlackkeydataScripts [i].StopSound ();
						}
				}
	}

	public int SearchWhiteKey(Vector3 fv, int fingerNumber){
		for (int i = 0; i<WhiteKeyObjects.Length; i++) {
			int key = IsPush(i,WhiteKeyObjects[i].transform.position.x-fv.x,
			                 fingerNumber,true);
			if(key != -1)
				return key;
		}
		return -1;
	}
	public int SearchBlackKey(Vector3 fv, int fingerNumber){
		for (int i = 0; i<BlackKeyObjects.Length; i++) {
			int key = IsPush(i,BlackKeyObjects[i].transform.position.x-fv.x,
			                      fingerNumber,false);
			if(key != -1)
				return key;
		}
		return -1;
	}

	public Vector3 getKeyPosition(int i,bool isWhite){
		if (isWhite)
						return WhiteKeyObjects [i].transform.position;
				else
						return BlackKeyObjects [i].transform.position;

		}

}
