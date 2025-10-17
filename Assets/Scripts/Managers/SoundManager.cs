using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : SceneSingletonMonoBehaviour<SoundManager>
{ 
    public AudioSource bgm;
    public GameObject sfx;
    private Dictionary<string,AudioClip> soundDict = new();
    
    private float soundVolume;
    public float SoundVolume 
    { 
        get => soundVolume;
        set
        {
            soundVolume = Mathf.Clamp(value, 0f, 100f);
            ApplyVolume();
        }
    }
    
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("SFX");
        foreach (var clip in clips)
        {
            soundDict.Add(clip.name, clip);
        }
        
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 50f);
        }
        SoundVolume = PlayerPrefs.GetFloat("Volume");
    }

    private void ApplyVolume()
    {
        float normalizedVolume = soundVolume / 100f; 
        
        if (bgm != null)
        {
            bgm.volume = normalizedVolume;
        }
        
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (var source in allSources)
        {
            if (source != bgm)
            {
                source.volume = normalizedVolume;
            }
        }
    }

    public void OnSaveSounds()
    {
        PlayerPrefs.SetFloat("Volume", SoundVolume);
    }
    
    public void PlaySFX(string key)
    {
        AudioSource source = GetAvailableSource();
        source.clip = soundDict[key];
        source.volume = soundVolume / 100f;
        source.Play();
    }

    private AudioSource GetAvailableSource()
    {
        return ObjectPoolManager.Instance.Get(sfx, gameObject.transform.position, new Vector3(0, 0, 0)).GetComponent<AudioSource>();
    }
    
    public void PlayBGM(string key)
    {
        StartCoroutine(FadeToNewBGM(key));
    }

    private IEnumerator FadeToNewBGM(string key)
    {
        yield return null;
        if (soundDict.ContainsKey(key))
        {
            bgm.clip = soundDict[key];
            bgm.volume = soundVolume / 100f; 
            bgm.Play();
        }
    }
    
}