﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingMenu : MonoBehaviour
{
    public GameObject buildingInfoMenuUI;
    public GameObject firstPanelUI;
    public GameObject surePanelUI;
    public GameObject trainPanelUI;
    public GameObject movePanelUI;

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI levelInfoText;

    private GameManager GameManager;

    public GameObject humanPrefab;
    public UnitSettings humanSettings;
    public GameObject catPrefab;
    public UnitSettings catSettings;
    public GameObject elfPrefab;
    public UnitSettings elfSettings;
    public GameObject dwarfPrefab;
    public UnitSettings dwarfSettings;
    public GameObject twiiPrefab;
    public UnitSettings twiiSettings;
    public GameObject hazelnutPrefab;
    public UnitSettings hazelnutSettings;
    public GameObject nougatPrefab;
    public UnitSettings nougatSettings;

    [HideInInspector]
    public Hexagon hex;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
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
        City c = hex.GetCity();
        if (c.GetHumans() != 0 || c.GetElves() != 0 || c.GetDwarfs() != 0 || c.GetCats() != 0 || c.GetTwiis() != 0 || c.GetTurroncitos() != 0 || c.GetCraftsmen() != 0)
        {
            firstPanelUI.SetActive(false);
            movePanelUI.SetActive(true);
        }
    }

    public void LevelUpButton()
    {
        if ((hex.GetCity().GetCityType() == "Settlement" && hex.GetCity().GetLevel() < 3) || (hex.GetCity().GetLevel() < 5 && hex.GetCity().GetCityType()=="Capital"))
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
            if (hex.GetCity().GetCityType() == "Sawmill" || hex.GetCity().GetCityType() == "Farm" || hex.GetCity().GetCityType() == "Mina" || hex.GetCity().GetCityType() == "Settlement")
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

            CloseWindow(); //Por ahora así esta bien

            string type = hex.GetCity().GetCityType();

            GameObject nextLevel = hex.GetCity().nextLevel;

            Destroy(hex.environment);
            
            hex.environment = Instantiate(nextLevel, new Vector3(hex.CentroHexagono.position.x, hex.CentroHexagono.position.y, hex.CentroHexagono.position.z), Quaternion.identity);

            hex.SetCity(hex.environment.GetComponent<City>());
            hex.GetCity().SetCityType(type);
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
        movePanelUI.SetActive(false);
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
                if (train.GetComponent<UnitMovement>().stats.occupation == "Worker")
                {
                    gameManager.AIBld.Remove(train.GetComponent<Builder>());
                }
                else if (train.GetComponent<UnitMovement>().stats.occupation == "Collector")
                {
                    gameManager.AICll.Remove(train.GetComponent<Collector>());
                }
                gameManager.AIUnt.Remove(train.GetComponent<UnitMovement>());
                GameManager.GetComponent<ResourcesHolder>().changeCurrentPopulation("Blue", 1, true);
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", humanSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", catSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", elfSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", dwarfSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", twiiSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", hazelnutSettings.stores, false);

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
                GameManager.GetComponent<ResourcesHolder>().changeStores("Blue", nougatSettings.stores, false);

                firstPanelUI.SetActive(true);
                trainPanelUI.SetActive(false);
            }
        }
    }

    public void MoveHuman()
    {
        if (hex.GetCity().GetHumans() != 0)
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
                hex.GetCity().AddUnits(humanSettings.race, -1, -humanSettings.defense);
                GameObject train = Instantiate(humanPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveCat()
    {
        if (hex.GetCity().GetCats() != 0)
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
                hex.GetCity().AddUnits(catSettings.race, -1, -catSettings.defense);
                GameObject train = Instantiate(catPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveElf()
    {
        if (hex.GetCity().GetElves() != 0)
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
                hex.GetCity().AddUnits(elfSettings.race, -1, -elfSettings.defense);
                GameObject train = Instantiate(elfPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveDwarf()
    {
        if (hex.GetCity().GetDwarfs() != 0)
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
                hex.GetCity().AddUnits(dwarfSettings.race, -1, -dwarfSettings.defense);
                GameObject train = Instantiate(dwarfPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveTwii()
    {
        if (hex.GetCity().GetTwiis() != 0)
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
                hex.GetCity().AddUnits(twiiSettings.race, -1, -twiiSettings.defense);
                GameObject train = Instantiate(twiiPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveHazelnut()
    {
        if (hex.GetCity().GetCraftsmen() != 0)
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
                hex.GetCity().AddUnits(hazelnutSettings.race, -1, -hazelnutSettings.defense);
                GameObject train = Instantiate(hazelnutPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }

    public void MoveNougat()
    {
        if (hex.GetCity().GetTurroncitos() != 0)
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
                hex.GetCity().AddUnits(nougatSettings.race, -1, -nougatSettings.defense);
                GameObject train = Instantiate(nougatPrefab, new Vector3(generate.CentroHexagono.position.x, generate.CentroHexagono.position.y, generate.CentroHexagono.position.z), Quaternion.identity);

                firstPanelUI.SetActive(true);
                movePanelUI.SetActive(false);
            }
        }
    }
}
