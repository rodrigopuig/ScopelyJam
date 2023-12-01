using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string axis;
    public string fire;
    public Transform player;
    public Transform cargo;
    public float speed = 1;
    public float recoverSpeed = 0.5f;
    public float cargoSpeed = 0.2f;
    public GameObject sword;
    [Range(-1, 1)]
    public int attackDirection = 1;
    public float strikeForce = 3;
    public float hitForce = 3;

    private bool attacking;
    private float force;

    // Start is called before the first frame update
    void Start()
    {
        sword.SetActive(false);

        if(attackDirection == -1)
        {
            sword.transform.localPosition = new Vector3(-sword.transform.localPosition.x, sword.transform.localPosition.y, sword.transform.localPosition.z);
        }
    }

    void Update()
    {
        // ATTACK
        if(Input.GetAxis(fire)>0)
        {
            if(!attacking)
            {
                StartCoroutine(AttackRoutine());
                StartCoroutine(ApplyForce(strikeForce * attackDirection, 0.2f));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MOVEMENT
        float input = Input.GetAxis(axis);
        input += force;

        player.Translate(new Vector3(input * speed, 0, 0));

        if(cargo.eulerAngles.z > 180)
        {
            cargo.Rotate(new Vector3(0, 0, cargoSpeed + recoverSpeed * input));
        }
        else
        {
            cargo.Rotate(new Vector3(0, 0, -cargoSpeed + recoverSpeed * input));
        }

        if(cargo.eulerAngles.z > 270 || cargo.eulerAngles.z < 90)
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0;
        }

        

    }

    private IEnumerator AttackRoutine()
    {
        attacking = true;
        sword.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attacking = false;
        sword.SetActive(false);
    }

    private IEnumerator ApplyForce(float force, float time)
    {
        this.force = force;
        yield return new WaitForSeconds(time);
        this.force = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collission with " + other.name);
        if(other.tag == "weapon")
        {
            Debug.Log("Aplicando fuerza en " + gameObject.name);
            StartCoroutine(ApplyForce(hitForce * -attackDirection, 0.2f));
        }
    }
}
