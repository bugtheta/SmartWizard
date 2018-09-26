using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FireBall : MonoBehaviour {


    public Wizard wizardScript;
    float skillAtk;

    // Use this for initialization
    void Awake() {

        skillAtk = wizardScript.atk;
    }

    // Update is called once per frame
    //void FixedUpdate () {

    //}

    void Update() {
        transform.localPosition = Vector3.zero;

    }
    private void OnTriggerEnter(Collider other) {

//        print("triggering");

        wizardScript.resetFireBall();
        transform.localPosition = Vector3.zero;

        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            other.GetComponent<Saber>().Damaged(skillAtk);
        }
    }
}
