using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {
	public AudioSource efxSource;
	public AudioSource musicSource;
	public AudioClip[] foodMusic;
	public AudioClip[] sodaMusic;
	public AudioClip stepsMusic;

	private float minPitch = 0.95f;
	private float maxPitch = 1.05f;

	public static SoundManager instance = null;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	
	void play(AudioClip music) {
		efxSource.clip = music;
		efxSource.Play();
	}
	void playRandom(params AudioClip[] clips) {
		int clipNumber = Random.Range(0, clips.Length);
		float pitch = Random.Range(minPitch, maxPitch);
		efxSource.pitch = pitch;
		play(clips[clipNumber]);
	}

}
