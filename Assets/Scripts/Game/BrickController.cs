using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : Singleton<BrickController>
{

    public MeshRenderer brickMeshRender;
    [SerializeField] BrickColorScriptable colorScriptable;

    public BrickType brickType;
    //public bool isFloor2 = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResetBrick(0f));
    }


    public IEnumerator ResetBrick(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        if(BrickManager.Instance.inFloor2 == false)
        {
            brickMeshRender.gameObject.SetActive(true);
            this.GetComponent<BoxCollider>().enabled = true;
            brickType = (BrickType)Random.Range(0, 3);
            brickMeshRender.material = colorScriptable.listMaterial[(int)brickType];
        }
        
        if(BrickManager.Instance.inFloor2 == true)
        {
            brickMeshRender.gameObject.SetActive(true);
            this.GetComponent<BoxCollider>().enabled = true;
            brickType = BrickManager.Instance.brickColors[Random.Range(0, BrickManager.Instance.brickColors.Count)];
            for(int i = 0; i < 3; i++)
            {
                if(brickType == (BrickType)i)
                {
                    brickMeshRender.material = colorScriptable.listMaterial[i];
                }
            }
        }
    }

    public void BrickEaten()
    {
        this.brickType = BrickType.NONE;
        brickMeshRender.gameObject.SetActive(false);
        this.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(ResetBrick(5f));
    }


}
