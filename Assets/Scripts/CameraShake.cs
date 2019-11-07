using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour {

    static public CameraShake S;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }

    public void Shake()
    {
        transform.DOShakePosition(0.3f, 0.5f, 20);
    }
}
