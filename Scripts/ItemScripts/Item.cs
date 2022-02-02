using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item 
{
    void SetIndicatorVisible();

    void SetIndiciatorInvisible();

    float GetPickupDuration();

    void Pickup(Transform parent);
    void Drop();
}
