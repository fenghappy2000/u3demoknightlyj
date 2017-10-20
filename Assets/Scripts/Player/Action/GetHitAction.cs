﻿using UnityEngine;
using System.Collections;

public class GetHitAction : ActionBase {

    public override void Start(Player player)
    {
        base.Start(player);
        EightDir dirEnum = EightDir.Front; ;
        Vector3 dir = player.hitSrcPos - player.transform.position;
        dir.y = 0;
        float frontProj = Vector3.Dot(player.transform.forward, dir);
        if (frontProj < 0)
        {
            dirEnum = EightDir.Back;
        }
        switch (dirEnum)
        {
            case EightDir.Front:
                player.aniController.SetAnimation(PlayerAniType.GetHit, PlayerAniDir.Front);
                break;
            case EightDir.Back:
                player.aniController.SetAnimation(PlayerAniType.GetHit, PlayerAniDir.Back);
                break;
        }
    }

    public override void OnAnimationEvent(string aniName, PlayerAniEventType aniEvent)
    {
        if (aniName.Equals("GetHit") && aniEvent == PlayerAniEventType.Finish)
        {
            if (this.onActionDone != null)
                onActionDone();
        }
    }
}

