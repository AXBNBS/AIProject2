
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ActionsMenu : MonoBehaviour
{
    public GameObject leftBottomHUD;
    public GameObject actionOneUI;
    public GameObject actionTwoUI;
    public GameObject actionThreeUI;
    public CameraController camera;
    public GameObject settlement, farm, tunnel;

    [SerializeField] private int settlementMin, settlementWod, farmMin, farmWod, tunnelMin, tunnelWod;
    private BuildingMenu buildingMenu;
    private ResourcesHolder resourcesHld;


    // Start is called before the first frame update.
    private void Start ()
    {
        buildingMenu = this.GetComponent<BuildingMenu> ();
        resourcesHld = GameObject.FindObjectOfType<ResourcesHolder> ();
    }


    // Update is called once per frame.
    private void Update ()
    {
        if (camera.GetSelectedUnits () != null)
        {
            if (camera.GetSelectedUnits()[0].stats.occupation == "Worker")
            {
                leftBottomHUD.SetActive(true);
                actionOneUI.GetComponentInChildren<TextMeshProUGUI>().text = "Settlement";
                actionTwoUI.GetComponentInChildren<TextMeshProUGUI>().text = "Farm";
                actionThreeUI.GetComponentInChildren<TextMeshProUGUI>().text = "Tunnel";
            }
        }
        else
        {
            leftBottomHUD.SetActive(false);
        }
    }


    public void Action1 ()
    {
        if (actionOneUI.GetComponentInChildren<TextMeshProUGUI>().text == "Settlement")
        {
            BuildSettlement();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }


    public void Action2 ()
    {
        if (actionTwoUI.GetComponentInChildren<TextMeshProUGUI>().text == "Farm")
        {
            BuildFarm();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }


    public void Action3 ()
    {
        if (actionThreeUI.GetComponentInChildren<TextMeshProUGUI>().text == "Tunnel")
        {
            BuildTunnel();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }


    public void BuildSettlement ()
    {
        if (!camera.GetSelectedUnits()[0].currentHex.GetIsBuilded ())
        {
            for (int i = 0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
            {
                if (camera.GetSelectedUnits()[0].currentHex.neighbours[i] != null && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetIsBuilded () == true)
                {
                    return;
                }
            }

            if (resourcesHld.GetBlueWood () >= settlementWod && resourcesHld.GetBlueMineral () >= settlementMin) 
            {
                resourcesHld.changeWood ("Blue", settlementWod, false);
                resourcesHld.changeMineral ("Blue", settlementMin, false);
                camera.GetSelectedUnits()[0].GetComponent<Builder>().BeginConstruction (settlement, settlementMin, settlementWod);
                camera.SetNullSelectedUnit ();
            }

            /*GameObject build = Instantiate (settlement, new Vector3 (camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);

            camera.GetSelectedUnits()[0].currentHex.SetCity(build.GetComponent<City>());
            camera.GetSelectedUnits()[0].currentHex.GetCity().SetCitySide("Blue");

            GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 3, true);

            Hexagon auxHex = camera.GetSelectedUnits()[0].currentHex;
           
            foreach (Hexagon hex in camera.GetSelectedUnits()[0].currentHex.neighbours)
            {
                if (hex.presentUnt == 0)
                {
                    camera.GetSelectedUnits()[0].FindPathTo (hex);

                    break;
                }
            }

            auxHex.SetIsBuilded (true);*/
        }
    }


    public void BuildFarm ()
    {
        if (!camera.GetSelectedUnits()[0].currentHex.GetIsBuilded ())
        {
            for (int i = 0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
            {
                if (camera.GetSelectedUnits()[0].currentHex.neighbours[i] != null && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetIsBuilded () == true)
                {
                    return;
                }
            }
            if (resourcesHld.GetBlueWood () >= farmWod && resourcesHld.GetBlueMineral () >= farmMin) 
            {
                resourcesHld.changeWood ("Blue", farmWod, false);
                resourcesHld.changeMineral ("Blue", farmMin, false);
                camera.GetSelectedUnits()[0].GetComponent<Builder>().BeginConstruction (farm, farmMin, farmWod);
                camera.SetNullSelectedUnit ();
            }

            /*GameObject build = Instantiate (farm, new Vector3 (camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);
            Hexagon auxHex = camera.GetSelectedUnits()[0].currentHex;

            camera.GetSelectedUnits()[0].currentHex.SetCity (build.GetComponent<City>());
            camera.GetSelectedUnits()[0].currentHex.GetCity().SetCitySide ("Blue");

            foreach (Hexagon hex in camera.GetSelectedUnits()[0].currentHex.neighbours)
            {
                if (hex.presentUnt == 0)
                {
                    camera.GetSelectedUnits()[0].FindPathTo (hex);

                    break;
                }
            }

            auxHex.SetIsBuilded (true);*/
        }
    }


    public void BuildTunnel ()
    {
        for (int i = 0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
        {
            if (camera.GetSelectedUnits()[0].currentHex.neighbours[i] != null && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetMountain() == true && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetHexagonType() == -1)
            {
                //GameObject build = Instantiate(farm, new Vector3(camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);
                //camera.GetSelectedUnits()[0].currentHex.neighbours[i].SetHexagonType(1);
                //GameManager.GetComponent<ResourcesHolder>().changeWood("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededWood(), false);
                //GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededMinerals(), false);
                if (resourcesHld.GetBlueWood () >= tunnelWod && resourcesHld.GetBlueMineral () >= tunnelMin)
                {
                    resourcesHld.changeWood ("Blue", tunnelWod, false);
                    resourcesHld.changeMineral ("Blue", tunnelMin, false);
                    camera.GetSelectedUnits()[0].GetComponent<Builder>().BeginConstruction (tunnel, tunnelMin, tunnelWod);
                    camera.SetNullSelectedUnit ();
                }

                break;
            }
        }
    }
}