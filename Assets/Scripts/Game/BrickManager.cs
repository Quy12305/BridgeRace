using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrickManager : Singleton<BrickManager>
{
    [SerializeField] GameObject brickPrefab;
    public List<GameObject>  Allbrick;
    public List<GameObject> AllbrickFloor2;
    public bool inFloor2 = false;
    public bool stopSpawn = false;

    public List<BrickType> brickColors = new List<BrickType>();


    // Start is called before the first frame update
    void Start()
    {
        SpawnAllBrick();
    }

    private void Update()
    {
        if(!stopSpawn && inFloor2)
        {
            SpawnAllBrickFloor2();
        }
    }

    void SpawnAllBrick()
    {
        for (int i = 0;i < 9;i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject obj = Instantiate(brickPrefab);
                obj.transform.position = new Vector3((8f - i*2), -0.38f, (8f - j*2));   
                Allbrick.Add(obj);
            }                
        }
    }

    void SpawnAllBrickFloor2()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject obj = Instantiate(brickPrefab);
                obj.transform.position = new Vector3((8f - i * 4), 3.82f, (29f - j * 4));
                AllbrickFloor2.Add(obj);
            }
        }

        stopSpawn= true;
    }
}
