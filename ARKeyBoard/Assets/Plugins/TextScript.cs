using UnityEngine;
using System.Collections;

public class TextScript : MonoBehaviour {

	string display_text;
	public GameObject ScoreBoard;
	ScoreBoardScript sc;
	// Use this for initialization
	void Start () {
		sc = ScoreBoard.GetComponent<ScoreBoardScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			InputTextCheck();
			this.guiText.text = display_text;			
		}
	}
	void InputTextCheck(){

		if (Input.GetKeyDown (KeyCode.KeypadEnter))
						sc.SetScoreFromPDF (display_text);
				else if (Input.GetKeyDown (KeyCode.Backspace))
						;
		else if(Input.GetKeyDown(KeyCode.Slash))
		        display_text += "/";
		else if (Input.GetKeyDown (KeyCode.Period))
			display_text += ".";
		else if (Input.GetKeyDown (KeyCode.Space))
			display_text += " ";
		else if (Input.GetKeyDown (KeyCode.Underscore))
			display_text += "_";
		else if (Input.GetKeyDown (KeyCode.Alpha1))
			display_text += "1";
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			display_text += "2";
		else if (Input.GetKeyDown (KeyCode.Alpha3))
			display_text += "3";
		else if (Input.GetKeyDown (KeyCode.Alpha4))
			display_text += "4";
		else if (Input.GetKeyDown (KeyCode.Alpha5))
			display_text += "5";
		else if (Input.GetKeyDown (KeyCode.Alpha6))
			display_text += "6";
		else if (Input.GetKeyDown (KeyCode.Alpha7))
			display_text += "7";
		else if (Input.GetKeyDown (KeyCode.Alpha8))
			display_text += "8";
		else if (Input.GetKeyDown (KeyCode.Alpha8))
			display_text += "9";
		else if (Input.GetKeyDown (KeyCode.Alpha0))
			display_text += "0";
		else if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))) {
						GetLargeChara ();
				} else {
			GetSmallChara();
				}

	}

	void GetLargeChara(){
		if (Input.GetKeyDown ("a"))
			display_text += "A";
		else if (Input.GetKeyDown ("b"))
			display_text += "B";
		else if (Input.GetKeyDown ("c"))
			display_text += "C";
		else if (Input.GetKeyDown ("d"))
			display_text += "D";
		else if (Input.GetKeyDown ("e"))
			display_text += "E";
		else if (Input.GetKeyDown ("f"))
			display_text += "F";
		else if (Input.GetKeyDown ("g"))
			display_text += "G";
		else if (Input.GetKeyDown ("h"))
			display_text += "H";
		else if (Input.GetKeyDown ("i"))
			display_text += "I";
		else if (Input.GetKeyDown ("j"))
			display_text += "J";
		else if (Input.GetKeyDown ("k"))
			display_text += "K";
		else if (Input.GetKeyDown ("l"))
			display_text += "L";
		else if (Input.GetKeyDown ("m"))
			display_text += "M";
		else if (Input.GetKeyDown ("n"))
			display_text += "N";
		else if (Input.GetKeyDown ("o"))
			display_text += "O";
		else if (Input.GetKeyDown ("p"))
			display_text += "P";
		else if (Input.GetKeyDown ("q"))
			display_text += "Q";
		else if (Input.GetKeyDown ("r"))
			display_text += "R";
		else if (Input.GetKeyDown ("s"))
			display_text += "S";
		else if (Input.GetKeyDown ("t"))
			display_text += "T";
		else if (Input.GetKeyDown ("u"))
			display_text += "U";
		else if (Input.GetKeyDown ("v"))
			display_text += "V";
		else if (Input.GetKeyDown ("w"))
			display_text += "W";
		else if (Input.GetKeyDown ("X"))
			display_text += "X";
		else if (Input.GetKeyDown ("y"))
			display_text += "Y";
		else if (Input.GetKeyDown ("z"))
			display_text += "Z";
	}
	void GetSmallChara(){
		if (Input.GetKeyDown ("a"))
			display_text += "a";
		else if (Input.GetKeyDown ("b"))
			display_text += "b";
		else if (Input.GetKeyDown ("c"))
			display_text += "c";
		else if (Input.GetKeyDown ("d"))
			display_text += "d";
		else if (Input.GetKeyDown ("e"))
			display_text += "e";
		else if (Input.GetKeyDown ("f"))
			display_text += "f";
		else if (Input.GetKeyDown ("g"))
			display_text += "g";
		else if (Input.GetKeyDown ("h"))
			display_text += "h";
		else if (Input.GetKeyDown ("i"))
			display_text += "i";
		else if (Input.GetKeyDown ("j"))
			display_text += "j";
		else if (Input.GetKeyDown ("k"))
			display_text += "k";
		else if (Input.GetKeyDown ("l"))
			display_text += "l";
		else if (Input.GetKeyDown ("m"))
			display_text += "m";
		else if (Input.GetKeyDown ("n"))
			display_text += "n";
		else if (Input.GetKeyDown ("o"))
			display_text += "o";
		else if (Input.GetKeyDown ("p"))
			display_text += "p";
		else if (Input.GetKeyDown ("q"))
			display_text += "q";
		else if (Input.GetKeyDown ("r"))
			display_text += "r";
		else if (Input.GetKeyDown ("s"))
			display_text += "s";
		else if (Input.GetKeyDown ("t"))
			display_text += "t";
		else if (Input.GetKeyDown ("u"))
			display_text += "u";
		else if (Input.GetKeyDown ("v"))
			display_text += "v";
		else if (Input.GetKeyDown ("w"))
			display_text += "w";
		else if (Input.GetKeyDown ("X"))
			display_text += "x";
		else if (Input.GetKeyDown ("y"))
			display_text += "y";
		else if (Input.GetKeyDown ("z"))
			display_text += "z";
	}
}
