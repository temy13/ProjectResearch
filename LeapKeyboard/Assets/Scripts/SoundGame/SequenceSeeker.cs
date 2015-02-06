using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

//
public class SequenceSeeker<ElementType>
	where ElementType: MusicalElement
{	//
	public void SetSequence( List<ElementType> sequence ){
		m_sequence = sequence;
		m_nextIndex=0;
		m_currentBeatCount=0;
		m_isJustPassElement=false;
	}
	//
	public int nextIndex{
			get{return m_nextIndex;}
	}
	//
	public bool isJustPassElement{
			get{return m_isJustPassElement;}
	}

	//
	public void ProceedTime(float deltaBeatCount){

		//
		m_currentBeatCount += deltaBeatCount;
		//
		m_isJustPassElement = false;

		int		index = find_next_element(m_nextIndex);

		//
		if(index!=m_nextIndex){

			//
			m_nextIndex = index;

			//
			m_isJustPassElement=true;
		}
	}
	//
	public void Seek(float beatCount){

		m_currentBeatCount = beatCount;

		int		index = find_next_element(0);

		m_nextIndex = index;
	}

	//
	//
	private int	find_next_element(int start_index)
	{
		//
		int ret = m_sequence.Count;

		for (int i = start_index;i < m_sequence.Count; i++)
		{
			//
			if(m_sequence[i].triggerBeatTiming > m_currentBeatCount)
			{
				ret = i;
				break;
			}
		}

		return(ret);
	}

//private variables
	int		m_nextIndex = 0;				//
	float	m_currentBeatCount = 0;			//
	bool	m_isJustPassElement = false;	//

	List<ElementType> m_sequence;			//
}

