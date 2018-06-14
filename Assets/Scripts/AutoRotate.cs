using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隕石自動回転用
/// </summary>
public class AutoRotate : MonoBehaviour
{
    private Rigidbody _rigidBody;

    // Use this for initialization
    private void Start()
    {
        _rigidBody = this.GetComponent<Rigidbody>();
        _rigidBody.AddTorque(Random.onUnitSphere * 100.0f, ForceMode.Acceleration);
    }

}
