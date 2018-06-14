using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隕石
/// </summary>
public class Asteroid : MonoBehaviour
{
    /// <summary>
    /// 耐久力
    /// </summary>
    [SerializeField]
    private float _durability;
    /// <summary>
    /// 接触時のプレイヤーへのダメージ
    /// </summary>
    [SerializeField]
    private int _damage;
    /// <summary>
    /// 破壊時の獲得スコア
    /// </summary>
    [SerializeField]
    private int _score;

    /// <summary>
    /// 消滅時エフェクト
    /// </summary>
    [SerializeField]
    private GameObject _destroyParticle;

    private System.Action<Asteroid> _onDestroyAsteroid;
    private PlayerCtrl _player;

    /// <summary>
    /// 消滅時コールバック
    /// </summary>
    public System.Action<Asteroid> OnDestroyAsteroid
    {
        set
        {
            _onDestroyAsteroid = value;
        }
    }

    /// <summary>
    /// プレイヤー
    /// </summary>
    public PlayerCtrl Player
    {
        set
        {
            _player = value;
        }
    }

    /// <summary>
    /// 隕石にダメージを加える
    /// </summary>
    /// <param name="damage">加えるダメージ量</param>
    /// <returns>破壊されたかどうか</returns>
    public bool AddDamage(float damage)
    {
        if (_durability <= 0) { return true; }
        _durability -= damage;
        if (_durability <= 0)
        {
            // 破壊時のプレイヤーとの距離に応じたスコアボーナス算出
            var targetRange = Mathf.Min(2000.0f, Vector3.Distance(this.transform.position, _player.transform.position) * 0.8f);
            MainGameState.Instance.AddScore((int)(_score * ((2500.0f - targetRange) / 1000.0f)));
            _onDestroyAsteroid(this);
            Instantiate(_destroyParticle, this.transform.position, Quaternion.identity);
            SoundManager.Instance.Play(SoundClip.EXPLOSION, false, 0.25f);
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == TagList.Player)
        {
            var isDestroy = this.AddDamage(_player.NowAttackPower);
            if (!isDestroy)
            {
                _player.AddDamage(_damage);
            }
            _player.PlayDamageEffect();
        }
    }
}
