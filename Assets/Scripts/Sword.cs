using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    public Saber saberScript;
    float skillAtk;

    // Use this for initialization
    void Start () {
        skillAtk = saberScript.atk;


    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("cutting");

            other.GetComponent<Wizard>().Damaged(skillAtk);
        }

    }
}
