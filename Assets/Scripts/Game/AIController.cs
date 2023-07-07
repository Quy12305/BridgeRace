using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Singleton<AIController>
{
    public enum AnimationType
    {
        Idle, Run, Dance
    }

    public float speed = 5f;
    [SerializeField] Animator playerAnim;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] Transform brickTransform;
    [SerializeField] BrickColorScriptable colorScriptable;
    [SerializeField] LayerMask Floor2;
    [SerializeField] Vector3 NavMeshMove = new Vector3(2.5f, -0.5f, 10f);
    [SerializeField] Vector3 NavMeshMove2 = new Vector3(0.2f, 3f, 32.4f);

    public int brick = 0;
    public List<GameObject> AIlistBrickHave = new List<GameObject>();
    public AnimationType currentAnimType = AnimationType.Idle;
    public BrickType AIbrickType = BrickType.NONE;
    public GameObject bricknearest;
    public GameObject bricknearestfloor2;
    public bool isPlay = false;
    public bool isMoveBridge = true;
    public bool isBridge = false;
    public bool isBridge1 = false;
    public bool t = false;

    GameObject brickNearest = null;

    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay) && Input.GetMouseButton(0))
        {
            isPlay = true;
        }

        if (isPlay) 
        {
            ChangeAnim(AnimationType.Run);
            if (brickNearest == null) 
            {
                if(isBridge == false && AIlistBrickHave.Count < 15)
                {
                    Movefindbrick();
                }

                if(AIlistBrickHave.Count >= 15 && isMoveBridge == true)
                {
                    this.GetComponent<NavMeshAgent>().SetDestination(NavMeshMove);
                }

                if (isBridge1 == false && BrickManager.Instance.inFloor2 == true)
                {
                    Movefindbrick();
                }

                if (AIlistBrickHave.Count >= 15 && BrickManager.Instance.inFloor2 == true)
                {
                    this.GetComponent<NavMeshAgent>().SetDestination(NavMeshMove2);
                }
            }
        }

        if(!isPlay)
        {
            ChangeAnim(AnimationType.Idle);
        }


        if (Physics.Raycast(transform.position, -this.transform.up, 3f, Floor2))
        {
            if(!BrickManager.Instance.brickColors.Contains(AIbrickType))
            {
                BrickManager.Instance.brickColors.Add(AIbrickType);
                //BrickController.Instance.isFloor2 = true;
                BrickManager.Instance.inFloor2 = true;
            }
        }

    }

    public void ChangeAnim(AnimationType _type)
    {
        if (currentAnimType != _type)
        {
            currentAnimType = _type;
            switch (_type)
            {
                case AnimationType.Idle:
                    playerAnim.SetTrigger("isIdle");
                    break;
                case AnimationType.Run:
                    playerAnim.SetTrigger("isRunning");
                    break;
                case AnimationType.Dance:
                    playerAnim.SetTrigger("isDance");
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BrickController brick = other.GetComponent<BrickController>();
        BrickBridgeController brickbridge = other.GetComponent<BrickBridgeController>();


        if (brick != null && brick.brickType == this.AIbrickType)
        {
            brickNearest = null;
            brick.BrickEaten();
            GameObject obj = Instantiate(brickPrefab, brickTransform);
            obj.GetComponent<BrickController>().enabled = false;
            obj.GetComponent<BrickController>().brickMeshRender.material = colorScriptable.listMaterial[(int)AIbrickType];
            obj.GetComponent<BoxCollider>().enabled = false;
            obj.transform.localPosition = new Vector3(0f, AIlistBrickHave.Count * 0.3f, 0f);
            AIlistBrickHave.Add(obj);
        }

        if(other.tag == "Bac1")
        {
            isMoveBridge = false;
            isBridge = true;
        }

        if (other.tag == "Bac2")
        {
            isBridge1 = true;
        }
    }


    void Movefindbrick()
    {
        if(BrickManager.Instance.inFloor2 == false)
        {
            GameObject gameobj = FindBrick(transform.position, AIbrickType);
            this.GetComponent<NavMeshAgent>().SetDestination(gameobj.transform.position);
        }

        if (BrickManager.Instance.inFloor2 == true)
        {
            GameObject gameobj = FindBrickFloor2(transform.position, AIbrickType);
            this.GetComponent<NavMeshAgent>().SetDestination(gameobj.transform.position);
        }
    }

    public GameObject FindBrick(Vector3 transform, BrickType color)
    {
        foreach (GameObject Brick in BrickManager.Instance.Allbrick)
        {
            if (Brick.GetComponent<BoxCollider>().enabled == (true) && Brick.GetComponent<BrickController>().brickType == color)
            {
                bricknearest = Brick;
                break;
            }
        }

        foreach (GameObject Brick in BrickManager.Instance.Allbrick)
        {
            if (Brick.GetComponent<BrickController>().brickType == color)
            {
                if (Vector3.Distance(transform, bricknearest.transform.position) > Vector3.Distance(transform, Brick.transform.position))
                {
                    bricknearest = Brick;
                }
            }
        }
        return bricknearest;
    }

    public GameObject FindBrickFloor2(Vector3 transform, BrickType color)
    {
        foreach (GameObject Brick in BrickManager.Instance.AllbrickFloor2)
        {
            if (Brick.GetComponent<BoxCollider>().enabled == (true) && Brick.GetComponent<BrickController>().brickType == color)
            {
                bricknearestfloor2 = Brick;
                break;
            }
        }

        foreach (GameObject Brick in BrickManager.Instance.AllbrickFloor2)
        {
            if (Brick.GetComponent<BrickController>().brickType == color)
            {
                if (Vector3.Distance(transform, bricknearestfloor2.transform.position) > Vector3.Distance(transform, Brick.transform.position))
                {
                    bricknearestfloor2 = Brick;
                }
            }
        }
        return bricknearestfloor2;
    }

    public bool checkList(BrickType Color)
    {
        foreach (BrickType color in BrickManager.Instance.brickColors)
        {
            if (color == Color)
            {
                t = true;
            }
        }
        return t;
    }
}
