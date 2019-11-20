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

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (!camera.GetSelectedUnits()[0].currentHex.GetIsBuilded())
            {
                GameObject build = Instantiate(settlement, new Vector3(camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.x, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.y, camera.GetSelectedUnits()[0].currentHex.CentroHexagono.position.z), Quaternion.identity);

                camera.GetSelectedUnits()[0].currentHex.SetCity(build.GetComponent<City>());
            }
        }
        camera.SetNullSelectedUnit();
    }
}
