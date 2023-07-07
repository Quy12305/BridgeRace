using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrickBridgeController : MonoBehaviour
{

    [SerializeField] MeshRenderer brickMeshRender;
    [SerializeField] BrickColorScriptable colorScriptable;

    public BrickType brickBridgeType ;
    public Transform target;

    //public void SetColor(BrickType color)
    //{
    //    brickType = color;
    //    brickMeshRender.material = colorScriptable.listMaterial[(int)brickType];
    //    this.GetComponent<MeshRenderer>().enabled = true;
    //}


    private void OnTriggerEnter(Collider other)
    {
        PlayerController playercontroller = other.GetComponent<PlayerController>();
        AIController aicontroller = other.GetComponent<AIController>();
        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();

        if (playercontroller != null)
        {
            playercontroller.transformbrickbridge = this.transform.position;
            
            if (playercontroller.listBrickHave.Count > 0)
            {
                playercontroller.transform.position = this.transform.position + Vector3.up * 0.1f;

                if (this.GetComponent<MeshRenderer>().enabled == false || this.brickBridgeType != playercontroller.playerbrickType)
                {
                    this.GetComponent<MeshRenderer>().enabled = true;
                    brickBridgeType = playercontroller.playerbrickType;
                    brickMeshRender.material = colorScriptable.listMaterial[(int)brickBridgeType];

                    GameObject brickRemove = playercontroller.listBrickHave[playercontroller.listBrickHave.Count - 1];
                    Destroy(brickRemove);
                    playercontroller.listBrickHave.Remove(brickRemove);
                }
            }  

        }

        if(aicontroller != null)
        {
            agent.SetDestination(target.position);

            this.GetComponent<MeshRenderer>().enabled = true;
            brickBridgeType = aicontroller.AIbrickType;
            brickMeshRender.material = colorScriptable.listMaterial[(int)brickBridgeType];
            aicontroller.transform.position = this.transform.position + Vector3.up * 0.1f;

            GameObject brickRemove = aicontroller.AIlistBrickHave[aicontroller.AIlistBrickHave.Count - 1];
            Destroy(brickRemove);
            aicontroller.AIlistBrickHave.Remove(brickRemove);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y - 0.3f, controller.transform.position.z);
        }
    }

}
