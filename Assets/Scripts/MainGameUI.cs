using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインゲームUI
/// </summary>
public class MainGameUI : MonoBehaviour
{

    /// <summary>
    /// ステータス取得対象プレイヤ
    /// </summary>
    [SerializeField]
    private PlayerCtrl _player;
    private MainGameState _gameState;

    /// <summary>
    /// スコア表示テキスト
    /// </summary>
    [SerializeField]
    private Text _score;
    /// <summary>
    /// エネルギー表示テキスト
    /// </summary>
    [SerializeField]
    private Text _energy;
    /// <summary>
    /// 画面フェード
    /// </summary>
    [SerializeField]
    private Fade _fade;

    private int _beforeEnergy;
    private int _beforeScore;

    /// <summary>
    /// 画面のフェードイン ※明るくなる方
    /// </summary>
    public void FadeIn()
    {
        _fade.FadeIn();
    }

    /// <summary>
    /// 画面のフェードアウト ※暗くなる方
    /// </summary>
    public void FadeOut()
    {
        _fade.FadeOut();
    }

    private void Start()
    {
        _gameState = MainGameState.Instance;
    }

    private void Update()
    {
        if (_beforeEnergy != _player.Energy)
        {
            _beforeEnergy = _player.Energy;
            _energy.text = string.Format("{0:D4}", _player.Energy);
        }
        if (_beforeScore != _gameState.Score)
        {
            _beforeScore = _gameState.Score;
            _score.text = string.Format("{0:D7}", _gameState.Score);

        }
    }
}
