using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

	public GameObject audioVolume;

	private AudioSource audioSource;
	private Slider slider;
	private bool mute;

	public void Start () {
		audioSource = GameObject.FindGameObjectsWithTag("Audio")[0].transform.GetComponent<AudioSource> ();
		slider = audioVolume.transform.GetComponent<Slider> ();
		mute = false;
	}

	public void MusicToggle() {
		mute = !mute;
		audioSource.mute = mute;
	}

	public void VolumeChanged () {
		audioSource.volume = slider.value;
	}
}
