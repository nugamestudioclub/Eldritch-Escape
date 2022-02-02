using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVertex 
{
    private Vector2 pos;
    private bool isBlocked;
    public GridVertex(Vector2 pos, bool isBlocked)
    {
        this.pos = pos;
        this.isBlocked = isBlocked;
    }

    public Vector2 getPos()
    {
        return new Vector2(pos.x, pos.y);
    }

    public void updatePos(Vector2 pos)
    {
        this.pos = pos;
    }

    public bool IsBlocked()
    {
        return this.isBlocked;
    }

    public void SetBlock()
    {
        this.isBlocked = true;
    }
    public void UnsetBlock()
    {
        this.isBlocked = false;
    }

}
