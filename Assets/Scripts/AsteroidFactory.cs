using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隕石データ
/// </summary>
[System.Serializable]
public class AsteroidData
{
    public Asteroid baseObj;
    public int weight;
}

/// <summary>
/// 隕石生成ファクトリ
/// </summary>
public class AsteroidFactory : MonoBehaviour
{
    /// <summary>
    /// 隕石リスト
    /// </summary>
    [SerializeField]
    private AsteroidData[] _asteroids;

    /// <summary>
    /// 生成位置計算用プレイヤ
    /// </summary>
    [SerializeField]
    private PlayerCtrl _player;
    /// <summary>
    /// 生成間隔
    /// </summary>
    [SerializeField]
    private float _spawnInterval;
    /// <summary>
    /// 生成地点計算用：プレイヤーからの最低距離
    /// </summary>
    [SerializeField]
    private float _spawnMinDistance;
    /// <summary>
    /// 生成地点計算用：プレイヤーからの最大距離
    /// </summary>
    [SerializeField]
    private float _spawnMaxDistance;
    /// <summary>
    /// 隕石最大数
    /// </summary>
    [SerializeField]
    private int _maxCount;

    private int _randRange;
    private float _nowTime;
    private LinkedList<Asteroid> _fieldAsteroids;

    /// <summary>
    /// フィールド内にいる隕石リスト
    /// </summary>
    public LinkedList<Asteroid> FieldAsteroids
    {
        get
        {
            return _fieldAsteroids;
        }
    }

    private void OnDestroyAsteroid(Asteroid target)
    {
        _fieldAsteroids.Remove(target);
    }

    private void Start()
    {
        _fieldAsteroids = new LinkedList<Asteroid>();
        foreach (var data in _asteroids)
        {
            _randRange += data.weight;
        }
    }

    private void Update()
    {
        _nowTime += Time.deltaTime;
        while (_nowTime >= _spawnInterval)
        {
            if (_fieldAsteroids.Count < _maxCount)
            {
                var ast = SpawnRandomAsteroid();
                _fieldAsteroids.AddLast(ast);
            }
            _nowTime -= _spawnInterval;
        }
    }

    private Asteroid SpawnRandomAsteroid()
    {
        var point = Random.Range(0, _randRange);
        var nowRange = 0;
        foreach (var data in _asteroids)
        {
            nowRange += data.weight;
            if (point <= nowRange)
            {
                var pos = this.GetPlayerBaseRangePosition();
                var ast = Instantiate<Asteroid>(data.baseObj, pos, Random.rotation);
                ast.transform.SetParent(this.transform);
                ast.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * Random.Range(0.0f, 1000.0f), ForceMode.Acceleration);
                ast.OnDestroyAsteroid = this.OnDestroyAsteroid;
                ast.Player = _player;
                return ast;
            }
        }
        Debug.LogError("Asteroidの生成に失敗しました");
        return null;
    }

    /// <summary>
    /// プレイヤーを中心としてMinDistance～MaxDistanceのドーナツ状の範囲における座標をランダムで返す
    /// </summary>
    /// <returns>算出された座標</returns>
    private Vector3 GetPlayerBaseRangePosition()
    {
        var playerPos = _player.transform.position;
        var firstPos = new Vector3(
            Random.Range(-_spawnMaxDistance, _spawnMaxDistance),
            0,
            Random.Range(-_spawnMaxDistance, _spawnMaxDistance));
        var way = ((firstPos + playerPos) - playerPos).normalized;
        var lastPos = playerPos + firstPos + (way * _spawnMinDistance);
        return lastPos;
    }
}
