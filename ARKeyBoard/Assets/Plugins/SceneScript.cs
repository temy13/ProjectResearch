using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour {

	//public GameObject KeyBoard;
	public string log;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		//Application.LoadLevel("KeyBoardWithScore");
		log = "test";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
