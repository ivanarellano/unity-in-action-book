using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceOperator : MonoBehaviour {

    public float radius = 1.5f;

	void Update () {
		if (Input.GetButtonDown("Fire3")) {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider hit in hitColliders) {
                Vector3 direction = hit.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, direction) > 0.5f) {
                    /// SendMessage() tries to call the named
                    /// function regardless of the target's type
                    hit.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
	}
}
