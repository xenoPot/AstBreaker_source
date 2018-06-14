using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロックオン表示UI
/// </summary>
public class LockonUI : MonoBehaviour
{
    /// <summary>
    /// ロックオン処理オブジェクト
    /// </summary>
    [SerializeField]
    private LockonCtrl _lockon;
    /// <summary>
    /// ロックオンマーカーのベースオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject _lockOnUIObj;

    private GameObject[] _lockOnUIObjects;

    private void Start()
    {
        _lockOnUIObjects = new GameObject[_lockon.MaxBullet];
        for (int i = 0; i < _lockon.MaxBullet; i++)
        {
            var obj = Instantiate(_lockOnUIObj);
            obj.transform.SetParent(this.transform.parent);
            obj.SetActive(false);
            _lockOnUIObjects[i] = obj;
        }
    }

    private void Update()
    {
        var count = 0;
        foreach (var target in _lockon.Targets)
        {
            if (target == null) { continue; }
            var obj = _lockOnUIObjects[count];
            obj.SetActive(true);
            obj.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
            count++;
        }
        // 使われなかった分を非表示
        for (int i = count; i < _lockOnUIObjects.Length; ++i)
        {
            _lockOnUIObjects[i].SetActive(false);
        }
    }
}
