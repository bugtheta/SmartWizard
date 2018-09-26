using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public GameObject wizard;
    public GameObject saber;
    public GameObject trash;
    public Text textS;
    public Text textW;

    public WizardAgent agent;

    public Slider levelSlider;
    public GameObject info;

//    public Brain brain;


    private int wScore;
    private int sScore;

    private bool isAIMode = true;
    private int aiLevel = 0;

    // Use this for initialization
    void Start(){
    }

    // Update is called once per frame
//    void FixedUpdate(){
//    }

    public void ResetGame(){
        if (wizard.activeSelf && saber.activeSelf){
        }else if(wizard.activeSelf){
        wScore += 1;
            textW.text = "WIZARD: "+wScore.ToString();
        }
        else{
            sScore += 1;
            textS.text = "WARRIOR: "+sScore.ToString();

        }

        wizard.GetComponent<Wizard>().hp = 100;
        saber.GetComponent<Saber>().hp = 100;
        wizard.SetActive(true);
        saber.SetActive(true);
        wizard.transform.position = new Vector3(3f, 0.5f, 3f);
        wizard.GetComponent<Wizard>().movement = Vector3.zero;
        saber.transform.position= new Vector3(-3f, 0.5f, -3f);

        wizard.transform.rotation = Quaternion.LookRotation(Vector3.forward*-1f);
        saber.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        
        agent.GameReset();

        
//        Transform[] allChildren = trash.GetComponentsInChildren<Transform>();
//        foreach (Transform box in allChildren){
//            Destroy(box.gameObject);
//        }
    }

    public void RefreshScoreBoard(){
        
    }

    public void Chang2PayerMode(){
        info.SetActive(true);
        levelSlider.gameObject.SetActive(false);
        isAIMode = false;
        aiLevel = 0;
        agent.ChangeBrain(aiLevel);
        ResetGame();



    }

    public void Change2AIMode(){
        info.SetActive(false);
        levelSlider.gameObject.SetActive(true);
        isAIMode = true;
        aiLevel = (int) levelSlider.value;
        agent.ChangeBrain(aiLevel);
        ResetGame();

    }

    public void ChangeLevel(){
        aiLevel = (int) levelSlider.value;
        agent.ChangeBrain(aiLevel);
        ResetGame();



    }

}