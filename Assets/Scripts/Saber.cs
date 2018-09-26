using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Saber : MonoBehaviour {

    public float hp = 100f;
    public float atk = 10f;
    public float spd = 5f;


    public Animator anim_cut;
    public GameObject attackArea;
    public GameObject sword;
    public GameObject sword_fixed;

    public GameObject box;
    public Slider hpSlider;
    public GameObject canvas;
    public GameObject wizard;
    public GameObject swModel;
    public GameManager gm;
    public GameObject trash;

    public WizardAgent agent;
    public Vector3 movement;




    public bool atkCliked;

    RaycastHit floorHit;

    Rigidbody rb;
    Quaternion canvasDir = Quaternion.LookRotation(-1f * Vector3.up, Vector3.forward);

    int floorMask;

    bool attacking = false;
    bool preAttacking = false;
    bool inAttackArea = false;
    float timer = 0f;
    float disCount = 0f;
    float oldDisCount = 0f;

    float zDis = .62f;
    float deltaZ = 0f;
    private Vector3 towards;
    private float attackDis = 1.8f;



    List<GameObject> swList = new List<GameObject>();





    private void Awake() {
        rb = transform.GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        hpSlider.value = hp;
        //anim = GetComponent<Animator>();

        disCount = sword.transform.parent.transform.localPosition.x;
        oldDisCount = disCount;
        zDis = disCount;

    }



    // Update is called once per frame

    private void FixedUpdate() {
        hpSlider.value = hp;
        canvas.transform.rotation = canvasDir;


        if (!preAttacking) {
            Move();
            Rotate();
        }
        Animate();
        //rb.drag = 20;
    }

    void Move() {

        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");

        towards = wizard.transform.position - transform.position;

        float h = towards.x;
        float v = towards.z;

        if (towards.magnitude > attackDis){
            movement.Set(h, 0.0f, v);

        }else{
            movement.Set(0f, 0.0f, 0f);

        }

        movement = movement.normalized * Time.deltaTime * spd*1f;
        rb.MovePosition(transform.position + movement);



        //transform.LookAt(transform.forward, Vector3.up);

    }

    void Rotate() {

        Vector3 towards = wizard.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(towards, Vector3.up);


    }

    void Animate() {

        if (Input.GetKeyDown("l")) {
            preAttacking = true;
            attackArea.SetActive(true);
        }

        if (towards.magnitude < attackDis){
            preAttacking = true;
            attackArea.SetActive(true);
        }


        anim_cut.SetBool("IsAttacking", attacking);
        sword.SetActive(attacking);
//        sword_fixed.SetActive(attacking);


        if (preAttacking) {
            timer = timer + Time.deltaTime;
            if (timer > .2f) {
                attacking = true;
                disCount = sword.transform.parent.transform.localPosition.x;
                //print("disCount" + disCount);

                deltaZ = deltaZ+ oldDisCount-disCount;
                oldDisCount = disCount;

                if (timer > .4f+.2f) {
                    timer = 0f;
                    attackArea.SetActive(false);
                    preAttacking = false;
                    attacking = false;
                    foreach (GameObject sw in swList) Destroy(sw);

                    disCount = zDis;
                    oldDisCount = zDis;
                    deltaZ = 0f;
                }


            }
        }

        if (attacking) {
            //print("deltaZ"+deltaZ);
            if (deltaZ > .1f) {
                GameObject wsClone = Instantiate(swModel, sword.transform.position, sword.transform.rotation, transform);
                swList.Add(wsClone);
                deltaZ = 0f;
            }


        }



    }

    void Dead() {

//        Instantiate(box, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up),trash.transform);
        
        gameObject.SetActive(false);
        agent.KillRewards();
        gm.ResetGame();


    }


    public void Damaged(float enemyAtk) {
        hp -= enemyAtk;
        agent.DamageRewards();
        
        if (hp < 0) {
            Dead();
        }
    }


}
