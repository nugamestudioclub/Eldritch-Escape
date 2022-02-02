using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyAnim))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyAnim anim;
    private GameObject player;
    [SerializeField]
    private float range = 50f;
    private RaycastHit2D hit;
    private bool seesSomething = false;
    private bool pSeesSomething = false;
    private bool inRange = false;
    [SerializeField]
    private float damage = 40f;

    private LineRenderer curPatrol = null;
    private int patrolNum = 0;

    /// <summary>
    /// Number of seconds it takes for the enemy to give up chasing you after it has lost sight of you.
    /// </summary>
    [SerializeField]
    private float giveUpDuration = 5f;
    private bool moving = false;

    [SerializeField]
    private float speed = 10f;
    private SpriteRenderer rend;

    /// <summary>
    /// If the enemy should flip left/right based on direction of movement.
    /// </summary>
    [SerializeField]
    private bool fliipable = false;

    /// <summary>
    /// If the enemy should rotate in direction of movement.
    /// </summary>
    [SerializeField]
    private bool rotatable = true;

    private bool attacking = false;
    private bool patroling = false;


    private LineRenderer[] patrolPaths;
    private NavigationManager navManager;
    private bool inPatrolZone = false;
    private Camera cam;
    [SerializeField]
    private AudioClip movingSound;
    [SerializeField]
    private AudioClip runningSound;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<EnemyAnim>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        patroling = true;

        navManager = GameObject.FindObjectOfType<NavigationManager>();
        patrolPaths = navManager.patrolPaths;
        cam = GameObject.FindObjectOfType<Camera>();
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,this.transform.position.z - 15f);
        float dist = Vector2.Distance(new Vector2(this.player.transform.position.x, this.player.transform.position.y),
            new Vector2(this.transform.position.x, this.transform.position.y));
        //If no attack is happening then run running stuff
        if (!attacking)
        {
            if (seesSomething && !anim.isRunning && dist < this.range)
            {
                print("saw something!");
                this.curPatrol = null;
                
                anim.Play();
                moving = true;
            }
            else
            {
                if (pSeesSomething != seesSomething)
                {
                    StartCoroutine(GiveUp());
                }
                if (patroling)
                {
                    PatrolUpdate();
                }
            }
        }
        else
        {
            //if is attacking, check if it is still attacking until attack anim is over.
            attacking = anim.IsAttacking();
            if (!attacking)
            {
                if (inRange)
                {
                    attacking = true;
                    anim.Attack();
                    player.GetComponent<PlayerMovement>().UpdateHP(this.damage);
                    print("Player HP:" + player.GetComponent<PlayerMovement>().GetHP());
                }
                else
                {
                    anim.Play();
                    moving = true;
                }
               

            }
        }
       
        if (moving)
        {
            //rb.MovePosition(this.transform.position + );
            rb.velocity = ((this.player.transform.position - this.transform.position) / dist) * speed;
            
            rend.flipX = rb.velocity.x > 0&&fliipable;

            if (rotatable)
            {
                this.transform.eulerAngles = Vector3.RotateTowards(this.transform.position, this.player.transform.position, Mathf.PI / 4, 1);
            }
        }
        
        
        pSeesSomething = seesSomething;
    }
    private IEnumerator GiveUp()
    {
        yield return new WaitForSeconds(this.giveUpDuration);
        anim.Stop();
        moving = false;
        print("Nevermind it got away!");
        patroling = true;
        
    }

    private void FixedUpdate()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, player.transform.position-this.transform.position);
        bool sawSomething = false;
        int count = 0;
        bool seesWall = false;

        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                
                if (hit.collider.gameObject.CompareTag("Player")&&!seesWall)
                {
                    
                    count++;
                    sawSomething = true;
                    patroling = false;
                }
                else
                {
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        count += 1;
                    }
                    if (hit.collider.gameObject.CompareTag("Wall"))
                    {
                        seesWall = true;
                    }
                }

            }
            else
            {
                
            }
        }
        seesSomething = sawSomething;


        if (moving||patroling)
        {
            if (patroling)
            {
                audio.clip = this.movingSound;

            }
            else
            {
                audio.clip = this.runningSound;
            }
            if (!audio.isPlaying)
            {

                audio.loop = true;
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }

    }

    private void PatrolUpdate()
    {
        if (!inPatrolZone)
        {
            GameObject patrolZoneCollider = GameObject.FindGameObjectWithTag("Patrol Zone");
            if (player.transform.position.x > patrolZoneCollider.transform.position.x)
            {
                this.inPatrolZone = true;
            }

            return;
        }
        print("Updating...");
        if (curPatrol == null)
        {
            this.patrolNum = 1;
            LineRenderer[] paths = GameObject.FindObjectOfType<NavigationManager>().patrolPaths;
            this.curPatrol = paths[Random.Range(0,paths.Length)];
            this.transform.position = curPatrol.transform.position + curPatrol.GetPosition(0);
            
            print(curPatrol.GetPosition(0));
            anim.Play();

        }

        if (Vector3.Distance(this.transform.position, curPatrol.transform.position+curPatrol.GetPosition(patrolNum)) < 0.5f)
        {
            this.patrolNum = (patrolNum + 1);
            if (this.patrolNum >= curPatrol.positionCount)
            {
                LineRenderer[] paths = GameObject.FindObjectOfType<NavigationManager>().patrolPaths;
                this.curPatrol = paths[Random.Range(0, paths.Length)];
            }
        }
        
        this.transform.position = Vector3.MoveTowards(this.transform.position, curPatrol.transform.position + curPatrol.GetPosition(patrolNum), 0.05f);

    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&seesSomething)
        {
            //collision.GetComponent<PlayerMovement>().DisableInput();
            attacking = true;
            anim.Attack();
            inRange = true;
        }
        if (collision.CompareTag("Breakable"))
        {
            collision.GetComponent<HingeJoint2D>().enabled = false;

            
        }

        if(collision.CompareTag("Patrol Zone"))
        {
            this.inPatrolZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    public bool IsPatrolling()
    {
        return this.patroling;
    }
}
