using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public GameObject me;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			MoveMe();	
		}
	}

	void MoveMe(){
		Vector3 nowPositon = this.transform.localPosition;
		if (Input.GetKey(KeyCode.RightArrow)) {
			nowPositon.x--;
		}else if (Input.GetKey (KeyCode.LeftArrow)) {
			nowPositon.x++;
		}else if (Input.GetKey (KeyCode.UpArrow)) {
			nowPositon.z--;
		}else if (Input.GetKey (KeyCode.DownArrow)) {
			nowPositon.z++;
		}
		this.transform.localPosition = nowPositon;
	}
}
