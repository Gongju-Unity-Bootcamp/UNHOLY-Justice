using System;
using System.Collections;
using UnityEngine;

public enum SoundType
{
    Bgm,
    Effect,
    Max
}

public class SoundManager
{
    private static SoundManager s_instance;
    public static SoundManager Instance => s_instance ??= s_instance = new SoundManager();
            
    AudioSource[] _audioSources;
    GameObject _soundRoot;

    private SoundManager()
    {
        Init();
    }

    public void Init()
    {
        _soundRoot = GameObject.Find("@SoundRoot");
        if (_soundRoot == null)
        {
            _soundRoot = new GameObject { name = "@SoundRoot" };
            UnityEngine.Object.DontDestroyOnLoad(_soundRoot);

            string[] soundTypeNames = Enum.GetNames(typeof(SoundType));
            _audioSources = new AudioSource[(int)SoundType.Max];
            for (int i = 0; i < _audioSources.Length; ++i)
            {
                GameObject go = new GameObject(soundTypeNames[i]);
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = _soundRoot.transform;
            }
        }

        _audioSources[(int)SoundType.Bgm].loop = true;
    }

    public void Play(SoundType soundType, string path)
    {
        // TODO : 사운드 경로 설정 
        AudioClip clip = Resources.Load<AudioClip>(path);
        AudioSource source = _audioSources[(int)soundType];

        switch (soundType)
        {
            case SoundType.Bgm:
                if (source.isPlaying)
                {
                    source.Stop();
                }
                source.clip = clip;
                source.Play();
                break;
            case SoundType.Effect:
                source.PlayOneShot(clip);
                break;
        }
    }
}