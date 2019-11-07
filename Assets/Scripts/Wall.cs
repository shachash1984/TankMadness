using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject, 1f);
    }
}
