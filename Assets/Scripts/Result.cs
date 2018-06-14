using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルト画面制御
/// </summary>
public class Result : MonoBehaviour
{
    /// <summary>
    /// スコア表示
    /// </summary>
    [SerializeField]
    private Text _scoreLabel;
    /// <summary>
    /// ツイート用ボタン
    /// </summary>
    [SerializeField]
    private Button _tweetButton;
    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    [SerializeField]
    private Button _returnButton;

    private void Start()
    {
        _scoreLabel.text = "Score: " + MainGameState.Instance.Score;
        _tweetButton.onClick.AddListener(() =>
        {
            var score = MainGameState.Instance.Score;
            // スクリプトをリポジトリに入れてないので一旦コメントアウト中
            // naichilab.UnityRoomTweet.Tweet("ast_breaker", "今回の隕石破壊スコア: " + score, "unityroom", "unity1week");
        });
        _returnButton.onClick.AddListener(() =>
        {
            MainGameState.Instance.Reset();
            SceneManager.LoadScene("Title");
        });
    }
}
