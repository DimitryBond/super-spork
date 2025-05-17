using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "TargetPlanetData", menuName = "Game/TargetPlanetData")]
    public class TargetPlanetData : ScriptableObject
    {
        [Serializable]
        public class RoundTarget
        {
            public string targetPlanetName; 
        }

        [SerializeField] private List<RoundTarget> roundTargets;

        public string GetTargetPlanetName(int round)
        {
            if (round < 0 || round >= roundTargets.Count)
            {
                return null;
            }

            return roundTargets[round].targetPlanetName;
        }

        public int TotalRounds => roundTargets.Count;
    }
}