using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//
public class InputManager : MonoBehaviour {



	void Awake(){
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		m_musicManager=GameObject.Find("MusicManager").GetComponent<MusicManager>();
		m_keypositionAction=GameObject.Find("PlayerAvator").GetComponent<KeyPositionAction>();
		m_scoringManager=GameObject.Find("ScoringManager").GetComponent<ScoringManager>();
		keyBehaviour = GameObject.Find ("Keys").GetComponent<KeyBehaviourScript> ();

	}

	// Update is called once per frame
	void Update () {
		//
		//
		//
		//
		if (!m_musicManager.IsPlaying ()) {
			return;
		}
		KeyPositionEnum KeyPosition
			=m_musicManager.currentSongInfo.onKeyPositionSequence[
			                                                     m_scoringManager.GetNearestPlayerActionInfoIndex()
			                                                     ].KeyPositionType;
		int keynumber;
		KeyDataScript keydata;
		if (m_musicManager.currentSongInfo.onKeyPositionSequence [
		            m_scoringManager.GetNearestPlayerActionInfoIndex ()].isWhiteKey) {
			keynumber = getWhiteKeyNumber (KeyPosition);
			keydata = keyBehaviour.WhitekeydataScripts [keynumber];
		} else {
			keynumber = getBlackKeyNumber (KeyPosition);
			keydata = keyBehaviour.BlackkeydataScripts [keynumber];
		}
		
		//if( Input.GetMouseButtonDown(0)){
		if(keydata.OnFingerNumber != -1  && !keydata.getGameCheckCount()){
			keydata.setGameCheckCount();//check true
			if (m_scoringManager.temper < ScoringManager.temperThreshold){
				//KeyPosition=KeyPositionEnum.HeadBanging;
				KeyPosition=KeyPositionEnum.Miss;
			}
			else{
				KeyPosition
					=m_musicManager.currentSongInfo.onKeyPositionSequence[
						m_scoringManager.GetNearestPlayerActionInfoIndex()
					                                                     ].KeyPositionType;
			}
			m_keypositionAction.SetNowKey(KeyPosition);
		}
	}



	//privaete variables
	MusicManager m_musicManager;
	KeyPositionAction m_keypositionAction;
	ScoringManager m_scoringManager;
	KeyBehaviourScript keyBehaviour;

	int getWhiteKeyNumber(KeyPositionEnum keyenum){
		string keystr = keyenum.ToString ();
		int keynumber = 0;
		switch (keystr.Substring(keystr.Length -1,1)) {
		case "C":
			keynumber = 14;
			break;
		case "D":
			keynumber = 15;
			break;
		case "E":
			keynumber = 16;
			break;
		case "F":
			keynumber = 17;
			break;
		case "G":
			keynumber = 18;
			break;
		case "A":
			keynumber = 19;
			break;
		case "H":
			keynumber = 20;
			break;
		default:
			keynumber = 0;
			break;
		}
		if(keystr.StartsWith("lowlow")){
			return keynumber - 14;
		}else if(keystr.StartsWith("low")){
			return keynumber - 7;
		}else if(keystr.StartsWith("highhigh")){
			return keynumber + 14;
		}else if(keystr.StartsWith("high")){
			return keynumber +7;
		}
		return keynumber;
	}
	int getBlackKeyNumber(KeyPositionEnum keyenum){
		string keystr = keyenum.ToString ();
		int keynumber = 0;
		switch (keystr.Substring(keystr.Length -1,1)) {
		case "C":
			keynumber = 10;
			break;
		case "D":
			keynumber = 11;
			break;
		case "F":
			keynumber = 12;
			break;
		case "G":
			keynumber = 13;
			break;
		case "A":
			keynumber = 14;
			break;
		default:
			keynumber = 0;
			break;
		}
		if(keystr.StartsWith("lowlow")){
			return keynumber - 10;
		}else if(keystr.StartsWith("low")){
			return keynumber - 5;
		}else if(keystr.StartsWith("highhigh")){
			return keynumber + 10;
		}else if(keystr.StartsWith("high")){
			return keynumber +5;
		}
		return keynumber;
		
	}
	

}
