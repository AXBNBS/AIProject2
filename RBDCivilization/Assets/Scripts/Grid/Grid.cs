
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid : MonoBehaviour
{
    public float hexagonWth, hexagonHgt, limitX1, limitX2, limitZ1, limitZ2;
    public int gridWth, gridHgt;

    [SerializeField] private Transform hexagonPfb;
    //[SerializeField] private int gridWth, gridHgt;
    [SerializeField] private float gap, hexagonScl;
    private Vector3 startPos;
    public int hexagonsX, hexagonsY;
    private Transform ground;

    public Hexagon[,] hexagons;

    public GameObject Capital; //Generar las dos capitales


    // We initialize some variables and add the gap to the hexagons's with and height, we calculate the starting position of the grid, and we finally create it.
    private void Awake ()
    {
        hexagonWth *= hexagonScl;
        hexagonHgt *= hexagonScl;
        hexagonsX = (int) (gridWth / hexagonWth);
        hexagonsY = (int) (gridHgt / hexagonHgt);
        ground = this.transform.GetChild (0);
        ground.localScale = new Vector3 (gridWth / 2, 1, gridHgt / 2);

        AddGap ();
        StartPosition ();
        CreateGrid ();
        AssignNeighbours ();
    }


    // A wired cube is drawn to see the area where the grid will be applied.
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
        float z = +0.75f * (gridHgt / 2);
        //float x = -hexagonWth * (gridWth / 2) - offset;
        //float z = +hexagonHgt * 0.75f * (gridHgt / 2);

        startPos = new Vector3 (x, 0.1f, z);
        limitX1 = startPos.x - hexagonWth / 2;
        limitZ1 = startPos.z + hexagonHgt / 2;
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
        hexagons = new Hexagon[hexagonsX + 1, hexagonsY + 1];

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
                if (x == hexagonsX && y == hexagonsY) 
                {
                    limitX2 = hexagon.position.x + hexagonWth / 2;
                    limitZ2 = hexagon.position.z - hexagonHgt / 2;
                }
            }
        }
        
        //Creación del mapa, es en esta función pq no se puede pasar por referencia un Hexagons[,]
        //Capital azul, falta la linea que indique que es del jugador
        hexagons[14, 0].SetVisible (true);
        hexagons[14, 0].SetIsBuilded (true);

        GameObject build1 = Instantiate (Capital, new Vector3 (hexagons[14, 0].CentroHexagono.position.x, hexagons[14, 0].CentroHexagono.position.y, hexagons[14, 0].CentroHexagono.position.z), Quaternion.identity);

        hexagons[14, 0].environment = build1;

        hexagons[14, 0].SetCity (build1.GetComponent<City> ());
        hexagons[14, 0].GetCity().SetCityType ("Capital");
        hexagons[14, 0].GetCity().SetCitySide ("Blue");

        hexagons[14, 0].environment.tag = "BlueCapital";

        hexagons[14, 1].SetVisible (true);
        hexagons[13, 0].SetVisible (true);
        hexagons[13, 1].SetVisible (true);
        hexagons[15, 0].SetVisible (true);

        //Capital roja
        hexagons[14, 48].SetVisible (true);
        hexagons[14, 48].SetIsBuilded (true);

        GameObject build2 = Instantiate (Capital, new Vector3 (hexagons[14, 48].CentroHexagono.position.x, hexagons[14, 48].CentroHexagono.position.y, hexagons[14, 48].CentroHexagono.position.z), Quaternion.identity);

        hexagons[14, 48].environment = build2;

        hexagons[14, 48].SetCity (build2.GetComponent<City> ());
        hexagons[14, 48].GetCity().SetCityType ("Capital");
        hexagons[14, 48].GetCity().SetCitySide ("Red");

        hexagons[14, 48].environment.tag = "RedCapital";

        hexagons[13, 47].SetVisible (true);
        hexagons[13, 48].SetVisible (true);
        hexagons[15, 48].SetVisible (true);
        hexagons[14, 47].SetVisible (true);

        //Generación de rio
        for (int i = 0; i <= hexagonsX; i += 1)
        {
            hexagons[i, 24].SetHexagonType (-2);
            hexagons[i, 24].SetVisible (true);
        }

        //Generación de puentes
        hexagons[4, 24].SetHexagonType (+1);
        hexagons[11, 24].SetHexagonType (+1);
        hexagons[18, 24].SetHexagonType (+1);
        hexagons[25, 24].SetHexagonType (+1);
        hexagons[4, 24].SetVisible (false);
        hexagons[11, 24].SetVisible (false);
        hexagons[18, 24].SetVisible (false);
        hexagons[25, 24].SetVisible (false);

        //Generación de montañas
        hexagons[9, 0].SetHexagonType (-1);
        hexagons[8, 1].SetHexagonType (-1);
        hexagons[0, 2].SetHexagonType (-1);
        hexagons[0, 3].SetHexagonType (-1);
        hexagons[1, 3].SetHexagonType (-1);
        hexagons[2, 4].SetHexagonType (-1);
        hexagons[14, 4].SetHexagonType (-1);
        hexagons[18, 4].SetHexagonType (-1);
        hexagons[19, 4].SetHexagonType (-1);
        hexagons[20, 4].SetHexagonType (-1);
        hexagons[21, 4].SetHexagonType (-1);

        hexagons[2, 5].SetHexagonType (-1);
        hexagons[18, 5].SetHexagonType (-1);
        hexagons[19, 5].SetHexagonType (-1);
        hexagons[24, 5].SetHexagonType (-1);
        hexagons[25, 5].SetHexagonType (-1);


        hexagons[9, 0].SetVisible (true);
        hexagons[9, 0].SetMountain (true);
        hexagons[8, 1].SetVisible (true);
        hexagons[8, 1].SetMountain (true);
        hexagons[0, 2].SetVisible (true);
        hexagons[0, 2].SetMountain (true);
        hexagons[0, 3].SetVisible (true);
        hexagons[0, 3].SetMountain (true);
        hexagons[1, 3].SetVisible (true);
        hexagons[1, 3].SetMountain (true);
        hexagons[2, 4].SetVisible (true);
        hexagons[2, 4].SetMountain (true);
        hexagons[14, 4].SetVisible (true);
        hexagons[14, 4].SetMountain (true);
        hexagons[18, 4].SetVisible (true);
        hexagons[18, 4].SetMountain (true);
        hexagons[19, 4].SetVisible (true);
        hexagons[19, 4].SetMountain (true);
        hexagons[20, 4].SetVisible (true);
        hexagons[20, 4].SetMountain (true);
        hexagons[21, 4].SetVisible (true);
        hexagons[21, 4].SetMountain (true);
        hexagons[2, 5].SetVisible (true);
        hexagons[2, 5].SetMountain (true);
        hexagons[18, 5].SetVisible (true);
        hexagons[18, 5].SetMountain (true);
        hexagons[19, 5].SetVisible (true);
        hexagons[19, 5].SetMountain (true);
        hexagons[24, 5].SetVisible (true);
        hexagons[24, 5].SetMountain (true);
        hexagons[25, 5].SetVisible (true);
        hexagons[25, 5].SetMountain (true);

        /*Generación de bosques
        hexagons[50, 10].SetHexagonType(2);
        hexagons[50, 40].SetHexagonType(2);*/
    }


    // We check every hexagon of the grid and assign its neighbours.
    private void AssignNeighbours ()
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