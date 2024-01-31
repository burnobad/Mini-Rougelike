using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : InteractableObject
{
    private Vector2 spawnPos;
    private float maxDist = 20;

    protected override void IOjOnEnable()
    {
        spawnPos = transform.position;
    }
    protected override void IObjFixedUpdate()
    {
        rb.velocity = transform.up * defaultMoveSpeed;
    
        if(Vector2.Distance(spawnPos, transform.position) > maxDist)
        {
            OnDeath();
        }
    
    }

    protected override void OnDeath()
    {
        this.gameObject.SetActive(false);
    }
}
