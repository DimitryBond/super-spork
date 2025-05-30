using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public string PlanetName;
    public string PlanetTargetDescription;

    public bool IsTarget { get; set; }

    public void TriggerDestroy()
    {
        animator.SetTrigger("Destroy");
    }

    public void TriggerEnd()
    {
        Destroy(gameObject);
    }
}
