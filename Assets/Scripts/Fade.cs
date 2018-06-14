using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェード処理
/// 完了時コールバックなどは付けてないのでフェードイン・アウト完了が欲しかったら改めて追加する必要あり
/// </summary>
public class Fade : MonoBehaviour
{

    /// <summary>
    /// フェード用板
    /// </summary>
    [SerializeField]
    private Image _plate;

    private Coroutine _nowProc;

    public void FadeIn(float time = 1.0f)
    {
        this.StartFadeProc(1.0f, 0.0f, time);
    }

    public void FadeOut(float time = 1.0f)
    {
        this.StartFadeProc(0.0f, 1.0f, time);
    }

    private void StartFadeProc(float start, float end, float time)
    {
        if (_nowProc != null) { StopCoroutine(_nowProc); }
        _nowProc = StartCoroutine(this.FadeProc(start, end, time));
    }

    private IEnumerator FadeProc(float start, float end, float time)
    {
        float nowTime = 0;
        while (nowTime < time)
        {
            nowTime += Time.deltaTime;
            _plate.color = new Color(0, 0, 0, Mathf.Lerp(start, end, nowTime / time));
            yield return null;
        }
    }
}
