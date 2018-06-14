using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤの速度に応じて距離が変化するカメラ
/// </summary>
public class SpeedCamera : MonoBehaviour
{

    /// <summary>
    /// プレイヤ
    /// </summary>
    [SerializeField]
    private Rigidbody _player;
    /// <summary>
    /// 実際の注視対象オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject _lookAt;
    /// <summary>
    /// カメラ最近距離位置指定用オブジェクト
    /// </summary>
    [SerializeField]
    private Transform _minDistanceObj;
    /// <summary>
    /// カメラ最遠位置指定用オブジェクト
    /// </summary>
    [SerializeField]
    private Transform _maxDistanceObj;
    /// <summary>
    /// カメラの最大加速度
    /// </summary>
    [SerializeField]
    private float _maxVelocityLength;

    private float _nowT;
    private float _nowVel;

    private void FixedUpdate()
    {
        var t = Mathf.Min(1.0f, _player.velocity.magnitude / _maxVelocityLength);
        _nowT = Mathf.SmoothDamp(_nowT, t, ref _nowVel, 0.1f, 100f);
        this.transform.position = Vector3.Lerp(_minDistanceObj.position, _maxDistanceObj.position, _nowT);
        this.transform.LookAt(_lookAt.transform);
    }
}
