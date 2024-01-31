using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    protected override void IObjFixedUpdate()
    {
        base.IObjFixedUpdate();

        if (canMove)
        {
            charModel.transform.Rotate(0, 0, -Time.deltaTime * 300);
            MoveToPlayer();
        }

    }
}
