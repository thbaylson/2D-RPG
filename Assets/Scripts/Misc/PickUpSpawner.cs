using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickup;

    public void SpawnItems()
    {
        Instantiate(pickup, transform.position, Quaternion.identity);
    }
}
