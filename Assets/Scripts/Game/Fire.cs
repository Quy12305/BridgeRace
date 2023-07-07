using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Vector3 end;
    [SerializeField] private GameObject Firework;
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playercontroller = other.GetComponent<PlayerController>();
        AIController aicontroller = other.GetComponent<AIController>();
        if (playercontroller != null || aicontroller != null)
        {
            Instantiate(Firework, new Vector3(transform.position.x, other.transform.position.y, transform.position.z), Quaternion.identity);
            LevelManager.Instance.OnFinish();
            AIController.Instance.isPlay = false;
        }
    }
}
