using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー処理
/// </summary>
public class Laser : MonoBehaviour
{

    /// <summary>
    /// 着弾予定ターゲット
    /// </summary>
    [SerializeField]
    private GameObject _target;
    /// <summary>
    /// 速度算出用力
    /// </summary>
    [SerializeField]
    private float _force;
    /// <summary>
    /// 追跡加速度
    /// </summary>
    [SerializeField]
    private float _trackingVelocity;
    /// <summary>
    /// 接触時ダメージ
    /// </summary>
    [SerializeField]
    private float _damage;
    /// <summary>
    /// 生存時間
    /// </summary>
    [SerializeField]
    private float _lifeTime;
    /// <summary>
    /// 着弾時エフェクト
    /// </summary>
    [SerializeField]
    private GameObject _hitEffect;

    /// <summary>
    /// 着弾予定ターゲット
    /// </summary>
    public GameObject Target
    {
        set
        {
            _target = value;
        }
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            return;
        }
        if (_target == null)
        {
            // ライフタイムが尽きるまで前進するだけ
            transform.position += transform.TransformDirection(Vector3.forward) * _force * Time.deltaTime;
            return;
        }

        // ターゲットまでの角度を取得
        Vector3 vecTarget = _target.transform.position - this.transform.position; // ターゲットへのベクトル
        Vector3 vecForward = this.transform.TransformDirection(Vector3.forward); // 弾の正面ベクトル
        float angleDiff = Vector3.Angle(vecForward, vecTarget); // ターゲットまでの角度
        float angleAdd = (_trackingVelocity * Time.deltaTime); // 回転角
        Quaternion rotTarget = Quaternion.LookRotation(vecTarget); // ターゲットへ向けるクォータニオン
        if (angleDiff <= angleAdd)
        {
            // ターゲットが回転角以内なら完全にターゲットの方を向く
            transform.rotation = rotTarget;
        }
        else
        {
            // ターゲットが回転角の外なら、指定角度だけターゲットに向ける
            float t = (angleAdd / angleDiff);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTarget, t);
        }
        transform.position += transform.TransformDirection(Vector3.forward) * _force * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this._lifeTime <= 0) { return; } // レーザーエフェクトが再生完了するまでオブジェクト消失しないため、条件を満たしていたら何もしないようにしないと隕石と接触してダメージが取れてしまう可能性がある
        if (other.gameObject.tag == TagList.Asteroid)
        {
            this._lifeTime = 0;
            other.GetComponent<Asteroid>().AddDamage(_damage);
            Instantiate(_hitEffect, this.transform.position, Quaternion.identity);
        }
    }
}
