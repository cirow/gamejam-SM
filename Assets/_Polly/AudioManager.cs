﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance = null;
	[HideInInspector]
	public AudioSource backgroundSource;

    [SerializeField]
    private AudioSource sourceSFX;
    [SerializeField]
    private AudioClip disableSFX;
	[SerializeField]
	private AudioClip jumpSFX;
	[SerializeField]
	private AudioClip toastSFX;
    [SerializeField]
    private AudioClip xplosionSFX;
    [SerializeField]
    private AudioClip turret;

    // Use this for initialization
    private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			//Leave here just in case we need after
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
	// Use this for initialization
	void Start()
	{
		backgroundSource = gameObject.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

    public void PlayDisable()
    {
        sourceSFX.PlayOneShot(disableSFX);
    }

	public void PlayJump()
	{
		sourceSFX.PlayOneShot(jumpSFX);
	}

	public void PlayToast()
	{
		sourceSFX.PlayOneShot(toastSFX);
	}

    public void PlayXplosion()
    {
		sourceSFX.PlayOneShot(xplosionSFX);
    }

    public void PlayTurret()
    {
        sourceSFX.PlayOneShot(turret);
    }

    public void PlayBackgroundAudio(bool play)
	{
		if (play)
		{
			if (backgroundSource.isPlaying)
			{
				backgroundSource.Stop();
				backgroundSource.Play();
				backgroundSource.volume = 0.5f;
			}
			else
			{

				backgroundSource.Play();
			}
		}
		else
		{
			if (backgroundSource.clip == null)
			{
				//do nothing
			}
			else
			{
				backgroundSource.Stop();
			}
		}
	}
}
