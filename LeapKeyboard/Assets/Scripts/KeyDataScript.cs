using UnityEngine;
using System.Collections;
using Leap;

public class KeyDataScript : MonoBehaviour {

	AudioSource keySound;

	private int onFingerNumber;
	public int OnFingerNumber{
		get{
			return onFingerNumber;
		}
		set{
			onFingerNumber = value;
		}
	}

	public bool CompareToFingerNumber(int number){
		return onFingerNumber == number;
	}

	// Use this for initialization
	void Start () {
		OnFingerNumber= -1;
		if (this.GetComponent<AudioSource> ()) {
						keySound = this.GetComponent<AudioSource> ();
				}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlaySound(int finN)
	{
		this.OnFingerNumber = finN;
		if (this.GetComponent<AudioSource> ()) {
						//keySound.loop = true;
						keySound.Play ();
				}
	}
	public void StopSound()
	{
		this.OnFingerNumber = -1;
		if (this.GetComponent<AudioSource> ()) {
						keySound.Stop ();
						//keySound.loop = false;
				}
	}
}
