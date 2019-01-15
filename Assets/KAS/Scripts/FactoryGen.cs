using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGen : MonoBehaviour {

    public Transform terrain, factoryGrid, screenParent;
    public int gridSizeX, gridSizeY;
    GameObject[] grid;
    public GameObject gridSpot;

    public List<GameObject> machines = new List<GameObject>();
    public GameObject[] screenTypes;

    //set to adjust grid to terrain world pos
    public Vector3 gridOffset;

	void Awake () {

        GenerateFactoryGrid();
        
    }

    void GenerateFactoryGrid()
    {
        //jump to corner of terrain

        grid = new GameObject[(gridSizeX + 1) * (gridSizeY + 1)];

        for (int i = 0, y = 0; y <= gridSizeY; y++)
        {
            for (int x = 0; x <= gridSizeX; x++, i++)
            {
                grid[i] = Instantiate(gridSpot, new Vector3(x * gridSpot.transform.localScale.x,
                    terrain.position.y, y * gridSpot.transform.localScale.x), Quaternion.identity, factoryGrid);
            }
        }

        factoryGrid.transform.localPosition = gridOffset;

        GenerateScreens();
    }

    void GenerateScreens()
    {
        for (int i = 0; i < grid.Length; i++)
        {
            //check if player or house is in this gridSpot
            bool canGenerate = true;

            Collider[] hitColliders = Physics.OverlapSphere(grid[i].transform.position, 15);

            for (int h = 0; h < hitColliders.Length; h++)
            {
                if (hitColliders[h].gameObject.tag == "Barren" )
                {
                    canGenerate = false;
                }
            }

            //if no player/house, generate tree
            if (canGenerate)
            {
                //generate random machine type
                int randomScreen = Random.Range(0, screenTypes.Length);
                GameObject machineClone = Instantiate(screenTypes[randomScreen], grid[i].transform.position, Quaternion.identity, screenParent);
                machines.Add(machineClone);

                //alter the scale
                float randomScaleX = Random.Range(0.5f, 2f);
                float randomScaleY = Random.Range(0.5f, 2f);
                float randomScaleZ = Random.Range(0.5f, 2f);

                machineClone.transform.localScale = new Vector3(machineClone.transform.localScale.x * randomScaleX,
                    machineClone.transform.localScale.y * randomScaleY, machineClone.transform.localScale.z * randomScaleZ);

                //alter the position
                float randomX = Random.Range(0f, 5f);
                float randomZ = Random.Range(0f, 5f);
                float yOffset = -machineClone.transform.localScale.y / 2;

                machineClone.transform.Translate(randomX, yOffset, randomZ);
            }
        }

        
    }
}
