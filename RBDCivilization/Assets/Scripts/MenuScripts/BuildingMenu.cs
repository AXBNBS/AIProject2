using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingInfoMenuUI;
    public GameObject firstPanelUI;
    public GameObject surePanelUI;

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
        firstPanelUI.SetActive(false);
        surePanelUI.SetActive(true);
    }

    public void YesButton()
    {
        firstPanelUI.SetActive(true);
        surePanelUI.SetActive(false);
        //Aquí hacer la función de subir de nivel y tal
    }

    public void NoButton()
    {
        firstPanelUI.SetActive(true);
        surePanelUI.SetActive(false);
    }
}
