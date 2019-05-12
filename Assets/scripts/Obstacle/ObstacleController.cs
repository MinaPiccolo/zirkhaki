using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject ParentObstacle, ObstacleGO;
    public Transform ObstacleParent;
   // public List<ObstacleData> obstacleList;
  //  public List<GameObject> ObstacleGo;
    public static ObstacleController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
    }

    

    public void generateObstacle(int idRegion)
    {
        List<ObstacleData> obstacleList = LoadData.Instance.RegionInfo[idRegion].ObstaclesList;
        for (int i = 0; i < obstacleList.Count; i++)
        {
            GameObject obstacle = Controller.Instance.generateGameObj(obstacleList[i].LocationObstacle, ObstacleParent);
            //ObstacleGo.Add(obstacle);
          //  obstacle.GetComponent<ObstacleObj>().HitObstacleEvent += Tools.Hitable;
            Vector3 pos=  obstacle.transform.position;
            pos.x += i*2;
            pos.y -= i * 2;
            obstacle.transform.position=pos;
            ObstacleObj script = obstacle.AddComponent<ObstacleObj>();
            script.ObstacleInfoObj = obstacleList[i];
        }

        //for (int i = 0; i < numObj; i++)
        //{
        //    GameObject obstacle = Controller.Instance.generateGameObj(ObstacleGO, ParentObstacle.transform);
        //    obstacle.transform.position = new Vector2((ObstacleGO.transform.position.x - 2) + i * 1, ObstacleGO.transform.position.y);
        //    obstacle.GetComponent<ObstacleObj>().scratchToDrop = scratchToDrop;
        //}
    }
}
