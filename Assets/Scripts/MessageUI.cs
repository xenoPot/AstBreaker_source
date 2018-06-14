using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メッセージ表示UI
/// </summary>
public class MessageUI : MonoBehaviour
{

    /// <summary>
    /// 下地
    /// </summary>
    [SerializeField]
    private Image _window;
    /// <summary>
    /// 表示文章
    /// </summary>
    [SerializeField]
    private Text _text;

    private float _playTime;
    private float _nowTime;
    private bool _playMode;

    /// <summary>
    /// メッセージ再生
    /// </summary>
    /// <param name="text">再生する文章</param>
    /// <param name="time">再生時間</param>
    public void Play(string text, float time)
    {
        SoundManager.Instance.Play(SoundClip.LOCKON, false);
        _window.gameObject.SetActive(true);
        _text.gameObject.SetActive(true);
        _text.text = text;
        _nowTime = 0;
        _playTime = time;
        _playMode = true;
    }

    private void Update()
    {
        if (!_playMode) { return; }
        _nowTime += Time.deltaTime;
        if (_nowTime >= _playTime)
        {
            _window.gameObject.SetActive(false);
            _text.gameObject.SetActive(false);
            _playMode = true;
        }
    }
}
