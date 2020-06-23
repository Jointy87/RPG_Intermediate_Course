using UnityEngine;

namespace RPGCourse.Core
{
    public class FollowCam : MonoBehaviour
    {
        //Config parameters
        [SerializeField] Transform target;


        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
