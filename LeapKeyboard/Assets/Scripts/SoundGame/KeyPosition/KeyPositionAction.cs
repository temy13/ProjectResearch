using UnityEngine;
using System.Collections;
//player's action
public enum KeyPositionEnum{
	None,Miss,
	lowlowC,
	lowlowCc,
	lowlowD,
	lowlowDc,
	lowlowE,
	lowlowF,
	lowlowFc,
	lowlowG,
	lowlowGc,
	lowlowA,
	lowlowAc,
	lowlowH,
	lowC,
	lowCc,
	lowD,
	lowDc,
	lowE,
	lowF,
	lowFc,
	lowG,
	lowGc,
	lowA,
	lowAc,
	lowH,
	C,
	Cc,
	D,
	Dc,
	E,
	F,
	Fc,
	G,
	Gc,
	A,
	Ac,
	H,
	highC,
	highCc,
	highD,
	highDc,
	highE,
	highF,
	highFc,
	highG,
	highGc,
	highA,
	highAc,
	highH,
	highhighC,
	highhighCc,
	highhighD,
	highhighDc,
	highhighE,
	highhighF,
	highhighFc,
	highhighG,
	highhighGc,
	highhighA,
	highhighAc,
	highhighH
};
//action
public class KeyPositionAction : MonoBehaviour {
	public AudioClip headBangingSoundClip_GOOD;
	public AudioClip headBangingSoundClip_BAD;
	//now action
	public KeyPositionEnum currentPlayerAction{
		get{ return m_currentKeyPosition; }
	}
	//lasta action
	public OnKeyPositionInfo lastActionInfo{
		get{ return m_lastActionInfo; }
	}
	// Use this for initialization
	void Start () {
		m_musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
	}
	
	// Update is called once per frame
	void Update () {
		m_currentKeyPosition = m_newKeyPosition;
		m_newKeyPosition = KeyPositionEnum.None;
	}
	public void SetNowKey(KeyPositionEnum KeyPosition){
		m_newKeyPosition = KeyPosition;
		
		OnKeyPositionInfo actionInfo = new OnKeyPositionInfo();
		actionInfo.triggerBeatTiming = m_musicManager.beatCountFromStart;
		actionInfo.KeyPositionType = m_newKeyPosition;
		m_lastActionInfo = actionInfo;
		
		/*if(actionType == PlayerActionEnum. HeadBanging){
			gameObject.GetComponent<SimpleSpriteAnimation>().BeginAnimation(2, 1, false);
		}
		else if (actionType == PlayerActionEnum.Jump)
		{	
			gameObject.GetComponent<SimpleActionMotor>().Jump();
			gameObject.GetComponent<SimpleSpriteAnimation>().BeginAnimation(1, 1, false);
		}*/
	}
	//入力に対応したアクションを行う
	//Private variables
	MusicManager m_musicManager;
	OnKeyPositionInfo m_lastActionInfo=new OnKeyPositionInfo();
	KeyPositionEnum m_currentKeyPosition;
	KeyPositionEnum m_newKeyPosition;
}
