using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public LineRenderer[] patrolPaths;

    private List<List<GridVertex>> grid = new List<List<GridVertex>>();
    private float width;
    private float height;

    /// <summary>
    /// Generates an unblocked grid of vertices.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void GenerateGrid(float width, float height)
    {
        int size = Mathf.RoundToInt(width * height);
        this.width = width;
        this.height = height;
        this.grid = new List<List<GridVertex>>();
        int index = 0;
        for(int x = Mathf.RoundToInt(-width / 2); x < width / 2; x++)
        {
            List<GridVertex> row = new List<GridVertex>();
            for(int y = Mathf.RoundToInt(-height / 2); y < height / 2; y++)
            {
                row.Add(new GridVertex(new Vector2(x,y),false));
                
                index++;
            }
            this.grid.Add(row);
        }
    }

    /// <summary>
    /// Finds all objects with ObjectacleObject class component on it, then uses scale and all to block segmenets of
    /// grid.
    /// </summary>
    public void SetWorldBlockings()
    {
        NavigationObstacle[] obstacles = GameObject.FindObjectsOfType<NavigationObstacle>();
       
        foreach(NavigationObstacle obstacle in obstacles)
        {
            Vector2 pos = obstacle.transform.position;
            Vector2 scale = obstacle.transform.localScale;
            float rotation = obstacle.transform.eulerAngles.z;
            float absRot = Mathf.Abs(rotation);

            print("at object with scale:" + scale.x + "," + scale.y + " and name " + obstacle.gameObject.name);
            for(int x = 0; x < scale.x; x++)
            {
                for(int y = 0; y < scale.y; y++)
                {
                    if (absRot==0)
                    {
                        int offsetX = x + (int)pos.x+(int)(width/2);
                        int offsetY = y + (int)pos.y+(int)(height/2);
                        
                        if (offsetX < width && offsetX > 0 && offsetY < height && offsetY > 0)
                        {
                            print("Managing at 0 degrees: " + (x + pos.x) + "," + (y + pos.y));
                            grid[offsetX][offsetY].SetBlock();
                        }
                        else
                        {
                            print("Failed at 0 degrees: " + (x + pos.x) + "," + (y + pos.y));
                        }
                       
                        
                    }
                    else
                    {
                        int offsetX = y + (int)pos.x+(int)(width/2);
                        int offsetY = x + (int)pos.y+(int)(height/2);
                        if (offsetX < width && offsetX > 0 && offsetY < height && offsetY > 0)
                        {
                            print("managing at 90 degrees " + (x + pos.x) + "," + (y + pos.y));
                            grid[offsetX][offsetY].SetBlock();
                        }
                    }
                    
                }
            }
            

        }


    }


    public void VisualizeGrid()
    {
        GameObject parent = new GameObject();
        parent.name = "Grid";
        parent.transform.position = Vector3.zero;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Cube);
                vertex.name = "vertex" + x+","+y;
                vertex.transform.parent = parent.transform;
                vertex.transform.position = grid[x][y].getPos();
                Material mat = vertex.GetComponent<MeshRenderer>().material;
                if (grid[x][y].IsBlocked())
                {
                    mat.color = new Color(0, 0, 0);

                }
                else
                {
                    mat.color = new Color(255, 255, 255);
                }



            }
        }
            
            

    }

    private void Start()
    {
        GenerateGrid(50, 50);
        SetWorldBlockings();
        //VisualizeGrid();
    }

}
