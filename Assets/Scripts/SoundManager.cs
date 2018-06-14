using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClip
{
    TITLE,
    MAIN,
    BRAKE,
    FIRE,
    EXPLOSION,
    LOCKON,
    ATTACK,
}

/// <summary>
/// 簡易サウンド制御
/// </summary>
public class SoundManager : MonoBehaviour
{

    /// <summary>
    /// サウンドリスト
    /// </summary>
    [SerializeField]
    private AudioClip[] _clipList;
    /// <summary>
    /// 同時再生可能数
    /// </summary>
    [SerializeField]
    private int _maxPlayCount;

    private AudioSource[] _sources;

    private static SoundManager _instance;

    /// <summary>
    /// インスタンス取得
    /// </summary>
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// サウンドの再生
    /// </summary>
    /// <param name="clip">再生するサウンドクリップ</param>
    /// <param name="isLoop">ループさせるかどうか</param>
    /// <param name="volume">音量</param>
    /// <returns>ハンドラID</returns>
    public int Play(SoundClip clip, bool isLoop, float volume = 0.5f)
    {
        for (int i = 0; i < this._sources.Length; ++i)
        {
            var source = _sources[i];
            if (source.isPlaying) { continue; }
            source.clip = _clipList[(int)clip];
            source.volume = volume;
            source.spatialBlend = 0.0f;
            source.loop = isLoop;
            source.Play();
            return i;
        }
        return -1;
    }

    /// <summary>
    /// 指定したハンドラが指すサウンドの停止
    /// </summary>
    /// <param name="handler">Play()で返ってきたハンドラID</param>
    public void Stop(int handler)
    {
        _sources[handler].Stop();
    }

    /// <summary>
    /// すべてのサウンドの停止
    /// </summary>
    public void AllStop()
    {
        foreach (var s in _sources)
        {
            s.Stop();
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
        // AudioSourceを先に作っておいて使い回す
        _sources = new AudioSource[_maxPlayCount];
        for (int i = 0; i < _maxPlayCount; ++i)
        {
            _sources[i] = this.gameObject.AddComponent<AudioSource>();
            _sources[i].playOnAwake = false;
            _sources[i].Stop();
        }
    }

}
