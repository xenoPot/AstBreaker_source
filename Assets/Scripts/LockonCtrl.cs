using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロックオン～レーザー射出処理
/// </summary>
public class LockonCtrl : MonoBehaviour
{
    /// <summary>
    /// 隕石ファクトリ
    /// </summary>
    [SerializeField]
    private AsteroidFactory _astFactory;
    /// <summary>
    /// 1ロックオン完了に必要な時間
    /// </summary>
    [SerializeField]
    private float _lockonTime;
    private float _nowLockonTime;
    /// <summary>
    /// 射出するレーザーオブジェクト
    /// </summary>
    [SerializeField]
    private Laser _laser;
    /// <summary>
    /// プレイヤー
    /// </summary>
    [SerializeField]
    private PlayerCtrl _player;
    /// <summary>
    /// レーザーの最大合計存在可能数
    /// </summary>
    [SerializeField]
    private int _maxBullet = 100;
    private bool _isCharge;

    private HashSet<Asteroid> _targets;

    /// <summary>
    /// ロックオン中の隕石一覧
    /// </summary>
    public HashSet<Asteroid> Targets
    {
        get
        {
            return _targets;
        }
    }

    /// <summary>
    /// レーザーの最大合計存在可能数
    /// </summary>
    public int MaxBullet
    {
        get
        {
            return _maxBullet;
        }
    }

    private void Start()
    {
        _targets = new HashSet<Asteroid>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_player.Energy <= 0) { return; }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _isCharge = true;
            var asts = _astFactory.FieldAsteroids;
            var count = 0;
            _nowLockonTime = Mathf.Max(0, _nowLockonTime - Time.deltaTime);
            // TODO: かなり処理負荷がキツい箇所になっているのでもっと数を出したい場合は見直しが必要
            foreach (var ast in asts)
            {
                if (!(this.InCamera(ast.gameObject) && count < _maxBullet))
                {
                    _targets.Remove(ast);
                    continue;
                }
                count++;
                if (_targets.Contains(ast)) { continue; }
                if (_nowLockonTime == 0)
                {
                    _targets.Add(ast);
                    SoundManager.Instance.Play(SoundClip.LOCKON, false);
                    _nowLockonTime = _lockonTime;
                }
            }
            return;
        }

        // レーザー発射判定～発射
        if (!_isCharge) { return; }
        _player.AddDamage(100);
        foreach (var ast in _targets)
        {
            if (ast == null) { continue; }
            _player.AddDamage(1);
            var laser = Instantiate(
                _laser,
                _player.transform.position,
                _player.transform.rotation * Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)));
            laser.Target = ast.gameObject;
        }
        _targets.Clear();
        _isCharge = false;
    }

    private bool InCamera(GameObject target)
    {
        Camera m = Camera.main;
        Matrix4x4 V = m.worldToCameraMatrix;
        Matrix4x4 P = m.projectionMatrix;
        Matrix4x4 VP = P * V;

        var p = target.transform.position;
        Vector4 pos = VP * new Vector4(p.x, p.y, p.z, 1.0f);

        if (pos.w == 0)
        {
            return true;
        }

        float x = pos.x / pos.w;
        float y = pos.y / pos.w;
        float z = pos.z / pos.w;

        if (x < -1.0f)
        {
            return false;
        }
        if (x > 1.0f)
        {
            return false;
        }

        if (y < -1.0f)
        {
            return false;
        }
        if (y > 1.0f)
        {
            return false;
        }

        if (z < -1.0f)
        {
            return false;
        }

        if (z > 1.0f)
        {
            return false;
        }

        return true;
    }
}
