using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingInfoMenuUI;
    public GameObject firstPanelUI;
    public GameObject surePanelUI;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI levelInfoText;

    private GameObject GameManager;

    [HideInInspector]
    public Hexagon hex;

    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void readHexagonBuilding(Hexagon n)
    {
        hex = n;
        descriptionText.text = hex.GetCity().GetCityType() + " level " + hex.GetCity().GetLevel();
        infoText.text = "This building has " + hex.GetCity().GetDefense() + " of defense";
        buildingInfoMenuUI.SetActive(true);
    }

    public void CloseWindow()
    {
        buildingInfoMenuUI.SetActive(false);
    }

    public void MovePopulation() 
    {
        //Aquí será el desplazamiento de población
    }

    public void LevelUpButton()
    {
        if (((hex.GetCity().GetCityType() == "Sawmill" || hex.GetCity().GetCityType() == "Farm" || hex.GetCity().GetCityType() == "Mina") && hex.GetCity().GetLevel() < 3) || hex.GetCity().GetLevel() < 5)
        {
            firstPanelUI.SetActive(false);
            levelInfoText.text = "It will cost:" + System.Environment.NewLine + "Wood: " + hex.GetCity().GetNeededWood() + System.Environment.NewLine + "Mineral: " + hex.GetCity().GetNeededMinerals();
            surePanelUI.SetActive(true);
        }
    }

    public void YesButton()
    {
        if (GameManager.GetComponent<ResourcesHolder>().GetBlueWood() >= hex.GetCity().GetNeededWood() && GameManager.GetComponent<ResourcesHolder>().GetBlueMineral() >= hex.GetCity().GetNeededMinerals())
        {
            GameManager.GetComponent<ResourcesHolder>().changeWood("Blue", hex.GetCity().GetNeededWood(), false);
            GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", hex.GetCity().GetNeededMinerals(), false);

            firstPanelUI.SetActive(true);
            surePanelUI.SetActive(false);

            CloseWindow(); //Por ahora así esta bien

            GameObject nextLevel = hex.GetCity().nextLevel;

            Object.Destroy(hex.GetCity().gameObject);
            
            GameObject build = Instantiate(nextLevel, new Vector3(hex.CentroHexagono.position.x, hex.CentroHexagono.position.y, hex.CentroHexagono.position.z), Quaternion.identity);

            hex.SetCity(build.GetComponent<City>());
        }
    }

    public void NoButton()
    {
        firstPanelUI.SetActive(true);
        surePanelUI.SetActive(false);
    }
}
