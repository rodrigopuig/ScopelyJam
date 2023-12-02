using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string axis;
    public string fire;
    public Transform player;
    public Transform cargo;
    public float initialIncline = 1;
    public float speed = 1;
    public float recoverSpeed = 0.5f;
    public float cargoSpeed = 0.2f;
    public GameObject sword;
    [Range(-1, 1)]
    public int attackDirection = 1;
    public float strikeForce = 3;
    public float hitForce = 3;

    private bool attacking;
    private bool attackHit;
    private float internalForce;
    private float externalForce;
    private bool collidingWithEnemy;
    private Collider selfCollider;

    // Start is called before the first frame update
    void Start()
    {
        sword.SetActive(false);

        if(attackDirection == -1)
        {
            sword.transform.localPosition = new Vector3(-sword.transform.localPosition.x, sword.transform.localPosition.y, sword.transform.localPosition.z);
        }

        selfCollider = GetComponent<Collider>();

        cargo.Rotate(new Vector3(0, 0, initialIncline));
    }

    void Update()
    {
        // ATTACK
        if(Input.GetAxis(fire)>0)
        {
            if(!attacking)
            {
                StartCoroutine(AttackRoutine());
                StartCoroutine(ApplyExternalForce(strikeForce * attackDirection, 0.2f));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MOVEMENT
        float input = Input.GetAxis(axis);

        // prevent attack movement when the attack hits
        if(attacking && !attackHit)
        {
            input += internalForce;
        }

        input += externalForce;

        if(collidingWithEnemy)
        {
            if(attackDirection > 0 && input > 0)
            {
                input = 0;
            }
            if(attackDirection < 0 && input < 0)
            {
                input = 0;
            }

        }

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
        sword.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        attackHit = false;
    }

    private IEnumerator ApplyInternalForce(float force, float time)
    {
        this.internalForce = force;
        yield return new WaitForSeconds(time);
        this.internalForce = 0;
    }

    private IEnumerator ApplyExternalForce(float force, float time)
    {
        this.externalForce = force;
        yield return new WaitForSeconds(time);
        this.externalForce = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collission with " + other.name);
        if(other.tag == "weapon")
        {
            Debug.Log("Applying force to " + gameObject.name);
            StartCoroutine(ApplyExternalForce(hitForce * -attackDirection, 0.2f));
        }
        if(other.tag == "Player")
        {
            if(attacking)
            {
                attackHit = true;
            }

            if (selfCollider.bounds.Intersects(other.bounds))
            {
                Debug.Log("collidingWithEnemy true");
                collidingWithEnemy = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("collidingWithEnemy false");
            collidingWithEnemy = false;
        }
    }
}
