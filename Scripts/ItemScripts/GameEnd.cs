using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class GameEnd : MonoBehaviour
{
    private Collider2D collectionArea;

    [SerializeField]
    public GameObject[] gemObjects;

    private int collected = 0;
    public Rigidbody2D[] doors;

    public Text messages;
    public Text closedSign;
    public bool won = false;
    public Text collectedText;

    // Start is called before the first frame update
    void Start()
    {
        collectionArea = GetComponent<Collider2D>();
        collectionArea.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        collectedText.text = "Collected: " + this.collected;
        if (!won)
        {
            float maxDist = 5f;
            bool allIn = true;
            collected = 0;
            foreach(GameObject gem in this.gemObjects)
            {
                if (Vector2.Distance(gem.transform.position,this.transform.position)<maxDist)
                {
                    collected += 1;
                }
                else
                {
                    allIn = false;
                }
            }
            if (allIn)
            {
               
                print("You did it congrats!");
                closedSign.text = "OPEN";
                closedSign.color = Color.green;
                foreach (Rigidbody2D door in doors)
                {
                    door.constraints = RigidbodyConstraints2D.None;

                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(GameObject gem in this.gemObjects)
        {
            if (collision.gameObject == gem)
            {
                this.collected += 1;
            }
        }

        if (collected == this.gemObjects.Length||won)
        {
            won = true;
            print("You did it congrats!");
            closedSign.text = "OPEN";
            closedSign.color = Color.green;
            foreach(Rigidbody2D door in doors)
            {
                door.constraints = RigidbodyConstraints2D.None;

            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (GameObject gem in this.gemObjects)
        {
            if (collision.gameObject == gem)
            {
                this.collected -= 1;
            }
        }
    }
}
