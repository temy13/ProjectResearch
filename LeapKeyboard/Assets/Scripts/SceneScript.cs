using UnityEngine;
using System.Collections;

public class SceneScript : MonoBehaviour {

	public GameObject KeyBoard;
	public GameObject PointSphere;
	public GameObject GameSettings;
	public GameObject SettingSwitch;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (PointSphere);
		DontDestroyOnLoad(KeyBoard);
		DontDestroyOnLoad (GameSettings);
		//Application.LoadLevel (2);test
	}	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeScene(int SceneN){
		DontDestroyOnLoad (PointSphere);
		DontDestroyOnLoad(KeyBoard);
		DontDestroyOnLoad (GameSettings);
		DontDestroyOnLoad (SettingSwitch);
		Vector3 keyboard_v = KeyBoard.transform.position;
		if (SceneN == 2) {
			KeyBoard.transform.position = new Vector3 (keyboard_v.x, keyboard_v.y, -7);
				} else {
						KeyBoard.transform.position = new Vector3 (keyboard_v.x, keyboard_v.y, -3);
				}
		Application.LoadLevel (SceneN);
	}
}
