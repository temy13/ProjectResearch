using UnityEngine;
using System.Collections;
using System.IO;

public class SongInfoExporter_CSV {

	// Use this for initialization
	static public void GetOnKeyPositionInfo(SongInfo songInfo,TextWriter writer){
		writer.WriteLine("scoringUnitSequenceRegion-Begin");
		float songLength = songInfo.onKeyPositionSequence[songInfo.onKeyPositionSequence.Count-1].triggerBeatTiming + 1;
		writer.WriteLine("regionParameters,Unified," +
			songLength
			+ "," + songLength);
		foreach(OnKeyPositionInfo onKeyPositionInfo in songInfo.onKeyPositionSequence){
			writer.WriteLine(onKeyPositionInfo.GetCustomParameterAsString_CSV());
		}
		writer.WriteLine("scoringUnitSequenceRegion-End");
	}
}
