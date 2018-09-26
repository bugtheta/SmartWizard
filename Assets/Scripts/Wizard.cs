using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour {


    public float hp = 100f;
    public float atk = 10f;
    public float spd = 5f;


    public Animator anim_l;
    public Animator anim_r;
    public GameObject attackArea;
    public GameObject box;
    public GameObject fireBall;
    public Slider hpSlider;
    public GameObject canvas;

    public GameManager gm;
    public GameObject trash;
    
    public WizardAgent agent;



    RaycastHit floorHit;
   public Vector3 movement;
    public bool cliked=false;

    Vector3 fireBorn;
    Vector3 fireMove;
    Vector3 fireLocalPos;


    bool hasFire =false;

    Ray camRay;
    Rigidbody rb;
    Quaternion canvasDir = Quaternion.LookRotation(-1f * Vector3.up,Vector3.forward);

    float camRayLength = 100f;
    int floorMask;

    bool attacking = false;

    float timer = 0f;




    private void Awake() {
        rb = transform.GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        hpSlider.value = hp;
        //anim = GetComponent<Animator>();

    }



    // Update is called once per frame


    private void FixedUpdate() {
        hpSlider.value = hp;

        if (!attacking) {
            Move();
//            Rotate();
        }
        Animate();
        //rb.drag = 20;
        canvas.transform.rotation = canvasDir;
    }

    void Move(){

//        float h = Input.GetAxisRaw("Horizontal");
//        float v = Input.GetAxisRaw("Vertical");
//        movement.Set(h, 0.0f, v);
        
        movement = movement.normalized * Time.deltaTime * spd;
        rb.MovePosition(transform.position + movement);



        //transform.LookAt(transform.forward, Vector3.up);

    }

    void Rotate() {
        camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            rb.MoveRotation(newRotation);
        }
    }

    void Animate() {
//        cliked = Input.GetMouseButtonDown(0);
        //float timer = 0f;

        //bool walking = (h != 0 || v != 0);
        if (cliked) {
            attacking = true;
            attackArea.SetActive(true);
        }

        if (attacking) {
            timer = timer + Time.deltaTime;
            if (timer > .35f) {
                attacking = false;
                timer = 0f;
                attackArea.SetActive(false);
                fireBall.transform.localPosition = Vector3.forward;

                fireBall.SetActive(true);
//                print("has set true");


                fireBorn = fireBall.transform.position;
                fireMove = fireBall.transform.position;
                fireLocalPos = fireBall.transform.localPosition;
                fireBall.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * 500.0f);
                hasFire = true;
            }
        }

        if (hasFire) {
            //print(fireBorn);
            //print(fireMove);

            fireMove = fireBall.transform.position;


            if (Vector3.Distance(fireBorn, fireMove) > 5f) {
                fireBall.SetActive(false);
//                print("has set false");

                fireBall.transform.localPosition = fireLocalPos;
                fireBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
                fireBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                hasFire = false;
            }


        }


        anim_l.SetBool("IsAttacking", attacking);
        anim_r.SetBool("IsAttacking", attacking);
        //print("attacking "+attacking);
        //print("Time.deltaTime " + Time.deltaTime);




    }

    void Dead(){
        //Instantiate(box, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up),trash.transform);
        gameObject.SetActive(false);
        agent.DeadRewards();
        gm.ResetGame();


    }

    public void resetFireBall(){
        fireMove = fireBorn;
        fireBall.SetActive(false);
//        print("trigger to false");

        fireBall.transform.localPosition = fireLocalPos;
        fireBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        fireBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        hasFire = false;
        timer = 0f;
        attackArea.SetActive(false);
        attacking = false;


    }



    public void Damaged(float enemyAtk){
        hp -= enemyAtk;
        if (hp<0){
            Dead();
        }
    }


}
