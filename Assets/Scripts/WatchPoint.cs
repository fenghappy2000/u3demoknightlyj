﻿using UnityEngine;
using System.Collections;

public class WatchPoint : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        LocalPlayer localPlayer = UnityHelper.FindLocalPlayer();
        if (localPlayer != null)
        {
            transform.position = localPlayer.sight.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LocalPlayer localPlayer = UnityHelper.FindLocalPlayer();
        if (localPlayer != null)
        {
            ChasePlayer(localPlayer);
        }

        //Debug.Log("calc " + realWatchPoint);
    }

    [SerializeField]
    float moveFactor = 1f;
    [SerializeField]
    float minMoveStep = 10f;
    [SerializeField]
    float chasingDistance = 3f;
    void ChasePlayer(LocalPlayer localPlayer)
    {
        float distance = (localPlayer.sight.position - this.transform.position).magnitude;
        bool chase = false;
        if (CommonHelper.FloatEqual(localPlayer.rigidBody.velocity.magnitude, 0))
        {  //静止时,不管距离直接跟上
            chase = true;
        }
        else
        {  //移动中,超出一定距离则跟上玩家
            if (distance > chasingDistance)
            {
                //Vector3 dir = transform.position - localPlayer.sight.position;
                //dir *= chasingDistance / distance;
                //transform.position = localPlayer.sight.position + dir;
                chase = true;
            }
        }


        if (chase)
        {
            float moveStep = distance * moveFactor * moveFactor;
            if (moveStep < minMoveStep)  //最小追踪步长
                moveStep = minMoveStep;

            if (moveStep * Time.deltaTime > distance)
            {
                transform.position = localPlayer.sight.position;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, localPlayer.sight.position, moveStep * Time.deltaTime);
            }
        }
    }
}
