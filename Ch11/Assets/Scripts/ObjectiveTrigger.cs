using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        Managers.Mission.ReachObjective();
    }
}
