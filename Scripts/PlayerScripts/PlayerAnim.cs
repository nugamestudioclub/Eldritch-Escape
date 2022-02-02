using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Sprite[] walkingSprites;
    public Sprite[] runningSprites;
    public Sprite[] carryingSprites;
    private PlayerMovement movement;
    private SpriteRenderer rend;
    private Camera cam;
    private bool animRunning = false;
    [SerializeField]
    private float durationBetweenFrames = 0.2f;
    private int animPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        movement = GetComponent<PlayerMovement>();
        rend = GetComponent<SpriteRenderer>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (movement.GetVertDirection() == Direction.UP)
        {
            if (movement.GetDirection() == Direction.UP)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                if (movement.GetHorDirection() == Direction.LEFT)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else
                {
                    this.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }
           
        }
        else
        {
            if (movement.GetDirection() == Direction.DOWN)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                if (movement.GetHorDirection() == Direction.LEFT)
                {
                    this.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else
                {
                    this.transform.eulerAngles = new Vector3(0, 0, 90);
                }
            }

        }
        cam.transform.eulerAngles = new Vector3(0, 0, 0);

        if (movement.IsMoving())
        {
            if (!animRunning)
            {

                StartCoroutine(RunAnim());
            }
        }


    }

    IEnumerator RunAnim()
    {
        this.animRunning = true;
        yield return new WaitForSeconds(this.durationBetweenFrames);
        if (movement.IsHoldingItem())
        {
            rend.sprite = this.carryingSprites[animPos];
        }
        else if (movement.IsRunning())
        {
            rend.sprite = this.runningSprites[animPos];
        }
        else
        {
            rend.sprite = this.walkingSprites[animPos];
        }
        animPos += 1;
        animPos = animPos % this.walkingSprites.Length;
        if (movement.IsMoving())
        {
            StartCoroutine(RunAnim());
        }
        else
        {
            this.animRunning = false;
        }
    }

    public void StartWalking()
    {
        
    }
    public void StopWalking()
    {

    }

    public void StartRunning()
    {

    }
    public void StopRunning()
    {

    }

    public void StartCarry()
    {

    }
    public void StopCarry()
    {

    }
}
