using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
public class SongInfo{
	public List<OnKeyPositionInfo> onKeyPositionSequence = new List<OnKeyPositionInfo>();
	public List<StagingDirection> stagingDirectionSequence = new List<StagingDirection>();
	public List<SequenceRegion> onKeyPositionRegionSequence = new List<SequenceRegion>();
	public float beatPerSecond=120.0f/60.0f;
	public float beatPerBar=4.0f;
}

