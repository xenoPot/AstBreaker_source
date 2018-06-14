using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンをまたいでゲーム状態を得るためのシングルトン
/// </summary>
public class MainGameState : MonoBehaviour
{
    private static MainGameState _instance;

    /// <summary>
    /// インスタンス取得
    /// </summary>
    public static MainGameState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainGameState>();
            }
            return _instance;
        }
    }

    private int _score;
    private const int MaxScore = 9999999;

    /// <summary>
    /// 現在スコア
    /// </summary>
    public int Score
    {
        get
        {
            return _score;
        }
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int score)
    {
        _score = Mathf.Min(MainGameState.MaxScore, _score + score);
    }

    /// <summary>
    /// 状態リセット
    /// </summary>
    public void Reset()
    {
        _score = 0;
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
