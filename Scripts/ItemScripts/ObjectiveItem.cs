using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveItem : MonoBehaviour,Item
{
    [SerializeField]
    private SpriteRenderer indicator;
    [SerializeField]
    private float pickupDuration;

    private List<Collider2D> colliders = new List<Collider2D>();


    public float GetPickupDuration()
    {
        return this.pickupDuration+0;
    }

    public void SetIndicatorVisible()
    {
        this.indicator.enabled = true;
    }

    public void SetIndiciatorInvisible()
    {
        this.indicator.enabled = false;
    }

    public void Pickup(Transform parent)
    {
        this.colliders = new List<Collider2D>();
        foreach(Collider2D collider in this.GetComponents<Collider2D>())
        {
            collider.enabled = false;
            this.colliders.Add(collider);
        }
        this.transform.position = parent.position;
        this.transform.parent = parent;
        this.transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        foreach(Collider2D collider in this.colliders)
        {
            collider.enabled = true;

        }
        this.transform.parent = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(this.indicator==null)
            this.indicator = GetComponentInChildren<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
