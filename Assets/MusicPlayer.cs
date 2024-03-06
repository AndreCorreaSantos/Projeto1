using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update

	public AudioSource musicSource;
	public AudioClip musicStart;
	void Start () 
	{
		musicSource.PlayOneShot(musicStart);
		musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
	}
	

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
