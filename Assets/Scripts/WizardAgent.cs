using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;




public class WizardAgent : Agent{
    Rigidbody rBody;
    public Transform Target;
    public float speed = 10;
    private float previousDistance = float.MaxValue;

    public Wizard wizardScript;
    public Saber saberScript;
    
    public Brain brain0;
    public Brain brain1;
    public Brain brain2;
    public Brain brain3;



   


    public GameManager GameManager;


    void Start(){
        rBody = GetComponent<Rigidbody>();
    }

//    public override void AgentReset() {
//    }

    public void ChangeBrain(int brainID){
        if (brainID == 0){
            GetComponent<WizardAgent>().GiveBrain(brain0);
        }
        if (brainID == 1){
            GetComponent<WizardAgent>().GiveBrain(brain1);
        }
        if (brainID == 2){
            GetComponent<WizardAgent>().GiveBrain(brain2);
        }
        if (brainID == 3){
            GetComponent<WizardAgent>().GiveBrain(brain3);
        }
    }

    public void KillRewards(){
        AddReward(20.0f);
    }

    public void DamageRewards(){
        AddReward(1.0f);
    }

    public void DeadRewards(){
        AddReward(-10.0f);
    }


    public void GameReset(){
        Done();
    }

    public override void CollectObservations(){
        // Calculate relative position
        Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // Distance to edges of platform
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);

        // Agent velocity of wizard
        AddVectorObs(wizardScript.movement.normalized.x / 5);
        AddVectorObs(wizardScript.movement.normalized.z / 5);

        // Agent velocity of saber
        AddVectorObs(saberScript.movement.normalized.x / 5);
        AddVectorObs(saberScript.movement.normalized.z / 5);
    }


    public override void AgentAction(float[] vectorAction, string textAction){
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
            Target.position);

//         Reached target
        if (distanceToTarget < 3f){
            AddReward(-0.005f);
        }

        if (distanceToTarget < 1.8f){
            AddReward(-0.01f);
//            Done();
        }
//        if (distanceToTarget<1.2f){
//            AddReward(-0.1f);
////            Done();
//            GameManager.ResetGame();
//        }

//         Time reward for not be dead
        AddReward(0.005f);

        // Fell off platform
//        if (this.transform.position.y < -1.0) {
//            AddReward(-1.0f);
//            Done();
//        }

        // Actions, size = 2
        Vector3 move = Vector3.zero;
        Vector3 dir = Vector3.zero;
        float attackRate = 0f;
        float turnAngle = 0f;


        move.x = vectorAction[0];
        move.z = vectorAction[1];
//        dir.x = vectorAction[2];
//        dir.z = vectorAction[3];
        turnAngle = vectorAction[2];
        attackRate = vectorAction[3];

        if (attackRate > 0.5f){
            GetComponent<Wizard>().cliked = true;
        }
        else{
            GetComponent<Wizard>().cliked = false;
        }
//        print("attackRate"+attackRate);


        wizardScript.movement = move.normalized * .01f + wizardScript.movement;
//        wizardScript.movement = move.normalized;

//        Quaternion newRotation = Quaternion.LookRotation(turnAngle+transform.rotation.eulerAngles);
        Vector3 newEuler = new Vector3(0f, transform.rotation.eulerAngles.y + turnAngle * 5f, 0f);
        Quaternion newRotation = Quaternion.Euler(newEuler);
        rBody.MoveRotation(newRotation);

//        rBody.AddForce(controlSignal * speed);
    }
}