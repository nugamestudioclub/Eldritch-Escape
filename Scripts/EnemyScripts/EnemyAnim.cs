using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyAnim : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> runSprites;
    [SerializeField]
    private List<Sprite> attackSprites;
    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    private float attackSpeed = 0.1f;
    private SpriteRenderer rend;
    private int count = 0;
    private int attackCount = 0;
    private bool runAnim = false;
    private bool runAttack = false;
    private bool running = false;

    public bool isRunning   // property
    {
        get { return runAnim; }   // get method
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rend = GetComponent<SpriteRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShowNextSprite()
    {
        yield return new WaitForSeconds(this.speed);
        if (this.runAnim&&!runAttack)
        {
            this.rend.sprite = runSprites[count];
            count += 1;
            count = count % runSprites.Count;
            StartCoroutine(ShowNextSprite());
        }
       
    }

    IEnumerator ShowNextAttackSprite()
    {
        yield return new WaitForSeconds(this.attackSpeed);
        if (runAttack)
        {
            this.rend.sprite = attackSprites[attackCount];
            attackCount += 1;
            if (attackCount >= attackSprites.Count)
            {
                print("Stopping attack!");
                this.runAttack = false;
                this.runAnim = true;
                this.attackCount = 0;

            }
            else
            {
                attackCount = attackCount % attackSprites.Count;
                StartCoroutine(ShowNextAttackSprite());
            }
            
        }

    }

    public void Play()
    {
        this.runAnim = true;
        StartCoroutine(ShowNextSprite());
        running = true;
    }
    public void Stop()
    {
        this.runAnim = false;
        running = false;
    }


    public void Attack()
    {
        this.attackCount = 0;
        this.runAttack = true;
        this.running = false;
        this.runAnim = false;
        StartCoroutine(ShowNextAttackSprite());
    }

    public bool IsAttacking()
    {
        return this.runAttack;
    }
}
