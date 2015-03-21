using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//gui
public class OnPlayGUI : MonoBehaviour {
	public Texture messageTexture_Best;
	public Texture messageTexture_Good;
	public Texture messageTexture_Miss;
	public Texture headbangingIcon;
	public Texture beatPositionIcon;
	public Texture hitEffectIcon;
	public Texture temperBar;
	public Texture temperBarFrame;

	public static float markerEnterOffset = 2.5f;	//start marker 's timing
	public static float markerLeaveOffset =-1.0f;	//finish marker

	public static int rythmHitEffectShowFrameDuration = 20;
	public static int messatShowFrameDuration = 40;

	public bool isDevelopmentMode=false;
	//public Vector2[] markerOrigin = new Vector2[2];

	public GUISkin	guiSkin;

	GameObject whitekeys;
	List<GameObject> WhiteKeyObjects = new List<GameObject>();
	GameObject blackkeys;
	List<GameObject> BlackKeyObjects = new List<GameObject>();
	List<GameObject> TargetSpheres = new List<GameObject>();

	float z_offset = 0;
	int keynumber = 0;

	public void BeginVisualization(){
		m_musicManager=GameObject.Find("MusicManager").GetComponent<MusicManager>();
		m_scoringManager=GameObject.Find("ScoringManager").GetComponent<ScoringManager>();
		m_seekerBack.SetSequence(m_musicManager.currentSongInfo.onKeyPositionSequence);
		m_seekerFront.SetSequence(m_musicManager.currentSongInfo.onKeyPositionSequence);
		m_seekerBack.Seek(markerLeaveOffset);
		m_seekerFront.Seek(markerEnterOffset);
		//get white key object
		whitekeys = GameObject.Find ("WhiteKeys");
		foreach( Transform child in whitekeys.transform)
		{ 
			GameObject childObj = child.gameObject;
			WhiteKeyObjects.Add(childObj);
		}
		//get black key object
		blackkeys = GameObject.Find ("BlackKeys");
		foreach( Transform child in blackkeys.transform)
		{ 
			GameObject childObj = child.gameObject;
			BlackKeyObjects.Add(childObj);
			
		}
//		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//		GameObject temp = WhiteKeyObjects [10];
//		sphere.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 5);
	}
    public void RythmHitEffect(int actionInfoIndex, float score)
    {	

		m_lastInputScore = score;
		m_rythmHitEffectCountDown = rythmHitEffectShowFrameDuration;
		m_messageShowCountDown=messatShowFrameDuration;
		if(score<0){
			m_playerAvator.GetComponent<AudioSource>().clip
				= m_playerAvator.GetComponent<KeyPositionAction>().headBangingSoundClip_BAD;
			messageTexture = messageTexture_Miss;
		}
		else if(score<=ScoringManager.goodScore){
			m_playerAvator.GetComponent<AudioSource>().clip
				= m_playerAvator.GetComponent<KeyPositionAction>().headBangingSoundClip_GOOD;
			messageTexture = messageTexture_Good;
		}
		else{
			m_playerAvator.GetComponent<AudioSource>().clip
				= m_playerAvator.GetComponent<KeyPositionAction>().headBangingSoundClip_GOOD;
			messageTexture = messageTexture_Best;
		}
		m_playerAvator.GetComponent<AudioSource>().Play();
    }

	// Use this for initialization
	void Start(){
		m_musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		m_scoringManager = GameObject.Find("ScoringManager").GetComponent<ScoringManager>();
		m_playerAvator = GameObject.Find("PlayerAvator");

		//markerOrigin[0] = new Vector2(20.0f, 300.0f);
		//markerOrigin[1] = new Vector2(85.0f, 300.0f);
	}
	public void Seek(float beatCount){
		m_seekerBack.Seek(beatCount + markerLeaveOffset);
		m_seekerFront.Seek(beatCount + markerEnterOffset);
	}
	// Update is called once per frame
	void Update () {
		if(m_musicManager.IsPlaying()){
			m_seekerBack.ProceedTime( m_musicManager.beatCountFromStart - m_musicManager.previousBeatCountFromStart);
			m_seekerFront.ProceedTime( m_musicManager.beatCountFromStart -m_musicManager.previousBeatCountFromStart);
		}
		//string temp = string.Format ("{0}:{1}",z_offset,keynumber);
		//Debug.Log (temp);
	}
	void OnGUI(){
		//score
		GUI.Box(new Rect(15,5,100,30),"");
		GUI.Label(new Rect(20,10,90,20),"Score: " + m_scoringManager.score);
		//hightension
		if (m_scoringManager.temper > ScoringManager.temperThreshold)
		{
			m_blinkColor.g = m_blinkColor.b
				= 0.7f + 0.3f * Mathf.Abs(
					Time.frameCount % Application.targetFrameRate - Application.targetFrameRate / 2
				) / (float)Application.targetFrameRate;
			GUI.color=m_blinkColor;
		}
		//moriagari gage
		Rect heatBarFrameRect=new Rect(180.0f, 20.0f, 100.0f, 20.0f);
		Rect heatBarRect = heatBarFrameRect;
		Rect heatBarLabelRect = heatBarFrameRect;
		heatBarRect.width *= m_scoringManager.temper;
		heatBarLabelRect.y = heatBarFrameRect.y-20;
		GUI.Label(heatBarLabelRect, "Temper");
		GUI.Box( heatBarFrameRect,"" );
		GUI.DrawTextureWithTexCoords( 
			heatBarRect, temperBar, new Rect(0.0f, 0.0f, 1.0f * m_scoringManager.temper, 1.0f)
		);
		GUI.color = Color.white;
		//when this icon & actions's aicon is overlap  input
//		float 	markerSize = ScoringManager.timingErrorToleranceGood * m_pixelsPerBeats;
		//draw marker
		/*for (int i = 0; i<markerOrigin.Length; i++) {
						Graphics.DrawTexture (
			new Rect (markerOrigin [i].x - markerSize / 2.0f, markerOrigin [i].y - markerSize / 2.0f, markerSize, markerSize)
			, beatPositionIcon
						);
				}*/

		// if not sound
		if (! m_musicManager.IsPlaying ()) {
			return ;
		}
			SongInfo song =  m_musicManager.currentSongInfo;
			int	begin = m_seekerBack.nextIndex;			//start display marker
			int end   = m_seekerFront.nextIndex;			//finish display marker.
			//display action timing marker
			for ( int drawnIndex = begin; drawnIndex < end; drawnIndex++) {

			OnKeyPositionInfo info = song.onKeyPositionSequence[drawnIndex];
			//float size = ScoringManager.timingErrorToleranceGood * m_pixelsPerBeats;
			//if hightension, marker is big
			/*if (m_scoringManager.temper > ScoringManager.temperThreshold)
			{
				size *= 1.5f;
			}*/
			// y's offset
			z_offset = info.triggerBeatTiming - m_musicManager.beatCount;
			if(info.isWhiteKey)
				keynumber = getWhiteKeyNumber(info.KeyPositionType);
			else
				keynumber = getBlackKeyNumber(info.KeyPositionType);
			//if new marker is created, make new sphere
			if(TargetSpheres.Count <= drawnIndex){
				GameObject sphere_target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				GameObject key ;

				if(info.isWhiteKey){
					key = WhiteKeyObjects [keynumber];
				}else {
					key = BlackKeyObjects [keynumber];
					sphere_target.renderer.material.color = Color.black;
				}
				sphere_target.transform.position = new Vector3(key.transform.position.x, key.transform.position.y, 0);
				TargetSpheres.Add(sphere_target);
			}else if(TargetSpheres.Count > whitekeys.transform.position.y && z_offset*10 > whitekeys.transform.position.y){
				TargetSpheres[drawnIndex].transform.position = new Vector3(TargetSpheres[drawnIndex].transform.position.x, 
				                                                           TargetSpheres[drawnIndex].transform.position.y, 
				                                                           z_offset*10 -3);//keyboard's z point is -3
			}else if(z_offset*10 < whitekeys.transform.position.y)
			{
				TargetSpheres[drawnIndex].SetActive(false);
			}
			z_offset *= m_pixelsPerBeats;
				/*Rect drawRect = new Rect(
					markerOrigin[keynumber].x - size/2.0f ,
					markerOrigin[keynumber].y - size/2.0f -  z_offset ,
					size,
					size
				);*/

			
				//GUI.DrawTexture( drawRect, headbangingIcon );
				GUI.color = Color.white;

				// the number of textfile
				if( isDevelopmentMode ){
					GUI.skin = this.guiSkin;
					//GUI.Label(new Rect(drawRect.x, drawRect.y - 10.0f, 50.0f, 30.0f), info.line_number.ToString());
					GUI.skin = null;
				}
			}

			//action timing's hit effect
			if( m_rythmHitEffectCountDown>0  ){
				float scale=2.0f - m_rythmHitEffectCountDown / (float)rythmHitEffectShowFrameDuration;
				if( m_lastInputScore >= ScoringManager.excellentScore){
					scale *= 2.0f;
				}
				else if( m_lastInputScore > ScoringManager.missScore){
					scale *= 1.2f;
				}
				else{
					scale *= 0.5f;
				}
//				float baseSize = 32.0f;
				/*Rect drawRect3 = new Rect(
				markerOrigin[keynumber].x - baseSize * scale / 2.0f,
				markerOrigin[keynumber].y - baseSize * scale / 2.0f,
					baseSize * scale,
					baseSize * scale
				);
				Graphics.DrawTexture(drawRect3, hitEffectIcon);*/
				m_rythmHitEffectCountDown--;
			}
			//show message
			if( m_messageShowCountDown > 0 ){
				GUI.color=new Color(1, 1, 1, m_messageShowCountDown/40.0f );
				GUI.DrawTexture(new Rect(20,230,150,50),messageTexture,ScaleMode.ScaleAndCrop, true);
				GUI.color=Color.white;
				m_messageShowCountDown--;

		}
	}
	//private Variables
	float	m_pixelsPerBeats = Screen.width * 1.0f/markerEnterOffset;
	int		m_messageShowCountDown=0;
	int		m_rythmHitEffectCountDown = 0;
	float	m_lastInputScore = 0;
	Color	m_blinkColor = new Color(1,1,1);


	// seek unit( finish position) advance
	SequenceSeeker<OnKeyPositionInfo> m_seekerFront = new SequenceSeeker<OnKeyPositionInfo>();

	// seek unit( start position) finish
	SequenceSeeker<OnKeyPositionInfo> m_seekerBack = new SequenceSeeker<OnKeyPositionInfo>();

	MusicManager	m_musicManager;
	ScoringManager	m_scoringManager;
	GameObject      m_playerAvator;
	Texture 		messageTexture;

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
