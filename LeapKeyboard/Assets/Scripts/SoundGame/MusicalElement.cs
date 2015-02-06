using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

//
public abstract class MusicalElement {
	//
	public float triggerBeatTiming = 0;
	//
	public virtual void ReadCustomParameterFromString(string[] parameters){}
	//
	public virtual MusicalElement GetClone(){
		MusicalElement clone = this.MemberwiseClone() as MusicalElement;
		return clone;
	}
	public System.Xml.Schema.XmlSchema GetSchema(){return null;}
};
//
public class SequenceRegion: MusicalElement{
	public float totalBeatCount;
	public string name;
	public float repeatPosition;
};

//
public class OnKeyPositionInfo : MusicalElement {
	public KeyPositionEnum KeyPositionType;//
	public bool isWhiteKey;
	public string GetCustomParameterAsString_CSV(){
		//return "SingleShot," + triggerBeatTiming.ToString() + "," + KeyPositionType.ToString();
		return "SingleShot," + triggerBeatTiming.ToString() + "," + KeyPositionType.ToString()+ "," + isWhiteKey.ToString();
	}

	public int	line_number;		//
}
