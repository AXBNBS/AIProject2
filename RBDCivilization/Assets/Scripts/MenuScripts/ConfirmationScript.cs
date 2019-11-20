using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationScript : MonoBehaviour
{
    public GameObject confirmationPanelUI;

    private UnitMovement unit;

    public void askCombat(UnitMovement n)
    {
        confirmationPanelUI.SetActive(true);
        unit = n;
    }

    public void YesButton()
    {
        unit.LookForCombat();
        confirmationPanelUI.SetActive(false);
    }

    public void NoButton()
    {
        confirmationPanelUI.SetActive(false);
    }
}
