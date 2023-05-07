using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 1f; // rychlost hada
    public float rotationSpeed = 1f; // rychlost rotace hada
    private Vector3 direction = Vector3.forward; // směr, kterým se had pohybuje
    private string wallTag = "Wall";
    private string bodyTag = "Body";
    private bool canChangeDirection = true; // určuje, zda může had změnit směr
    private Quaternion lastRotation; // definice lastRotation
    public GameObject snakeBodyPrefab; // prefab pro tělo hada
    private List<Transform> snakeBodyParts = new List<Transform>(); // seznam částí hada
    private List<Vector3> PositionsHistory = new List<Vector3>();
    public int Gap = 10;
    public FoodManager foodManager;

    void Start()
    {
        // Přidání první části hada
        snakeBodyParts.Add(transform);
        lastRotation = transform.rotation; // inicializace lastRotation

        // Skrytí krychle s jídlem
        // GameObject.FindGameObjectWithTag("Food").SetActive(false);

        foodManager = FindObjectOfType<FoodManager>();
    }

    void Update()
    {
        
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector3.back)
            {
                direction = Vector3.forward;
                canChangeDirection = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector3.forward)
            {
                direction = Vector3.back;
                canChangeDirection = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector3.right)
            {
                direction = Vector3.left;
                canChangeDirection = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector3.left)
            {
                direction = Vector3.right;
                canChangeDirection = false;
            }
        }


        // Pohyb hlavy hada
        transform.Translate(direction * speed * Time.deltaTime);

        // Pohyb těla hada

        PositionsHistory.Insert(0, transform.position);
        
        int index = 0;
        foreach(var body in snakeBodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Min(index * Gap, PositionsHistory.Count - 1)];
            body.transform.position = point;
            index++;
        }

        // Pohyb hada
        transform.Translate(direction * speed * Time.deltaTime);

        // Detekce kolize se zdmi a jídlem
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Food")
            {
                // Zvětšení hada o jednu část
                AddBodyPart();

                // Skrytí sežráného jídla
               hitCollider.gameObject.SetActive(false);

               foodManager.SpawnFood();
               

            }
            else if (hitCollider.tag == wallTag)
            {
                SceneManager.LoadScene("End");
            }

        }

    }





    void AddBodyPart()
    {
        // Vytvoření nové části hada
        GameObject newBodyPart = Instantiate(snakeBodyPrefab);

        // Nastavení pozice nové části hada za poslední část hada
        Transform lastBodyPart = snakeBodyParts[snakeBodyParts.Count - 1];
        Vector3 newBodyPartPosition = lastBodyPart.position - lastBodyPart.forward * 0.5f;
        newBodyPart.transform.position = newBodyPartPosition;

        // Nastavení rotace nové části hada podle rotace poslední části hada
        newBodyPart.transform.rotation = lastBodyPart.rotation;

        // Přidání nové části do seznamu
        snakeBodyParts.Add(newBodyPart.transform);


    }


    void LateUpdate()
    {
        if (Mathf.Abs(transform.eulerAngles.y - lastRotation.eulerAngles.y) > 0.1f)
        {
            canChangeDirection = true;
            lastRotation = transform.rotation;
        }
    }
}
