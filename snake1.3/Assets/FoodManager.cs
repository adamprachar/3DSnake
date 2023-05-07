using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GameObject foodPrefab; // reference to the food prefab

    public void SpawnFood()
    {
        Vector3 position = Vector3.zero;
        bool validPosition = false;
        while (!validPosition)
        {
            position = new Vector3(Random.Range(-8f, 8f) - 1.87f, 2f, Random.Range(-8f, 8f) - 17.175f);
            Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
            validPosition = true;
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Wall") || collider.gameObject.CompareTag("Body"))
                {
                    validPosition = false;
                    break;
                }
            }
        }

        // spawn the new food at the generated position
        GameObject newFood = Instantiate(foodPrefab, position, Quaternion.identity);
        newFood.SetActive(true);



    }

}
    // Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f) + -1.87f, 2f, Random.Range(-8f, 8f) + -17.175f);
    // Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));

