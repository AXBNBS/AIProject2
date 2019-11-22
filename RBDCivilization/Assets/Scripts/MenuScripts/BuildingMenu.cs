using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingInfoMenuUI;
    public GameObject firstPanelUI;
    public GameObject surePanelUI;
    public GameObject trainPanelUI;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI levelInfoText;

    private GameObject GameManager;

    public GameObject humanPrefab;
    public GameObject catPrefab;
    public GameObject elfPrefab;
    public GameObject dwarfPrefab;
    public GameObject twiiPrefab;
    public GameObject hazelnutPrefab;
    public GameObject nougatPrefab;

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
        if (((hex.GetCity().GetCityType() == "Sawmill" || hex.GetCity().GetCityType() == "Farm" || hex.GetCity().GetCityType() == "Mina" || hex.GetCity().GetCityType() == "Settlement") && hex.GetCity().GetLevel() < 3) || (hex.GetCity().GetLevel() < 5 && hex.GetCity().GetCityType()=="City"))
        {
            firstPanelUI.SetActive(false);
            levelInfoText.text = "It will cost:" + System.Environment.NewLine + "Wood: " + hex.GetCity().GetNeededWood() + System.Environment.NewLine + "Mineral: " + hex.GetCity().GetNeededMinerals();
            surePanelUI.SetActive(true);
        }
    }

    public void TrainButton()
    {
        if (GameManager.GetComponent<ResourcesHolder>().GetBlueCurrentPopulation() < GameManager.GetComponent<ResourcesHolder>().GetBlueTotalPopulation())
        {
            firstPanelUI.SetActive(false);
            trainPanelUI.SetActive(true);
        }
    }

    public void YesButton()
    {
        if (GameManager.GetComponent<ResourcesHolder>().GetBlueWood() >= hex.GetCity().GetNeededWood() && GameManager.GetComponent<ResourcesHolder>().GetBlueMineral() >= hex.GetCity().GetNeededMinerals())
        {
            GameManager.GetComponent<ResourcesHolder>().changeWood("Blue", hex.GetCity().GetNeededWood(), false);
            GameManager.GetComponent<ResourcesHolder>().changeMineral("Blue", hex.GetCity().GetNeededMinerals(), false);
            if (hex.GetCity().GetCityType() == "Sawmill" || hex.GetCity().GetCityType() == "Farm" || hex.GetCity().GetCityType() == "Mina")
            {
                GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 3, true);
            } else
            {
                GameManager.GetComponent<ResourcesHolder>().changeTotalPopulation("Blue", 5, true);
            }

            firstPanelUI.SetActive(true);
            surePanelUI.SetActive(false);

            int humans = hex.GetCity().GetHumans();
            int cats = hex.GetCity().GetCats();
            int elves = hex.GetCity().GetElves();
            int dwarfs = hex.GetCity().GetDwarfs();
            int twiis = hex.GetCity().GetTwiis();
            int craftsmen = hex.GetCity().GetCraftsmen();
            int turroncitos = hex.GetCity().GetTurroncitos();

            print(humans);

            CloseWindow(); //Por ahora así esta bien

            GameObject nextLevel = hex.GetCity().nextLevel;

            Object.Destroy(hex.GetCity().gameObject);
            
            GameObject build = Instantiate(nextLevel, new Vector3(hex.CentroHexagono.position.x, hex.CentroHexagono.position.y, hex.CentroHexagono.position.z), Quaternion.identity);

            hex.SetCity(build.GetComponent<City>());
            hex.GetCity().SetCitySide("Blue");

            hex.GetCity().AddUnits("Human", humans, humans);
            hex.GetCity().AddUnits("Cat", cats, cats * 1.5f);
            hex.GetCity().AddUnits("Elf", elves, elves);
            hex.GetCity().AddUnits("Dwarf", dwarfs, dwarfs);
            hex.GetCity().AddUnits("Twii", twiis, twiis*2);
            hex.GetCity().AddUnits("Craftsman", craftsmen, craftsmen*0.5f);
            hex.GetCity().AddUnits("Turroncito", turroncitos, turroncitos);
        }
    }

    public void NoButton()
    {
        firstPanelUI.SetActive(true);
        surePanelUI.SetActive(false);
    }

    public void BackButton()
    {
        firstPanelUI.SetActive(true);
        trainPanelUI.SetActive(false);
    }

    public void TrainHuman()
    {
        if (humanPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(humanPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainCat()
    {
        if (catPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(catPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainElf()
    {
        if (elfPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(elfPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainDwarf()
    {
        if (dwarfPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(dwarfPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainTwii()
    {
        if (twiiPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(twiiPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainHazelnut()
    {
        if (hazelnutPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(hazelnutPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void TrainNougat()
    {
        if (nougatPrefab.GetComponent<Unit>().GetStores() <= GameManager.GetComponent<ResourcesHolder>().GetBlueStores())
        {
            Hexagon generate = null;
            for (int i = 0; i < hex.neighbours.Length; i++)
            {
                if (hex.neighbours[i] != null && hex.neighbours[i].presentUnt == 0)
                {
                    generate = hex.neighbours[i];
                    break;
                }
            }

            if (generate != null)
            {
                GameObject train = Instantiate(nougatPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }
}
