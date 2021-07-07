using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    [CreateAssetMenu(menuName = "Red Owl/Locations/Flow", fileName = "Flow")]
    public class LocationFlow : RedOwlScriptableObject
    {
        public Location[] Flow;

        public Location[] Others;

        public Location this[int index] => Flow[index];

        public IEnumerable<Location> AllLocations
        {
            get
            {
                foreach (var location in Flow)
                {
                    yield return location;
                }

                foreach (var location in Others)
                {
                    yield return location;
                }
            }
        }
    }
}