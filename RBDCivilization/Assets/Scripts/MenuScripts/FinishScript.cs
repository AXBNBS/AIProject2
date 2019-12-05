using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishScript : MonoBehaviour
{
    public GameObject finishPanelUI;
    public TextMeshProUGUI finishText;
    
    public void finishMatch(string team)
    {
        if (team == "Blue" || team == "blue")
        {
            finishText.text = "Defeat";
        } else
        {
            finishText.text = "Victory";
        }

        finishPanelUI.SetActive(true);
    }

    public void continueButton()
    {
        finishPanelUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
