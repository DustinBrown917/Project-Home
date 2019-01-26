using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOME
{
    [CreateAssetMenu(fileName = "New Pickup Pool", menuName = "Pickup Pool")]
    public class PickupPool : ScriptableObject
    {
        public List<GameObject> Pickups;

        public Pickup GetRandomPickup()
        {
            return Instantiate(Pickups[UnityEngine.Random.Range(0, Pickups.Count)]).GetComponent<Pickup>();
        }
    }
}

