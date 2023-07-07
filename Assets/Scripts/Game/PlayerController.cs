using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public enum AnimationType
    {
        Idle,Run,Dance
    }

    [SerializeField] Animator playerAnim;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] Transform brickTransform;
    [SerializeField] BrickColorScriptable colorScriptable;
    [SerializeField] LayerMask Wall;
    [SerializeField] LayerMask Floor2;

    public bool inBridge;
    public float speed = 5f;
    public Vector3 transformbrickbridge = new Vector3(10, 10, 10);
    public int brickeaten = 0;
    public List<GameObject> listBrickHave = new List<GameObject>();
    public AnimationType currentAnimType = AnimationType.Idle;
    public BrickType playerbrickType = BrickType.GREEN;
    public bool finish = false;


    void Update()
    {
        CheckLanCan();
        if (GameManager.Instance.IsState(GameState.GamePlay) && Input.GetMouseButton(0))
        {
            this.transform.LookAt(new Vector3(this.transform.position.x + variableJoystick.Horizontal * speed * Time.deltaTime,
                this.transform.position.y, this.transform.position.z + variableJoystick.Vertical * speed * Time.deltaTime));

            if (CheckLanCan())
            {
                Debug.Log("Va Lan Can");
                ChangeAnim(AnimationType.Idle);
            }

            else
            {
                ChangeAnim(AnimationType.Run);
                this.transform.position = new Vector3(this.transform.position.x + variableJoystick.Horizontal * speed * Time.deltaTime,
                    this.transform.position.y, this.transform.position.z + variableJoystick.Vertical * speed * Time.deltaTime);
            }
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.LogError("GetMouseButtonUp");
            ChangeAnim(AnimationType.Idle);
        }

        if(Physics.Raycast(transform.position, -this.transform.up, 3f, Floor2))
        {
            if (!BrickManager.Instance.brickColors.Contains(playerbrickType))
            {
                BrickManager.Instance.brickColors.Add(playerbrickType);
                //BrickController.Instance.isFloor2 = true;
                BrickManager.Instance.inFloor2 = true;
            }
                
        }

        Vector3 playertransform = transform.position;

        if(playertransform.x > 9.8f)
        {
            playertransform.x = -9.8f;
        }
        if (playertransform.x < -9.8f)
        {
            playertransform.x = 9.8f;
        }
        if (playertransform.z < -9.4f)
        {
            playertransform.z = 9.4f;
        }   
        if(listBrickHave.Count == 0 && playertransform.z > transformbrickbridge.z && !finish)
        {
            playertransform.z = transformbrickbridge.z;
        }

            transform.position = playertransform;
    }


    public void ChangeAnim(AnimationType _type)
    {
        if(currentAnimType != _type)
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

        if (brick != null && brick.brickType == this.playerbrickType)
        {
                brick.BrickEaten();
                GameObject obj = Instantiate(brickPrefab, brickTransform);
                obj.GetComponent<BrickController>().enabled = false;
                obj.GetComponent<BrickController>().brickMeshRender.material = colorScriptable.listMaterial[(int)playerbrickType];
                obj.GetComponent<BoxCollider>().enabled= false;
                obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.3f, 0f);
                listBrickHave.Add(obj);
                brickeaten += 1;
            
        }

        if(other.tag == "Win")
        {
            finish = true;
            while (listBrickHave.Count > 0)
            {
                GameObject brickRemove = listBrickHave[listBrickHave.Count - 1];
                Destroy(brickRemove);
                listBrickHave.Remove(brickRemove);
            }
            ChangeAnim(AnimationType.Dance);
        }

        //if (brickbridge != null && other.tag == "BrickBridge")
        //{
        //    if (other.GetComponent<MeshRenderer>().enabled == false)
        //    {
        //        //playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 0.3f, playerTransform.position.z);
        //        //brickbridge.SetColor(this.brickbridgeType);
        //        //Debug.LogError("Destroy");

        //        GameObject brickRemove = listBrickHave[listBrickHave.Count - 1];
        //        Destroy(brickRemove);
        //        listBrickHave.Remove(brickRemove);
        //    }
        //}
    }

    private bool CheckLanCan()
    {
        Debug.DrawLine(transform.position + Vector3.up, (transform.position + this.transform.forward * 1f) + Vector3.up, Color.red);
        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up , this.transform.forward , out hit, 0.1f, Wall);

        return hit.collider != null;
    }
}
