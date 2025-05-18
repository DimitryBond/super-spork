using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    public string PlanetName;
    public string PlanetTargetDescription;

    public bool IsTarget { get; set; }

    public void TriggerDestroy()
    {
        Destroy(gameObject);
    }
}
