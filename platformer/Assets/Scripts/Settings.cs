using UnityEngine;
using System.Collections;

public class Settings{
	private AudioSource m_MusicSource;
	private AudioSource m_SoundSource;

	public float soundVolume {
		get { return m_SoundSource.volume;}
		set { m_SoundSource.volume = value;}
	}
	public float musicVolume {
		get { return m_MusicSource.volume;}
		set { m_MusicSource.volume = value;}
	}

	public int HighScore{get;set;}

	public void Load( AudioSource music, AudioSource sound){
		m_MusicSource = music;
		m_SoundSource = sound;

		soundVolume = PlayerPrefs.GetFloat("soundVolume", 1.0f);
		musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
		HighScore = PlayerPrefs.GetInt("HighScore", 0);
	}

	public void Save(){
		PlayerPrefs.SetFloat("soundVolume", soundVolume);
		PlayerPrefs.SetFloat("musicVolume", musicVolume);
		PlayerPrefs.SetInt ("HighScore", HighScore);
	}

}
