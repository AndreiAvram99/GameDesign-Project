using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public List<GameObject> floors;
    public GameObject firstPlayer;
    public GameObject secondPlayer;
    public GameObject mainCamera;
    private List<GameObject> availableFloors = new List<GameObject>();
    private List<GameObject> currentFloors = new List<GameObject>();
    private GameObject startFloor;
    
    private void shuffle(List<GameObject> list)
    {
        for (int i = 0; i <= Constants.ShuffleIterations; i++) {
            int fIndex = Random.Range(0, list.Count);
            int sIndex = Random.Range(0, list.Count);
            GameObject aux = list[fIndex];
            list[fIndex] = list[sIndex];
            list[sIndex] = aux; 
        }
    }

    private void createAvailableFloors() {
        availableFloors.Clear();
        foreach (GameObject floor in floors) {
            if (!currentFloors.Contains(floor)){
                availableFloors.Add(floor);
            }
        }
    }

    private void setPositionsForCurrentFloors() {
        int index = 0;
        foreach (GameObject currentFloor in currentFloors)
        {
            currentFloor.transform.position = new Vector3(0, 0, Constants.FloorLength * index);
            index += 1;
        }
    }

    private void setPositionsForAvailableFloors() {
        int index = 0;
        foreach (GameObject availableFloor in availableFloors) {
            availableFloor.transform.position = new Vector3(100, 0, Constants.FloorLength * index);
            index += 1;
        }
    }

    private void replaceFirstCurrentFloor() {
        GameObject saveFloor = currentFloors[0];
        currentFloors[0].transform.position = new Vector3(200, 0, 0);
        currentFloors.RemoveAt(0);
        shuffle(availableFloors);
        currentFloors.Add(availableFloors[0]);
        createAvailableFloors();
        setPositionsForCurrentFloors();
        setPositionsForAvailableFloors();
    }

    public void resetStart()
    {
        currentFloors[0] = startFloor;
        createAvailableFloors();
        setPositionsForCurrentFloors();
        setPositionsForAvailableFloors();  
    }

    private void initCurrentFloors()
    {
        currentFloors.Add(floors[0]);
        floors.RemoveAt(0);
        shuffle(floors);
        currentFloors.Add(floors[1]);
        currentFloors.Add(floors[2]);
        createAvailableFloors();
        setPositionsForCurrentFloors();
        setPositionsForAvailableFloors();  
    }

    void Start()
    {
        startFloor = floors[0];
        initCurrentFloors();
    }


    private void resetPositions(GameObject gameObject)
    {
        var position = gameObject.transform.position;
        position = new Vector3(position[Constants.X],
                               position[Constants.Y],
                               position[Constants.Z] - Constants.FloorLength);
        gameObject.transform.position = position;
    }

    private void checkPlayers() {
        if (firstPlayer.transform.position[Constants.Z] >= 2 ||
           secondPlayer.transform.position[Constants.Z] >= 2) {

            resetPositions(mainCamera);
            resetPositions(firstPlayer);
            resetPositions(secondPlayer);
           
            replaceFirstCurrentFloor();
        }
    }

    void Update()
    {
        checkPlayers();
    }
}
