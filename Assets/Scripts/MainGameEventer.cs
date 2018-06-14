using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メインゲーム中のイベント処理
/// </summary>
public class MainGameEventer : MonoBehaviour
{
    /// <summary>
    /// イベントデータ
    /// </summary>
    private class EventData
    {
        private float _startTime;
        private System.Func<bool> _condition;
        private System.Action _action;
        private bool _end;

        /// <summary>
        /// イベント実行可能ならばイベント実行する
        /// </summary>
        /// <param name="nowTime">現在時間</param>
        public void EventPlayIfConditionOk(float nowTime)
        {
            if (!_end && nowTime >= _startTime && (_condition != null && _condition()))
            {
                _action();
                _end = true;
            }
        }

        public EventData(float startTime, System.Func<bool> condition, System.Action action)
        {
            _startTime = startTime;
            _condition = condition;
            _action = action;
        }
    }

    /// <summary>
    /// メインゲームUI
    /// </summary>
    [SerializeField]
    private MainGameUI _ui;
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    private PlayerCtrl _player;
    /// <summary>
    /// メッセージ表示UI
    /// </summary>
    [SerializeField]
    private MessageUI _messageUI;


    private EventData[] _events;
    private float _nowTime;

    private bool _gameEnd;
    private float _waitTime = 5.0f;
    private float _fadeWaitTime = 3.0f;

    private void Start()
    {
        _events = new EventData[] {
            // メインゲーム開始
            new EventData(0.0f, () => true, () => {
                MainGameState.Instance.Reset();
                SoundManager.Instance.Play(SoundClip.MAIN, true);
                _messageUI.Play("作戦開始", 3.0f);
                _ui.FadeIn();
            }),
            // チュートリアル
            new EventData(3.0f, () => true, () => {
                _messageUI.Play("機体エネルギーの続く限り\nこの宙域の隕石群を破壊してください", 4.0f);
            }),
            new EventData(8.0f, () => true, () => {
                _messageUI.Play("左右のキーで方向を操作\n下キーの押し続けで\"チャージ\"を行うことができます", 4.0f);
            }),
            new EventData(13.0f, () => true, () => {
                _messageUI.Play("チャージは\n機体のブースターにエネルギーを貯めるのと同時に", 3.0f);
            }),
            new EventData(17.0f, () => true, () => {
                _messageUI.Play("隕石群をロックオンする事ができます", 3.0f);
            }),
            new EventData(21.0f, () => true, () => {
                _messageUI.Play("下キーを離すとブースターを開放し急加速\n同時に ロックオン対象破壊レーザーの発射が行われます", 6.0f);
            }),
            new EventData(28.0f, () => true, () => {
                _messageUI.Play("また 高い加速力を持った自機を隕石にぶつける事でも\n隕石にダメージを与え破壊することが出来ます", 6.0f);
            }),
            new EventData(35.0f, () => true, () => {
                _messageUI.Play("ただし 加速力が隕石の耐久力を下回った場合\n逆に機体にダメージを受けてしまうので注意してください", 6.0f);
            }),
            new EventData(42.0f, () => true, () => {
                _messageUI.Play("説明は以上です\nご武運を", 4.0f);
            }),

            // 状況メッセージ
            new EventData(0.0f, () => { return _player.Energy <= 7000; }, () => {
                _messageUI.Play("機体エネルギー 残り70%", 3.0f);
            }),
            new EventData(0.0f, () => { return _player.Energy <= 5000; }, () => {
                _messageUI.Play("機体エネルギー 残り50%\n隕石への低速度接触に注意してください", 4.0f);
            }),
            new EventData(0.0f, () => { return _player.Energy <= 3000; }, () => {
                _messageUI.Play("機体エネルギー 残り30%\n作戦終盤です これまで通りに", 4.0f);
            }),
            new EventData(0.0f, () => { return _player.Energy <= 1000; }, () => {
                _messageUI.Play("機体エネルギー 残り10%\nどうぞ 最後の花火を！", 4.0f);
            }),

            // スコアメッセージ
            new EventData(0.0f, () => { return MainGameState.Instance.Score >= 200000; }, () => {
                _messageUI.Play("その調子です", 4.0f);
            }),
            new EventData(0.0f, () => { return MainGameState.Instance.Score >= 300000; }, () => {
                _messageUI.Play("レーザーは発射時に固定量のエネルギー消費を行うので\nなるべくまとめて撃つようにしてください", 7.0f);
            }),
            new EventData(0.0f, () => { return MainGameState.Instance.Score >= 500000; }, () => {
                _messageUI.Play("司令部から なるべく隕石に近づいて破壊してくれ とのことです\n機体が破壊素材を回収するからと・・・", 7.0f);
            }),
            new EventData(0.0f, () => { return MainGameState.Instance.Score >= 750000; }, () => {
                _messageUI.Play("破壊スコアの上昇を確認\n戦果としては十分になってきていますよ！", 4.0f);
            }),
            new EventData(0.0f, () => { return MainGameState.Instance.Score >= 1000000; }, () => {
                _messageUI.Play("破壊スコアの上昇を確認\nこれは・・・作戦終了後の表彰が楽しみですね", 4.0f);
            }),

            // メインゲーム終了～リザルト遷移
            new EventData(0.0f, () => {
                return _player.Energy <= 0;
            }, () => {
                _messageUI.Play("作戦終了\nお疲れ様でした！", 5.0f);
                _gameEnd = true;
            }),
            new EventData(0.0f, () => {
                if( !_gameEnd ){ return false; }
                _waitTime -= Time.deltaTime;
                return _waitTime <= 0.0f;
            }, () => {
                _ui.FadeOut();
            }),
            new EventData(0.0f, () => {
                if( !_gameEnd || _waitTime > 0.0f ){ return false; }
                _fadeWaitTime -= Time.deltaTime;
                return _fadeWaitTime <= 0.0f;
            }, () => {
                SoundManager.Instance.AllStop();
                SceneManager.LoadScene("Result");
            }),

        };

    }

    private void Update()
    {
        _nowTime += Time.deltaTime;
        for (int i = 0; i < _events.Length; ++i)
        {
            _events[i].EventPlayIfConditionOk(_nowTime);
        }
    }
}
