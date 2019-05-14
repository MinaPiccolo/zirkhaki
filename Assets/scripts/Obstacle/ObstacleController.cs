using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject ParentObstacle, ObstacleGO;
    public Transform ObstacleParent;
    public static ObstacleController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    

    public void generateObstacle(int idRegion)
    {
        List<ObstacleData> obstacleList = LoadData.Instance.RegionInfo[idRegion].ObstaclesList;
        for (int i = 0; i < obstacleList.Count; i++)
        {
            GameObject obstacle = Controller.Instance.generateGameObj(obstacleList[i].LocationObstacle, ObstacleParent);
            Vector3 pos=  obstacle.transform.position;
           // pos.x += i*2;
            pos.y -= i;
            obstacle.transform.position=pos;
            ObstacleObj script = obstacle.AddComponent<ObstacleObj>();
            script.ObstacleInfoObj = obstacleList[i];
        }
    }
}
