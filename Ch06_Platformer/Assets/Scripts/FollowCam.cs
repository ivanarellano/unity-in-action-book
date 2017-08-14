using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    [SerializeField] private Transform target;

    private Vector3 vel = Vector3.zero;

    public float smoothTime = 0.2f;

    void LateUpdate() {
        Vector3 targetPos = new Vector3(
            target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position,
            targetPos, ref vel, smoothTime);
    }
}
