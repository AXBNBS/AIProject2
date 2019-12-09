﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesHolder : MonoBehaviour
{
    //Recursos del jugador
    private int blueWood;
    private int blueMineral;
    private int blueStores;
    private int blueTotalPopulation;
    private int blueCurrentPopulation;

    //Recursos de la IA
    private int redWood;
    private int redMineral;
    private int redStores;
    private int redTotalPopulation;
    private int redCurrentPopulation;

    //Referencias para mostrar en el HUD los minerales
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI mineralText;
    public TextMeshProUGUI storesText;
    public TextMeshProUGUI populationText;

    void Awake()
    {
        blueWood = 1000;
        blueMineral = 1000;
        blueStores = 50;
        blueTotalPopulation = 5;
        redCurrentPopulation = 0; //Esto se deberá de cambiar cuando sepamos con que unidades se empieza en cada bando

        redWood = 1000;
        redMineral = 1000;
        redStores = 50;
        redTotalPopulation = 5;
        redCurrentPopulation = 0; //Esto se deberá de cambiar cuando sepamos con que unidades se empieza en cada bando

        woodText.text = blueWood.ToString();
        mineralText.text = blueMineral.ToString();
        storesText.text = blueStores.ToString();
        populationText.text = blueCurrentPopulation.ToString() + " / " + blueTotalPopulation.ToString();
    }

    //Función para cambiar la cantidad de madera del jugador o la IA
    public void changeWood(string team, int n, bool add)
    {
        if (team == "Blue" || team == "blue")
        {
            if (add)
                blueWood += n;
            else
                blueWood -= n;
            
            woodText.text = blueWood.ToString();
        } else if (team == "Red" || team == "red")
        {
            if (add)
                redWood += n;
            else
                redWood -= n;
        }
    }

    //Función para cambiar la cantidad de mineral del jugador o la IA
    public void changeMineral(string team, int n, bool add)
    {
        if (team == "Blue" || team == "blue")
        {
            if (add)
                blueMineral += n;
            else
                blueMineral -= n;

            mineralText.text = blueMineral.ToString();
        }
        else if (team == "Red" || team == "red")
        {
            if (add)
                redMineral += n;
            else
                redMineral -= n;
        }
    }

    //Función para cambiar la cantidad de viveres del jugador o la IA
    public void changeStores(string team, int n, bool add)
    {
        if (team == "Blue" || team == "blue")
        {
            if (add)
                blueStores += n;
            else
                blueStores -= n;
            
            storesText.text = blueStores.ToString();
        }
        else if (team == "Red" || team == "red")
        {
            if (add)
                redStores += n;
            else
                redStores -= n;
        }
    }

    //Función para cambiar la cantidad de población total del jugador o la IA
    public void changeTotalPopulation(string team, int n, bool add)
    {
        if (team == "Blue" || team == "blue")
        {
            if (add)
                blueTotalPopulation += n;
            else
                blueTotalPopulation -= n;
            
            populationText.text = blueCurrentPopulation.ToString() + " / " + blueTotalPopulation.ToString();
        }
        else if (team == "Red" || team == "red")
        {
            if (add)
                redTotalPopulation += n;
            else
                redTotalPopulation -= n;
        }
    }

    //Función para cambiar la cantidad de población gastada del jugador o la IA
    public void changeCurrentPopulation(string team, int n, bool add)
    {
        if (team == "Blue" || team == "blue")
        {
            if (add)
                blueCurrentPopulation += n;
            else
                blueCurrentPopulation -= n;

            populationText.text = blueCurrentPopulation.ToString() + " / " + blueTotalPopulation.ToString();
        }
        else if (team == "Red" || team == "red")
        {
            if (add)
                redCurrentPopulation += n;
            else
                redCurrentPopulation -= n;
        }
    }

    public int GetBlueWood()
    {
        return blueWood;
    }

    public int GetBlueMineral()
    {
        return blueMineral;
    }

    public int GetBlueStores()
    {
        return blueStores;
    }

    public int GetBlueTotalPopulation()
    {
        return blueTotalPopulation;
    }

    public int GetBlueCurrentPopulation()
    {
        return blueCurrentPopulation;
    }

    public int GetRedWood()
    {
        return redWood;
    }

    public int GetRedMineral()
    {
        return redMineral;
    }

    public int GetRedStores()
    {
        return redStores;
    }

    public int GetRedTotalPopulation()
    {
        return redTotalPopulation;
    }

    public int GetRedCurrentPopulation()
    {
        return redCurrentPopulation;
    }
}
