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

    public GameObject settlement;
    public GameObject farm;
    private BuildingMenu buildingMenu;
    private GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        buildingMenu=this.GetComponent<BuildingMenu>();
        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.GetSelectedUnits() != null)
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

    public void Action1()
    {
        if (actionOneUI.GetComponentInChildren<TextMeshProUGUI>().text == "Settlement")
        {
            BuildSettlement();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }

    public void Action2()
    {
        if (actionTwoUI.GetComponentInChildren<TextMeshProUGUI>().text == "Farm")
        {
            BuildFarm();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }

    public void Action3()
    {
        if (actionThreeUI.GetComponentInChildren<TextMeshProUGUI>().text == "Tunnel")
        {
            BuildTunnel();
        }
        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
    }

    public void BuildSettlement()
    {
        if (!camera.GetSelectedUnits()[0].currentHex.GetIsBuilded())
        {
            for(int i =0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
            {
                if(camera.GetSelectedUnits()[0].currentHex.neighbours[i]!=null && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetIsBuilded())
                {
                    return;
                }
            }
            GameObject build = Instantiate(settlement, new Vector3(camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);

            camera.GetSelectedUnits()[0].currentHex.SetCity(build.GetComponent<City>());
            camera.GetSelectedUnits()[0].currentHex.GetCity().SetCitySide("Blue");

            GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 3, true);

            camera.GetSelectedUnits()[0].currentHex.SetIsBuilded(true);

            camera.SetNullSelectedUnit();

            GameManager.GetComponent<ResourcesHolder>().changeWood("Blue",50, false);
            GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", 50, false);
        }
    }

    public void BuildFarm()
    {
        if (!camera.GetSelectedUnits()[0].currentHex.GetIsBuilded())
        {
            for (int i = 0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
            {
                if (camera.GetSelectedUnits()[0].currentHex.neighbours[i] != null && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetIsBuilded())
                {
                    return;
                }
            }
            GameObject build = Instantiate(farm, new Vector3(camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);

            camera.GetSelectedUnits()[0].currentHex.SetCity(build.GetComponent<City>());
            camera.GetSelectedUnits()[0].currentHex.GetCity().SetCitySide("Blue");
            camera.GetSelectedUnits()[0].currentHex.SetIsBuilded(true);

            GameManager.GetComponent<ResourcesHolder>().changeWood("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededWood(), false);
            GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededMinerals(), false);

            camera.SetNullSelectedUnit();
        }
    }

    public void BuildTunnel()
    {
        for (int i = 0; i < camera.GetSelectedUnits()[0].currentHex.neighbours.Length; i++)
        {
            if (camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetMountain() == true && camera.GetSelectedUnits()[0].currentHex.neighbours[i].GetHexagonType()==-1)
            {
                //GameObject build = Instantiate(farm, new Vector3(camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);

                camera.GetSelectedUnits()[0].currentHex.neighbours[i].SetHexagonType(1);

                //GameManager.GetComponent<ResourcesHolder>().changeWood("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededWood(), false);
                //GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", camera.GetSelectedUnits()[0].currentHex.GetCity().GetNeededMinerals(), false);

                camera.SetNullSelectedUnit();
                break;
            }
        }
    }
}
