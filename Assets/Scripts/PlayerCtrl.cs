using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー操作
/// </summary>
public class PlayerCtrl : MonoBehaviour
{
    /// <summary>
    /// チャージ時のパワー増加量
    /// </summary>
    [SerializeField]
    private float _powerRatio = 1.0f;

    /// <summary>
    /// 左右入力時の回転力
    /// </summary>
    [SerializeField]
    private float _rotPower = 1.0f;

    /// <summary>
    /// 現在パワー
    /// </summary>
    [SerializeField]
    private float _nowPower = 0;
    /// <summary>
    /// 最大パワー
    /// </summary>
    [SerializeField]
    private float _maxPower = 0;

    /// <summary>
    /// 残エネルギー
    /// </summary>
    [SerializeField]
    private int _energy = 9999;
    /// <summary>
    /// ブレーキ（チャージ）中エフェクト
    /// </summary>
    [SerializeField]
    private GameObject _brakeAura;
    /// <summary>
    /// ブレーキ解除時エフェクト
    /// </summary>
    [SerializeField]
    private GameObject _burstObject;
    /// <summary>
    /// 被ダメージ時エフェクト
    /// </summary>
    [SerializeField]
    private GameObject _damageEffect;

    private bool _inputBrake = false;
    private bool _inputLeft = false;
    private bool _inputRight = false;

    private Rigidbody _rigidBody;
    private Transform _transform;

    private int _brakeSoundHandler = -1;

    private bool _controlEnd;

    private float _chargeDamageTime = 0.032f;
    private float _nowChargeDamageTime = 0;

    private float _unChargeDamageTime = 0.2f;
    private float _nowUnChargeDamageTime = 0;

    /// <summary>
    /// 衝突威力
    /// </summary>
    public float NowAttackPower
    {
        get
        {
            return _rigidBody.velocity.magnitude * 0.7f;
        }
    }

    /// <summary>
    /// 残エネルギー
    /// </summary>
    public int Energy
    {
        get
        {
            return _energy;
        }
    }

    /// <summary>
    /// 自機に指定値分のダメージを加える
    /// </summary>
    public void AddDamage(int damage)
    {
        _energy = Mathf.Max(0, _energy - damage);
    }

    /// <summary>
    /// 自機の被ダメージエフェクト再生
    /// </summary>
    public void PlayDamageEffect()
    {
        var obj = Instantiate(_damageEffect, _transform.position, Quaternion.identity);
        obj.transform.SetParent(this.transform);
        SoundManager.Instance.Play(SoundClip.ATTACK, false);
    }

    private void Start()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _transform = this.transform;
    }

    private void Update()
    {
        if (_controlEnd) { return; }
        _inputBrake = Input.GetKey(KeyCode.DownArrow);
        _inputLeft = Input.GetKey(KeyCode.LeftArrow);
        _inputRight = Input.GetKey(KeyCode.RightArrow);
        if (_inputBrake)
        {
            // ブレーキ処理
            if (_brakeSoundHandler == -1)
            {
                _brakeSoundHandler = SoundManager.Instance.Play(SoundClip.BRAKE, false);
            }
            _brakeAura.SetActive(true);
            _nowPower = Mathf.Min(_maxPower, _nowPower + _powerRatio * Time.deltaTime);
            _nowChargeDamageTime += Time.deltaTime;
            while (_nowChargeDamageTime >= _chargeDamageTime)
            {
                this.AddDamage(1);
                _nowChargeDamageTime = _nowChargeDamageTime - _chargeDamageTime;
            }
        }
        else
        {
            // 未ブレーキ処理
            if (_brakeSoundHandler != -1)
            {
                SoundManager.Instance.Stop(_brakeSoundHandler);
                _brakeSoundHandler = -1;
            }
            _brakeAura.SetActive(false);
            _nowUnChargeDamageTime += Time.deltaTime;
            while (_nowUnChargeDamageTime >= _unChargeDamageTime)
            {
                this.AddDamage(1);
                _nowUnChargeDamageTime = _nowUnChargeDamageTime - _unChargeDamageTime;
            }
        }

        if (_energy <= 0)
        {
            _controlEnd = true;
        }
    }

    private void FixedUpdate()
    {
        if (_inputBrake)
        {
            _rigidBody.velocity = _rigidBody.velocity * 0.96f;
        }
        else if (_nowPower > 0)
        {
            // 加速
            _rigidBody.AddForce(_transform.forward * _nowPower, ForceMode.Acceleration);
            var obj = Instantiate(_burstObject, _transform.position, Quaternion.identity);
            obj.transform.SetParent(_transform);
            SoundManager.Instance.Play(SoundClip.FIRE, false);

            _nowPower = 0.0f;
        }
        if (_inputRight)
        {
            _rigidBody.AddTorque(0, _rotPower, 0, ForceMode.Acceleration);
            var res = Quaternion.Euler(0, 1.0f, 0) * _rigidBody.velocity;
            _rigidBody.velocity = res;
        }
        if (_inputLeft)
        {
            _rigidBody.AddTorque(0, -_rotPower, 0, ForceMode.Acceleration);
            var res = Quaternion.Euler(0, -1.0f, 0) * _rigidBody.velocity;
            _rigidBody.velocity = res;
        }
        _rigidBody.angularVelocity *= 0.9f;
        _rigidBody.velocity *= 0.999f;
    }
}
