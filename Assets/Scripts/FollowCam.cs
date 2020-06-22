using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    //Config parameters
    [SerializeField] Transform target;


    void Update()
    {
        transform.position = target.position;
    }
}
