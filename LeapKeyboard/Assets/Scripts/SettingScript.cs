using UnityEngine;
using System.Collections;

public class SettingScript : MonoBehaviour {

	public GameObject pointObject;
	public GameObject AllKeyMode;
	public GameObject KeyBoard;
	HandBehaviorScript hbs;


	// Use this for initialization
	void Start () {
		hbs = KeyBoard.GetComponent<HandBehaviorScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (pointObject.activeSelf && Vector3.Distance (pointObject.transform.position, AllKeyMode.transform.position) < 2.0)
						ChangeAllKeyMode ();
	}

	void ChangeAllKeyMode(){
		if (hbs.allmode) {
						hbs.allmode = false;
						AllKeyMode.renderer.material.color = Color.white;
				} else {
						hbs.allmode = true;
						AllKeyMode.renderer.material.color = Color.blue;
				}

	}
}
