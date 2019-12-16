using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnityMenu : MonoBehaviour
{
    public GameObject unityInfoMenuUI;
    public GameObject panelUI;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI infoText;

    public CameraController cameraController;

    public void readHexagonUnity(Hexagon n)
    {
        UnitMovement[] units = n.GetUnits();
        int length = 0;
        for (int i = 0; i < 5; i++)
        {
            if (units[i] != null)
            {
                length++;
            } else
            {
                break;
            }
        }

        if (length != 0) 
        {
            string firstStr = "There are " + length + " " + units[0].stats.occupation.ToLower () + "s in this cell.";

            
            if (units[0].stats.race == "Turroncito")
            {
                descriptionText.text = "Lil nougat";
            }
            else 
            {
                descriptionText.text = units[0].stats.race;
            }
            if (length == 1)
            {
                firstStr = "There is 1 " + units[0].stats.occupation.ToLower () + " in this cell.";
            }
            infoText.text = firstStr + System.Environment.NewLine +
                "Movement: " + units[0].GetMovementLimit () + "/" + units[0].stats.speed + "." + System.Environment.NewLine + "Total attack power: " + units[0].stats.attack * length + "." +
                System.Environment.NewLine + "Total defense points: " + units[0].stats.defense * length + ".";
            unityInfoMenuUI.SetActive (true);
        }
    }

    public void CloseWindow()
    {
        cameraController.SetNullSelectedUnit(); //Desselecionas la unidad
        unityInfoMenuUI.SetActive(false);
    }
}
