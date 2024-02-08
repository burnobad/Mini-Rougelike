using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : EnemyBase
{
    protected override void IObjFixedUpdate()
    {
        base.IObjFixedUpdate();

        if (canMove)
        {
            charModel.transform.up = playerDir;
            MoveToPlayer();
        }

    }
}

