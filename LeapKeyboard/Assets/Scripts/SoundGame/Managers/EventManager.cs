using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//event
public class EventManager : MonoBehaviour {
	// Use this for initialization
	void Start(){
		m_musicManager=GameObject.Find("MusicManager").GetComponent<MusicManager>();
	}
	public void BeginEventSequence(){
		m_seekUnit.SetSequence(m_musicManager.currentSongInfo.stagingDirectionSequence);
	}
	public void Seek(float beatCount){
		m_seekUnit.Seek( beatCount );
		m_previousIndex=m_seekUnit.nextIndex;
		for ( LinkedListNode<StagingDirection> it = m_activeEvents.First; it != null; it = it.Next) {
			it.Value.OnEnd();
			m_activeEvents.Remove(it);
		}
	}
	void Update () {
		
		SongInfo	song = m_musicManager.currentSongInfo;
		
		if( m_musicManager.IsPlaying() )
		{
			//
			
			m_previousIndex = m_seekUnit.nextIndex;
			
			m_seekUnit.ProceedTime(m_musicManager.beatCount - m_musicManager.previousBeatCount);
			
			// 
			for(int i = m_previousIndex;i < m_seekUnit.nextIndex;i++){
				
				// 
				StagingDirection clone = song.stagingDirectionSequence[i].GetClone() as StagingDirection;
				
				clone.OnBegin();
				
				// 
				m_activeEvents.AddLast(clone);
			}
		}
		
		// 
		for ( LinkedListNode<StagingDirection> it = m_activeEvents.First; it != null; it = it.Next) {
			
			StagingDirection	activeEvent = it.Value;
			
			activeEvent.Update();
			
			// 
			if(activeEvent.IsFinished()) {
				
				activeEvent.OnEnd();
				
				// 
				m_activeEvents.Remove(it);
			}
		}
	}
	
	//private variables
	
	MusicManager m_musicManager;
	
	// 
	SequenceSeeker<StagingDirection> m_seekUnit
		= new SequenceSeeker<StagingDirection>();
	
	// 
	LinkedList<StagingDirection> m_activeEvents
		= new LinkedList<StagingDirection>();
	
	int		m_previousIndex=0;			// 
}

