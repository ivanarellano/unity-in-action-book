using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public float speed = 10.0f;
    public int damage = 1;

	void Update () {
        transform.Translate(0, 0, speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other) {
        PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
        if (pc != null) {
            pc.Hurt(damage);
        }

        Destroy(gameObject);
    }
}
