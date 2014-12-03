using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour {

	public GameObject KeyBoard;
	public GameObject PointSphere;
	public GameObject[] Modes;
	public string log;

	// Use this for initialization
	void Start () {

	}

	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeScene(int SceneN){
		DontDestroyOnLoad (PointSphere);
		DontDestroyOnLoad(KeyBoard);
		for (int i = 0; i < Modes.Length; i++) {
			DontDestroyOnLoad(Modes[i]);
				}
		Application.LoadLevel (SceneN);
	}
}
