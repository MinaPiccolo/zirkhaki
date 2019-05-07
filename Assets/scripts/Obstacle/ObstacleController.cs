using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject ParentObstacle, ObstacleGO;
    void Start()
    {
        generateObstacle(5, 10);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void generateObstacle(int numObj, int scratchToDrop)
    {
        for (int i = 0; i < numObj; i++)
        {
            GameObject obstacle = Controller.Instance.generateGameObj(ObstacleGO, ParentObstacle.transform);
            obstacle.transform.position = new Vector2((ObstacleGO.transform.position.x - 2) + i * 1, ObstacleGO.transform.position.y);
            obstacle.GetComponent<ObstacleObj>().scratchToDrop = scratchToDrop;
        }
    }
}
