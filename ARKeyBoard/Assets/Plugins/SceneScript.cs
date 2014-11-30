using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour {

	public GameObject KeyBoard;
	public GameObject PointSphere;
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
		Application.LoadLevel (SceneN);
	}
}
