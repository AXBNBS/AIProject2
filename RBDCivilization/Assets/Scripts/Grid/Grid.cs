
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid : MonoBehaviour
{
    public float hexagonWth, hexagonHgt;
    
    [SerializeField] private Transform hexagonPfb;
    [SerializeField] private int gridWth, gridHgt;
    [SerializeField] private float gap, hexagonScl;
    private Vector3 startPos;
    private int hexagonsX, hexagonsY;


    // We initialize some variables and add the gap to the hexagons's with and height, we calculate the starting position of the grid, and we finally create it.
    private void Awake ()
    {
        hexagonWth *= hexagonScl;
        hexagonHgt *= hexagonScl;
        hexagonsX = (int) (gridWth / hexagonWth);
        hexagonsY = (int) (gridHgt / hexagonHgt);

        AddGap ();
        StartPosition ();
        CreateGrid ();
    }


    //
    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireCube (this.transform.position, new Vector3 (gridWth, 1, gridHgt));
    }


    // We add the gap value we've set to the hexagons's with and height.
    private void AddGap () 
    {
        hexagonWth += hexagonWth * gap;
        hexagonHgt += hexagonHgt * gap;
    }


    // We define the starting position where the first hexagon will be placed.
    private void StartPosition () 
    {
        float offset = 0;

        if (gridHgt / 2 % 2 != 0) 
        {
            offset = hexagonWth / 2;
        }

        float x = -(gridWth / 2) + offset;
        float z = +0.5f * (gridHgt / 2);
        //float x = -hexagonWth * (gridWth / 2) - offset;
        //float z = +hexagonHgt * 0.75f * (gridHgt / 2);

        startPos = new Vector3 (x, 0.1f, z);
    }


    // Get an hexagon's world position from its position on the grid.
    Vector3 CalculateWorldPosition (Vector2 gridPos) 
    {
        float offset = 0;
        if (gridPos.y % 2 != 0) 
        {
            offset = hexagonWth / 2;
        }

        float x = startPos.x + gridPos.x * hexagonWth + offset;
        float z = startPos.z - gridPos.y * hexagonHgt * 0.75f;

        return new Vector3 (x, 0.1f, z);
    }


    // We instantiate every hexagon and put it in its corresponding position on the grid.
    private void CreateGrid () 
    {
        int hexagonCnt = 1;
        Hexagon[,] hexagons = new Hexagon[hexagonsX + 1, hexagonsY + 1];

        for (int y = 0; y <= hexagonsY; y += 1) 
        {
            for (int x = 0; x <= hexagonsX; x += 1)
            {
                Transform hexagon = Instantiate (hexagonPfb) as Transform;
                Vector2 gridPos = new Vector2 (x, y);

                hexagon.position = CalculateWorldPosition (gridPos);
                hexagon.localScale *= hexagonScl;
                hexagon.parent = this.transform;
                hexagon.name = "Hexagon" + hexagonCnt;
                hexagonCnt += 1;
                hexagons[x, y] = hexagon.GetComponent<Hexagon> ();
            }
        }

        AssignNeighbours (hexagons);
    }


    //
    private void AssignNeighbours (Hexagon[,] hexagons)
    {
        for (int x = 0; x <= hexagonsX; x += 1) 
        {
            for (int y = 0; y <= hexagonsY; y += 1) 
            {
                if (y > 0) 
                {
                    if (hexagons[x, y - 1].transform.position.x < hexagons[x, y].transform.position.x)
                    {
                        hexagons[x, y].neighbours[0] = hexagons[x, y - 1];
                        if (x != hexagonsX)
                        {
                            hexagons[x, y].neighbours[1] = hexagons[x + 1, y - 1];
                        }
                    }
                    else 
                    {
                        hexagons[x, y].neighbours[1] = hexagons[x, y - 1];
                        if (x != 0)
                        {
                            hexagons[x, y].neighbours[0] = hexagons[x - 1, y - 1];
                        }
                    }
                }
                if (x != 0) 
                {
                    hexagons[x, y].neighbours[5] = hexagons[x - 1, y];
                }
                if (x != hexagonsX) 
                {
                    hexagons[x, y].neighbours[2] = hexagons[x + 1, y];
                }
                if (y < hexagonsY)
                {
                    if (hexagons[x, y + 1].transform.position.x < hexagons[x, y].transform.position.x)
                    {
                        hexagons[x, y].neighbours[4] = hexagons[x, y + 1];
                        if (x != hexagonsX)
                        {
                            hexagons[x, y].neighbours[3] = hexagons[x + 1, y + 1];
                        }
                    }
                    else
                    {
                        hexagons[x, y].neighbours[3] = hexagons[x, y + 1];
                        if (x != 0)
                        {
                            hexagons[x, y].neighbours[4] = hexagons[x - 1, y + 1];
                        }
                    }
                }
            }
        }
    }
}