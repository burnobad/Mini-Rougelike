using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : EnemyBase
{
    const float DEFAULT_DASH_TIME = 0.15f;
    const float DASH_DELAY_TIME = 2f;

    private float dashDelayTimer;

    [SerializeField]
    private float dashForce = 18f;
    private const float dashDistance = 7;

    private bool isDashing;

    protected override void IOjOnEnable()
    {
        base.IOjOnEnable();
        isDashing = false;
    }
    protected override void IObjFixedUpdate()
    {
        base.IObjFixedUpdate();

        if (canMove)
        {
            charModel.transform.up = playerDir;
            if (!isDashing)
            {
                MoveToPlayer();
                if (dashDelayTimer <= 0 && IsInDashDistance())
                {
                    StartCoroutine(DashCoroutine());
                }

                if(dashDelayTimer > 0)
                {
                    dashDelayTimer -= Time.deltaTime;
                }
                else
                {
                    dashDelayTimer = 0;
                }
            }
        }
    }

    bool IsInDashDistance()
    {
        return Vector2.Distance(this.transform.position, WorldData.Instance.PlayerPos) < dashDistance;
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        canBeSlowedDown = false;

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        rb.AddForce
        (playerDir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(DEFAULT_DASH_TIME);

        rb.velocity = Vector2.zero;

        canBeSlowedDown = true;
        dashDelayTimer = DASH_DELAY_TIME;

        isDashing = false;
    }
}
