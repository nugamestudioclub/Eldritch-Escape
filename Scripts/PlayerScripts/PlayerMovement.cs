using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool inputEnabled = false;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float boost = 1f;

    private Direction curDirection;
    private Direction vertDirection;
    private Direction horDirection;
    private Item heldItem;
    private bool holdingItem;

    private bool isRunning = false;

    private Item itemInRange;
    [SerializeField]
    private Transform holdingPos;

    private float hp = 100;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Image deadImg;
    [SerializeField]
    private Button retryBtn;
    private bool isDead = false;
    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private AudioClip walking;
    [SerializeField]
    private AudioClip running;
    
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curDirection = Direction.UP;
        EnableInput();
        this.retryBtn.onClick.AddListener(Retry);
        this.exitBtn.onClick.AddListener(Exit);
        source = GetComponent<AudioSource>();
    }

    private void Retry()
    {
        SceneManager.LoadScene(1);
    }
    private void Exit()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP: " + Mathf.Clamp(this.hp,0,100);
        if (!isDead && hp <= 0)
        {
            this.inputEnabled = false;
            deadImg.GetComponent<Animator>().Play("Show");
        }

        if (inputEnabled)
        {
            if (Input.GetButtonDown("Jump") && !this.holdingItem)
            {
                this.heldItem = this.itemInRange;
                this.itemInRange = null;
                this.heldItem.Pickup(holdingPos);
                this.rb.velocity = Vector2.zero;
                StartCoroutine(DelayPickup(this.heldItem.GetPickupDuration()));
                this.holdingItem = true;
                this.isRunning = false;

            }
            else if (Input.GetButtonDown("Jump") && this.holdingItem)
            {
                this.itemInRange = heldItem;
                this.heldItem = null;
                this.itemInRange.Drop();
                this.rb.velocity = Vector2.zero;
                StartCoroutine(DelayPickup(this.itemInRange.GetPickupDuration()));
                this.holdingItem = false;
                this.isRunning = false;
            }

            if (Input.GetButton("Shift")&&!this.holdingItem)
            {
                this.isRunning = true;
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * (speed+boost), Input.GetAxis("Vertical") * (speed + boost));
            }
            else
            {
                this.isRunning = false;
                rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
            }

            
            
            if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
            {
                
                if (rb.velocity.x > 0)
                {
                    this.curDirection = Direction.RIGHT;
                }
                else if (rb.velocity.x < 0)
                {
                    this.curDirection = Direction.LEFT;
                }
                
            }
            else
            {
                if (rb.velocity.y > 0)
                {
                    this.curDirection = Direction.UP;
                }
                else if (rb.velocity.y < 0)
                {
                    this.curDirection = Direction.DOWN;
                }

            }
            if (rb.velocity.x > 0)
            {
                this.horDirection = Direction.RIGHT;
            }
            else if (rb.velocity.x < 0)
            {
                this.horDirection = Direction.LEFT;
            }
            if (rb.velocity.y > 0)
            {
                this.vertDirection = Direction.UP;
            }
            else if (rb.velocity.y < 0)
            {
                this.vertDirection = Direction.DOWN;
            }

        }

        if (this.IsMoving())
        {
            if (isRunning)
            {
                this.source.clip = running;
                this.source.loop = true;
                
            }
            else
            {
                this.source.clip = walking;
                this.source.loop = true;
                
            }
            if (!this.source.isPlaying)
            {
                this.source.Play();
            }
        }
        else
        {
            this.source.Stop();
        }

        
    }

   


    public void EnableInput()
    {
        this.inputEnabled = true;
    }

    public void DisableInput()
    {
        this.inputEnabled = false;
    }

    public Direction GetDirection()
    {
        return this.curDirection;
    }

    public Direction GetVertDirection()
    {
        return this.vertDirection;
    }
    public Direction GetHorDirection()
    {
        return this.horDirection;
    }

    public bool IsHoldingItem()
    {
        return this.holdingItem;
    }
    public bool IsRunning()
    {
        return this.isRunning;
    }
    public bool IsMoving()
    {
        return rb.velocity != Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item i = collision.GetComponent<Item>();
            itemInRange = i;
            i.SetIndicatorVisible();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item i = collision.GetComponent<Item>();
            i.SetIndiciatorInvisible();
            this.itemInRange = null;
        }
    }

    private IEnumerator DelayPickup(float time)
    {
        this.inputEnabled = false;
        yield return new WaitForSeconds(time);
        this.inputEnabled = true;
    }

    public float GetHP()
    {
        return this.hp;
    }

    public void SetHP(float hp)
    {
        this.hp = hp;
    }

    public void UpdateHP(float damage)
    {
        this.hp -= damage;
        StartCoroutine(ApplyTempSpeedBoost());
    }

    private IEnumerator ApplyTempSpeedBoost()
    {
        this.speed += 5f;
        yield return new WaitForSeconds(3);
        this.speed -= 5f;
    }

}
public enum Direction { UP,DOWN,LEFT,RIGHT};