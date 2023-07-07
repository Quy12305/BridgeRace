using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTranform;
    public Vector3 target;

    void Start()
    {
        target = transform.position - playerTranform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTranform.position + target, Time.deltaTime * 2);
    }
}
