using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//
public class ScoringManager : MonoBehaviour {
	public static float timingErrorToleranceGood = 0.22f;			// 
	public static float timingErrorTorelanceExcellent = 0.12f;		// 
	public static float missScore = -1.0f;
	public static float goodScore = 2.0f;
	public static float excellentScore = 4.0f;
	public static float failureScoreRate = 0.3f;//
	public static float excellentScoreRate = 0.85f;//
	public static float missHeatupRate = -0.08f;
	public static float goodHeatupRate = 0.01f;
	public static float bestHeatupRate = 0.02f;
	public static float temperThreshold = 0.5f;//
	public bool outScoringLog=true;
	//
	public float score{
		get{ return m_score; }
	}
	private float m_score;

	//
	public float temper
	{
		get { return m_temper; }
		set { m_temper = Mathf.Clamp(value, 0, 1); }
	}
	float m_temper = 0;
	//
	public float scoreJustAdded{
		get{ return m_additionalScore; }
	}

	//
	public float scoreRate
	{
		get { return m_scoreRate; }
	}
	private float m_scoreRate = 0;

	//
	public void BeginScoringSequence(){
		m_scoringUnitSeeker.SetSequence(m_musicManager.currentSongInfo.onKeyPositionSequence);
	}
	// Use this for initialization
    void Start()
    {
		m_musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		m_keypositionAction = GameObject.Find("PlayerAvator").GetComponent<KeyPositionAction>();
		//m_bandMembers = GameObject.FindGameObjectsWithTag("BandMember");
		//m_audiences = GameObject.FindGameObjectsWithTag("Audience");
		m_noteParticles = GameObject.FindGameObjectsWithTag("NoteParticle");
		m_phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
		//
		m_onPlayGUI    = m_phaseManager.guiList[1].GetComponent<OnPlayGUI>();
#if UNITY_EDITOR 
        m_logWriter = new StreamWriter("Assets/Game/PlayLog/scoringLog.csv");
#endif
    }
	public void Seek(float beatCount){
		m_scoringUnitSeeker.Seek( beatCount );
		m_previousHitIndex=-1;
	}
	// 
	public int	GetNearestPlayerActionInfoIndex(){

		SongInfo	song = m_musicManager.currentSongInfo;
		int 		nearestIndex = 0;

		if(m_scoringUnitSeeker.nextIndex == 0) {

			// 
			nearestIndex = 0;

		} else if(m_scoringUnitSeeker.nextIndex >= song.onKeyPositionSequence.Count) {

			//

			nearestIndex = song.onKeyPositionSequence.Count - 1;

		} else {

			// 

			OnKeyPositionInfo	crnt_action = song.onKeyPositionSequence[m_scoringUnitSeeker.nextIndex];			// 
			OnKeyPositionInfo	prev_action = song.onKeyPositionSequence[m_scoringUnitSeeker.nextIndex - 1];		// 

			float				act_timing = m_keypositionAction.lastActionInfo.triggerBeatTiming;
			if( crnt_action.triggerBeatTiming - act_timing < act_timing - prev_action.triggerBeatTiming) {
				nearestIndex = m_scoringUnitSeeker.nextIndex;
			} else {
				nearestIndex = m_scoringUnitSeeker.nextIndex - 1;
			}
		}

		return(nearestIndex);
	}

	// Update is called once per frame
	void Update () {

		m_additionalScore = 0;

		float additionalTemper = 0;
		bool hitBefore = false;
		bool hitAfter = false;

		if( m_musicManager.IsPlaying() ){

			float	delta_count = m_musicManager.beatCount - m_musicManager.previousBeatCount;

			m_scoringUnitSeeker.ProceedTime(delta_count);
			// 
			// 
			if(m_keypositionAction.currentPlayerAction != KeyPositionEnum.None){
				int nearestIndex = GetNearestPlayerActionInfoIndex();

				SongInfo song = m_musicManager.currentSongInfo;

				OnKeyPositionInfo marker_act = song.onKeyPositionSequence[nearestIndex];
				OnKeyPositionInfo player_act = m_keypositionAction.lastActionInfo;

				m_lastResult.timingError = player_act.triggerBeatTiming - marker_act.triggerBeatTiming;
				m_lastResult.markerIndex = nearestIndex;

				if (nearestIndex == m_previousHitIndex){
					// 
					m_additionalScore = 0;

				} else {

					// 
					// 
					m_additionalScore = CheckScore(nearestIndex, m_lastResult.timingError, out additionalTemper);
				}

				if (m_additionalScore > 0){

					// 

					// 
					// 
					m_previousHitIndex = nearestIndex;

					// 
					// 
					// 
					// 
					//
					if (nearestIndex == m_scoringUnitSeeker.nextIndex)
						hitAfter = true;
					else
						hitBefore = true;

					//
					OnScoreAdded(nearestIndex);
				} else{

					// 

					//
					m_additionalScore = missScore;

					additionalTemper = missHeatupRate;
				}
				m_score += m_additionalScore;

				temper += additionalTemper;
				m_onPlayGUI.RythmHitEffect(m_previousHitIndex, m_additionalScore);
				// 
				DebugWriteLogPrev();
				DebugWriteLogPost(hitBefore, hitAfter);
			}
			if (m_scoringUnitSeeker.nextIndex > 0)
				m_scoreRate = m_score / (m_scoringUnitSeeker.nextIndex * excellentScore);
		}
	}

	// 
	float CheckScore(int actionInfoIndex, float timingError, out float heatup){

		float	score = 0;

		timingError = Mathf.Abs(timingError);

		do {

			// 
			if(timingError >= timingErrorToleranceGood) {

				score  = 0.0f;
				heatup = 0;
				break;
			}
			
			//
			if(timingError >= timingErrorTorelanceExcellent) {

				score  = goodScore;
				heatup = goodHeatupRate;
				break;
			}

			// 
			score  = excellentScore;
			heatup = bestHeatupRate;

		} while(false);

		return(score);
	}

	// 
	private	void	DebugWriteLogPrev()
	{
#if UNITY_EDITOR
		if( m_scoringUnitSeeker.isJustPassElement ){
			if(outScoringLog){
				OnKeyPositionInfo onKeyPositionInfo
					= m_musicManager.currentSongInfo.onKeyPositionSequence[m_scoringUnitSeeker.nextIndex-1];
				m_logWriter.WriteLine(
					onKeyPositionInfo.triggerBeatTiming.ToString() + ","
					+ "IdealAction,,"
					+ onKeyPositionInfo.KeyPositionType.ToString()
				);
				m_logWriter.Flush();
			}
		}
#endif
	}
	private void	OnScoreAdded(int nearestIndex){
		SongInfo song = m_musicManager.currentSongInfo;
		/*if (song.onKeyPositionSequence[nearestIndex].KeyPositionType == KeyPositionEnum.Jump
			&& temper > temperThreshold)
		{
			/*foreach (GameObject bandMember in m_bandMembers)
			{
				bandMember.GetComponent<BandMember>().Jump();
			}
			foreach (GameObject audience in m_audiences)
			{
				audience.GetComponent<Audience>().Jump();
			}*
			foreach (GameObject noteParticle in m_noteParticles)
			{
				noteParticle.GetComponent<ParticleSystem>().Emit(20);
			}
		}*/
		/*else if (song.onKeyPositionSequence[nearestIndex].KeyPositionType == KeyPositionEnum.HeadBanging)
		{
			/*foreach (GameObject bandMember in m_bandMembers)
			{
				bandMember.GetComponent<SimpleSpriteAnimation>().BeginAnimation(1, 1);
			}
		}*/
	}
	// 
	private void	DebugWriteLogPost(bool hitBefore, bool hitAfter)
	{
#if UNITY_EDITOR
		if(outScoringLog){
			string relation="";
			if(hitBefore){
				relation = "HIT ABOVE";
			}
			if(hitAfter){
				relation = "HIT BELOW";
			}
			string scoreTypeString = "MISS";
			if( m_additionalScore>=excellentScore )
				scoreTypeString = "BEST";
			else if( m_additionalScore>=goodScore )
				scoreTypeString = "GOOD";
			m_logWriter.WriteLine(
				m_keypositionAction.lastActionInfo.triggerBeatTiming.ToString() + ","
				+ " PlayerAction,"
				+ relation + " " + scoreTypeString + ","
				+ m_keypositionAction.lastActionInfo.KeyPositionType.ToString() + ","
				+ "Score=" + m_additionalScore
			);
			m_logWriter.Flush();
		}
#endif
	}

	//Private
	SequenceSeeker<OnKeyPositionInfo> m_scoringUnitSeeker
		= new SequenceSeeker<OnKeyPositionInfo>();
	float			m_additionalScore;
	MusicManager	m_musicManager;
	KeyPositionAction	m_keypositionAction;
	OnPlayGUI		m_onPlayGUI;
	int				m_previousHitIndex = -1;
	//GameObject[]	m_bandMembers;
	//GameObject[]    m_audiences;
	GameObject[]    m_noteParticles;
    TextWriter		m_logWriter;
	PhaseManager m_phaseManager;
	//
	public struct Result {

		public float	timingError;		// 
		public int		markerIndex;		// 
	};

	// 
	public Result	m_lastResult;
}

