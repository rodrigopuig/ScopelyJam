using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Rodrigo;
using MANUELITO;

public class Player : MonoBehaviour
{
    public Transform trEnemy;

    public string axis;
    public string fire;
    public string parry;
    public Transform player;
    public Transform cargo;
    public float initialIncline = 1;
    public float speed = 1;
    public float recoverSpeed = 0.5f;
    public float cargoSpeed = 0.2f;
    public GameObject sword;
    [Range(-1, 1)]
    public int attackDirection = 1;
    public float attackCD = 0.5f;
    public float attackTime = 0.2f;
    public float parryCD = 0.5f;
    public float parryTime = 0.2f;
    public float strikeForce = 3;
    public float hitForce = 3;
    public float parryForce = 5;
    public float cargoForceDamp = 0.5f;
    public bool won;
    public bool lost;
    public float extraWeightMultiplier = 2;
    public float lessWeightMultiplier = 0.5f;

    [Header("UI")]
    public UnityEngine.UI.Image attackImage;
    public Color attackColor;
    public Color attackCDColor;
    public UnityEngine.UI.Image parryImage;
    public Color parryColor;
    public Color parryCDColor;

    [HideInInspector] public bool parrying;

    private bool attacking;
    private bool attackHit;
    private float internalForce;
    private float externalForce;
    private bool collidingWithEnemy;
    private bool collidingWithWall;
    private Collider selfCollider;
    private Collider weaponCollider;
    private Animator animator;
    private bool waitAttack;
    private bool waitParry;
    private bool updatedBoxes;

    [Header("Particles")]
    [SerializeField] GameObject sweatParticles;
    [SerializeField] ParticleSystem fishParticles;
    [SerializeField] ParticleSystem blockParticles;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        sword.SetActive(false);

        if(attackDirection == -1)
        {
            sword.transform.localPosition = new Vector3(-sword.transform.localPosition.x, sword.transform.localPosition.y, sword.transform.localPosition.z);
        }

        selfCollider = GetComponent<Collider>();
        weaponCollider = sword.GetComponent<Collider>();

        animator = GetComponent<Animator>();

        cargo.Rotate(new Vector3(0, 0, initialIncline));

        attackImage.fillAmount = 1;
        parryImage.fillAmount = 1;
        attackImage.color = attackColor;
        parryImage.color = parryColor;

        yield return null;
        
        UpdateBoxes();
    }

    void Update()
    {
        // ATTACK
        if(Input.GetAxis(fire)>0)
        {
            if(!attacking && sword.activeSelf == false)
            {
                StartCoroutine(AttackRoutine());
                StartCoroutine(ApplyInternalForce(strikeForce * attackDirection, 0.2f));
            }
        }
        
        if(Input.GetAxis(parry) > 0 && sword.activeSelf == false)
        {
            if(!parrying)
            {
                StartCoroutine(ParryRoutine());
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MOVEMENT
        float input = Input.GetAxis(axis);
        float localInternalForce = 0;
        float localExternalForce = 0;

        // prevent attack movement when the attack hits
        if (attacking && !attackHit)
        {
            localInternalForce = internalForce;
        }
        localExternalForce = externalForce;

        if(collidingWithEnemy)
        {
            if(attackDirection > 0 && input > 0)
            {
                input = 0;
                localExternalForce = 0;
                localInternalForce = 0;
            }
            if(attackDirection < 0 && input < 0)
            {
                input = 0;
                localExternalForce = 0;
                localInternalForce = 0;
            }
        }

        if (collidingWithWall)
        {
            if (attackDirection < 0 && input > 0)
            {
                input = 0;
                localExternalForce = 0;
                localInternalForce = 0;
            }
            if (attackDirection > 0 && input < 0)
            {
                input = 0;
                localExternalForce = 0;
                localInternalForce = 0;
            }
        }

        animator.SetFloat("speed", Mathf.Abs(input + localExternalForce + localInternalForce));

        player.Translate(new Vector3((input + localExternalForce + localInternalForce) * speed, 0, 0));

        if (attackDirection > 0 && player.position.x > (trEnemy.position.x - 1.1f))
            player.transform.position = new Vector3((trEnemy.position.x - 1.1f), player.position.y, player.position.z);
        else if(attackDirection < 0 && player.position.x < (trEnemy.position.x + 1.1f))
            player.transform.position = new Vector3((trEnemy.position.x + 1.1f), player.position.y, player.position.z);

        float weightMultiplier = 1;
        if(won)
        {
            weightMultiplier = extraWeightMultiplier;
        }
        else if(lost)
        {
            weightMultiplier = lessWeightMultiplier;
        }

        if(cargo.eulerAngles.z > 180)
        {
            cargo.Rotate(new Vector3(0, 0, cargoSpeed * weightMultiplier + recoverSpeed * (input + localExternalForce * cargoForceDamp)));
        }
        else
        {
            cargo.Rotate(new Vector3(0, 0, -cargoSpeed * weightMultiplier + recoverSpeed * input + localExternalForce * cargoForceDamp));
        }

        bool sweating = cargo.eulerAngles.z > 225 || cargo.eulerAngles.z < 135;
        bool beingPushed = externalForce <= 0;
        sweatParticles.SetActive(sweating);
        if (sweating && !beingPushed)
        {
            animator.Play("Head_Fall");
        }
        else if(!beingPushed)
        {
            animator.Play("Head_Idle");
        }

        if (cargo.eulerAngles.z > 270 || cargo.eulerAngles.z < 90)
        { /*
            Debug.Log("GAME OVER");
            Time.timeScale = 0.00001f;
            GameController.instance.NextRound(this);*/
        }
    }

    public void UpdateBoxes()
    {
        if (updatedBoxes) return;
        updatedBoxes = true;

        Debug.Log("Updating boxes lost: " + lost + " won: " + won + " for " + gameObject.name);

        PiledItemsController items = GetComponentInChildren<PiledItemsController>();
        if(lost)
        {
            // Debug.Log("Deactivating 2");
            items.piledItems[2].gameObject.SetActive(false);
            items.piledItems[1].gameObject.SetActive(false);
        }
        else if (!won)
        {
            // Debug.Log("Deactivating 1");
            items.piledItems[2].gameObject.SetActive(false);
        }
        else
        {
            // Debug.Log("Deactivating 0");
        }

    }

    public void DoAttack()
    {
        waitAttack = true;
    }

    public void DoParry()
    {
        waitParry = true;
    }

    public void ApplyParry()
    {
        blockParticles.Play();
        //restore attack
        attacking = false;
        attackHit = false;
    }

    private IEnumerator AttackRoutine()
    {
        sword.GetComponent<Renderer>().material.color = attackCDColor;
        animator.Play("Attack");
        // Debug.Log("Attacking");

        attacking = true;
        yield return new WaitUntil(() => waitAttack);
        waitAttack = false;
        sword.SetActive(true);
        attackImage.fillAmount = 0;
        attackImage.color = attackCDColor;
        DOTween.To(() => attackImage.fillAmount, x => attackImage.fillAmount = x, 1, attackTime+attackCD).OnComplete(() => attackImage.color = attackColor);
        yield return new WaitForSeconds(attackTime);
        animator.Play("Torso_Idle");
        sword.SetActive(false);
        yield return new WaitForSeconds(attackCD);
        attacking = false;
        attackHit = false;
    }

    private IEnumerator ParryRoutine()
    {
        sword.GetComponent<Renderer>().material.color = parryCDColor;
        animator.Play("Parry");
        // Debug.Log("Parrying");

        parrying = true;
        yield return new WaitUntil(() => waitParry);
        waitParry = false;
        sword.SetActive(true);
        parryImage.fillAmount = 0;
        parryImage.color = parryCDColor;
        DOTween.To(() => parryImage.fillAmount, x => parryImage.fillAmount = x, 1, parryTime+parryCD).OnComplete(() => parryImage.color = parryColor);
        yield return new WaitForSeconds(parryTime);
        animator.Play("Torso_Idle");
        sword.SetActive(false);
        yield return new WaitForSeconds(parryCD);
        parrying = false;
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
        animator.Play("Head_Hit");
        yield return new WaitForSeconds(time);
        this.externalForce = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "weapon")
        {
            {
                Player otherPlayer = other.GetComponentInParent<Player>();
                if(otherPlayer.parrying && attacking)
                {
                    otherPlayer.ApplyParry();
                    FreezeFrameManager.FreezeFrame();
                    StartCoroutine(ApplyExternalForce(parryForce * -attackDirection, 0.2f));
                }
                else if (otherPlayer.attacking && !parrying)
                {
                    FreezeFrameManager.FreezeFrame();
                    fishParticles.Play();
                    StartCoroutine(ApplyExternalForce(hitForce * -attackDirection, 0.2f));
                }
            }
        }
        else if(other.tag == "Player")
        {
            if(attacking)
            {
                attackHit = true;
            }

            if (selfCollider.bounds.Intersects(other.bounds))
            {
                // Debug.Log("collidingWithEnemy true");
                collidingWithEnemy = true;
            }
        }
        else if(other.tag == "wall")
        {
            if (selfCollider.bounds.Intersects(other.bounds))
            {
                // Debug.Log("collidingWithWall true");
                collidingWithWall = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Debug.Log("collidingWithEnemy false");
            collidingWithEnemy = false;
        }
        else if (other.tag == "wall")
        {
            // Debug.Log("collidingWithWall false");
            collidingWithWall = false;
        }
    }
}
