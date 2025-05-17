using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public string planetName; // Имя планеты

    public bool IsTarget { get; set; }
    public string GetPlanetName()
    {
        return planetName;
    }
}
