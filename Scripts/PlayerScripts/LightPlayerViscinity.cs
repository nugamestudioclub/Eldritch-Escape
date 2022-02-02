using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightPlayerViscinity : MonoBehaviour
{

    private GameObject player;

    private Light2D light;
    private float initialIntensity;
    private bool wasSeen = false;

    private Vector2 originalRot;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialIntensity = light.intensity;
        originalRot = this.transform.eulerAngles;
    }
    
    // Update is called once per frame
    void Update()
    {
        //((distance - 5)/5)*0.35
        float dist = Vector2.Distance(player.transform.position, this.transform.position);
        if (dist > 6)
        {
            light.intensity = 0;
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, -player.transform.up);
            bool hasHit = false;
            
            foreach(RaycastHit2D hit in hits){
               
                if (hit.collider.gameObject == this.gameObject)
                {
                    hasHit = true;
                   
                    light.intensity = Mathf.Clamp(1 - (dist / 6f), 0, 1f);

                    Vector2 playerRight = -this.transform.up;
                    Vector2 towardsOther = player.transform.position - this.transform.position;

                    float angle = Vector2.Angle(playerRight, towardsOther);
                    print("player me vector:" + angle);
                    if (!wasSeen)
                    {
                        //this.transform.Rotate(new Vector3(0, 0, angle));
                    }
                }
                
               
            }
            if (!hasHit)
            {
                light.intensity = 0;
                //this.transform.eulerAngles = this.originalRot;
                
            }
            wasSeen = hasHit;
            
        }
        
    }
}
