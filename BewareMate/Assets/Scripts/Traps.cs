using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Traps : MonoBehaviour
{
    public List<GameObject> traps;
    public int numberOfTrapsPerFloor;
    private List<GameObject> crtFloorTraps;

    private List<GameObject> getTraps()
    {
        List<GameObject> trapsCopy = new List<GameObject>();
        int rightThreshold = traps.Count - 1;
        // Choose two random traps
        for (int counter = 0; counter < numberOfTrapsPerFloor; counter++)
        {
            int position = Random.Range(0, rightThreshold);
            Debug.Log("random position: " +  position);
            trapsCopy.Add(Instantiate(traps[position], new Vector3(0, 0, 0), Quaternion.identity));
        }
    
        return trapsCopy;
    }

    private void setTrapsPosition()
    {
        Vector3 floorPosition = this.transform.position;
        Vector3 trapPosition = new Vector3(floorPosition.x, floorPosition.y, floorPosition.z + Constants.FenceStart);

        foreach (var trap in crtFloorTraps)
        {
            trap.transform.position = trapPosition;
            trap.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            trapPosition.z += Constants.FenceDifference;
        }
    }
    
    void Start()
    {
        crtFloorTraps = getTraps();
        setTrapsPosition();
    }
    
    void Update()
    {
        setTrapsPosition();
    }
}
