
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;



public class Grid : MonoBehaviour
{
    public static Grid instance;
    public float hexagonWth, hexagonHgt, limitX1, limitX2, limitZ1, limitZ2;
    public int gridWth, gridHgt;
    public Hexagon[,] hexagons;
    public Hexagon[] forestsArray, unwalkable;
    public GameObject Capital; //Generar las dos capitales

    public Material materialPradera1;
    public Material materialPradera2;
    public Material materialPradera3;
    public Material materialPradera4;

    [SerializeField] private Transform hexagonPfb;
    //[SerializeField] private int gridWth, gridHgt;
    [SerializeField] private float gap, hexagonScl;
    private Vector3 startPos;
    public int hexagonsX, hexagonsY, forests;
    private Transform ground;
    private int[] forestHex1, forestHex2;


    // We initialize some variables and add the gap to the hexagons's with and height, we calculate the starting position of the grid, and we finally create it.
    private void Awake ()
    {
        instance = this;
        hexagonWth *= hexagonScl;
        hexagonHgt *= hexagonScl;
        hexagonsX = (int) (gridWth / hexagonWth);
        hexagonsY = (int) (gridHgt / hexagonHgt);
        unwalkable = new Hexagon[hexagonsX - 3];
        ground = this.transform.GetChild (0);
        ground.localScale = new Vector3 (gridWth / 2, 1, gridHgt / 2);
        forestHex1 = new int[] {1, 30, 262, 292, 408, 61, 235, 264, 409, 554, 4, 526, 555, 5, 92, 121, 122, 586, 615, 355, 384, 356, 154, 184, 532, 561, 69, 98, 331, 360, 275, 247, 450, 451, 104, 76, 105, 279, 251, 50, 311, 51, 80, 167, 312, 457, 
            197, 458, 487, 603, 632, 403, 432};
        forestHex2 = new int[] {1393, 1364, 1132, 1104, 988, 1337, 1163, 1134, 989, 844, 1396, 874, 845, 1397, 1310, 1281, 1282, 818, 789, 1051, 1022, 1052, 1256, 1228, 880, 851, 1345, 1316, 1085, 1056, 1145, 1175, 972, 973, 1322, 1352, 1323, 1149, 
            1179, 1384, 1123, 1385, 1356, 1269, 1124, 979, 1241, 980, 951, 835, 806, 1041, 1012};
        forests = (int) (forestHex1.Length / 2);
        forestsArray = new Hexagon[forests * 2];

        AddGap ();
        StartPosition ();
        CreateGrid ();
        AssignNeighbours ();
        GenerateForests ();
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
        int hexagonCnt = 1, riverIdx = 0;
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
                hexagon.name = hexagonCnt.ToString ();
                hexagonCnt += 1;
                hexagons[x, y] = hexagon.GetComponent<Hexagon> ();
                hexagons[x, y].SetHexagonType (+1);
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

        // Generación de rio
        for (int i = 0; i <= hexagonsX; i += 1)
        {
            if (i == 4 || i == 11 || i == 18 || i == 25)
            {
                hexagons[i, 24].SetHexagonType (+1);
                hexagons[i, 24].SetVisible (false);
            }
            else 
            {
                hexagons[i, 24].SetHexagonType (-2);
                hexagons[i, 24].SetVisible (true);

                unwalkable[riverIdx] = hexagons[i, 24];
                riverIdx += 1;
            }
        }

        // Generación de montañas
        hexagons[9, 0].SetMountain (true);
        hexagons[8, 1].SetMountain (true);
        hexagons[0, 2].SetMountain (true);
        hexagons[0, 3].SetMountain (true);
        hexagons[1, 3].SetMountain (true);
        hexagons[2, 4].SetMountain (true);
        hexagons[14, 4].SetMountain (true);
        hexagons[18, 4].SetMountain (true);
        hexagons[19, 4].SetMountain (true);
        hexagons[20, 4].SetMountain (true);
        hexagons[21, 4].SetMountain (true);
        hexagons[2, 5].SetMountain (true);
        hexagons[18, 5].SetMountain (true);
        hexagons[19, 5].SetMountain (true);
        hexagons[24, 5].SetMountain (true);
        hexagons[25, 5].SetMountain (true);
        hexagons[2, 6].SetMountain (true);
        hexagons[25, 6].SetMountain (true);
        hexagons[26, 6].SetMountain (true);
        hexagons[1, 7].SetMountain (true);
        hexagons[2, 7].SetMountain (true);
        hexagons[25, 7].SetMountain (true);
        hexagons[25, 8].SetMountain (true);
        hexagons[26, 8].SetMountain (true);
        hexagons[23, 9].SetMountain (true);
        hexagons[24, 9].SetMountain (true);
        hexagons[20, 12].SetMountain (true);
        hexagons[22, 12].SetMountain (true);
        hexagons[19, 13].SetMountain (true);
        hexagons[20, 13].SetMountain (true);
        hexagons[21, 13].SetMountain (true);
        hexagons[16, 14].SetMountain (true);
        hexagons[9, 15].SetMountain (true);
        hexagons[15, 15].SetMountain (true);
        hexagons[16, 15].SetMountain (true);
        hexagons[9, 16].SetMountain (true);
        hexagons[10, 16].SetMountain (true);
        hexagons[11, 16].SetMountain (true);
        hexagons[12, 16].SetMountain (true);
        hexagons[13, 16].SetMountain (true);
        hexagons[15, 16].SetMountain (true);
        hexagons[11, 17].SetMountain (true);
        hexagons[13, 17].SetMountain (true);
        hexagons[14, 17].SetMountain (true);
        hexagons[20, 17].SetMountain (true);
        hexagons[21, 17].SetMountain (true);
        hexagons[22, 17].SetMountain (true);
        hexagons[23, 17].SetMountain (true);
        hexagons[18, 18].SetMountain (true);
        hexagons[22, 18].SetMountain (true);
        hexagons[6, 21].SetMountain (true);
        hexagons[15, 21].SetMountain (true);
        hexagons[5, 22].SetMountain (true);
        hexagons[6, 22].SetMountain (true);
        hexagons[4, 23].SetMountain (true);
        hexagons[4, 25].SetMountain (true);
        hexagons[5, 26].SetMountain (true);
        hexagons[6, 26].SetMountain (true);
        hexagons[6, 27].SetMountain (true);
        hexagons[15, 27].SetMountain (true);
        hexagons[18, 30].SetMountain (true);
        hexagons[22, 30].SetMountain (true);
        hexagons[11, 31].SetMountain (true);
        hexagons[13, 31].SetMountain (true);
        hexagons[14, 31].SetMountain (true);
        hexagons[20, 31].SetMountain (true);
        hexagons[21, 31].SetMountain (true);
        hexagons[22, 31].SetMountain (true);
        hexagons[23, 31].SetMountain (true);
        hexagons[9, 32].SetMountain (true);
        hexagons[10, 32].SetMountain (true);
        hexagons[11, 32].SetMountain (true);
        hexagons[12, 32].SetMountain (true);
        hexagons[13, 32].SetMountain (true);
        hexagons[15, 32].SetMountain (true);
        hexagons[9, 33].SetMountain (true);
        hexagons[15, 33].SetMountain (true);
        hexagons[16, 33].SetMountain (true);
        hexagons[16, 34].SetMountain (true);
        hexagons[19, 35].SetMountain (true);
        hexagons[20, 35].SetMountain (true);
        hexagons[21, 35].SetMountain (true);
        hexagons[20, 36].SetMountain (true);
        hexagons[22, 36].SetMountain (true);
        hexagons[23, 39].SetMountain (true);
        hexagons[24, 39].SetMountain (true);
        hexagons[25, 40].SetMountain (true);
        hexagons[26, 40].SetMountain (true);
        hexagons[1, 41].SetMountain (true);
        hexagons[2, 41].SetMountain (true);
        hexagons[25, 41].SetMountain (true);
        hexagons[2, 42].SetMountain (true);
        hexagons[25, 42].SetMountain (true);
        hexagons[26, 42].SetMountain (true);
        hexagons[2, 43].SetMountain (true);
        hexagons[18, 43].SetMountain (true);
        hexagons[19, 43].SetMountain (true);
        hexagons[24, 43].SetMountain (true);
        hexagons[25, 43].SetMountain (true);
        hexagons[2, 44].SetMountain (true);
        hexagons[14, 44].SetMountain (true);
        hexagons[18, 44].SetMountain (true);
        hexagons[19, 44].SetMountain (true);
        hexagons[20, 44].SetMountain (true);
        hexagons[21, 44].SetMountain (true);
        hexagons[0, 45].SetMountain (true);
        hexagons[1, 45].SetMountain (true);
        hexagons[0, 46].SetMountain (true);
        hexagons[8, 47].SetMountain (true);
        hexagons[9, 48].SetMountain (true);

        //Lake generation
        hexagons[1, 1].SetHexagonType (0);
        hexagons[1, 47].SetHexagonType (0);
        hexagons[4, 12].SetHexagonType (0);
        hexagons[4, 36].SetHexagonType (0);
        hexagons[12, 6].SetHexagonType (0);
        hexagons[12, 42].SetHexagonType (0);
        hexagons[12, 22].SetHexagonType (0);
        hexagons[12, 26].SetHexagonType (0);
        hexagons[16, 13].SetHexagonType (0);
        hexagons[16, 35].SetHexagonType (0);
        hexagons[20, 6].SetHexagonType (0);
        hexagons[20, 42].SetHexagonType (0);
        hexagons[27, 14].SetHexagonType (0);
        hexagons[27, 34].SetHexagonType (0);
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


    // Forests can appear in some cells, we decide randomly wich of these cells will contain a forest in the end. We also make sure each faction has a minimum number of forests on their side of the map.
    private void GenerateForests () 
    {
        int index = 0, activeFrs = 0, arrayIndex = 0;

        while (activeFrs != forests) 
        {
            for (int x = 0; x <= hexagonsX; x += 1) 
            {
                for (int y = 0; y <= hexagonsY / 2; y += 1) 
                {
                    if (hexagons[x, y].name == forestHex1[index].ToString ())
                    {
                        if (Random.Range (0, 2) == 1)
                        {
                            hexagons[x, y].SetHexagonType (+2);
                            forestsArray[arrayIndex] = hexagons[x, y];
                            arrayIndex++;

                            activeFrs += 1;

                            if (activeFrs == forests)
                            {
                                break;
                            }
                        }

                        index += 1;

                        if (index == forestHex1.Length)
                        {
                            break;
                        }
                    }
                }

                if (activeFrs == forests || index == forestHex1.Length) 
                {
                    break;
                }
            }

            index = 0;
        }
        activeFrs = 0;

        while (activeFrs != forests)
        {
            for (int x = 0; x <= hexagonsX; x += 1)
            {
                for (int y = hexagonsY; y >= hexagonsY / 2; y -= 1)
                {
                    if (hexagons[x, y].name == forestHex2[index].ToString ())
                    {
                        if (Random.Range (0, 2) == 1)
                        {
                            hexagons[x, y].SetHexagonType (+2);
                            forestsArray[arrayIndex] = hexagons[x, y];
                            arrayIndex++;

                            activeFrs += 1;

                            if (activeFrs == forests)
                            {
                                break;
                            }
                        }

                        index += 1;

                        if (index == forestHex1.Length)
                        {
                            break;
                        }
                    }
                }

                if (activeFrs == forests || index == forestHex1.Length)
                {
                    break;
                }
            }

            index = 0;
        }
    }
}