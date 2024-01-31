using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : InteractableObject
{
    [SerializeField]
    protected Slider spawnSlider;

    protected Vector2 playerDir;

    protected bool canMove;

    protected float slowedSpeed = 5f;
    protected float moveSpeed;

    protected bool canBeSlowedDown;

    protected override void IOjOnEnable()
    {
        base.IOjOnEnable();
        canBeSlowedDown = true;
        StartCoroutine(SpawnEnemy());
    }

    protected override void IObjFixedUpdate()
    {
        playerDir = WorldData.Instance.PlayerPos - transform.position;
        playerDir.Normalize();

    }

    public override void ReceiveDamage()
    {
        StartCoroutine(FlashWhite(0.1f, 1));
        base.ReceiveDamage();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        float randDeg;

        for (int i = 0; i < WorldData.Instance.enemyDeathBullets; i++)
        {
            randDeg = Random.Range(0, 360);

            GameObject playerBullet = 
                ObjectPoolsManager.Instance.FindPool(ObjectPoolTypes.PlayerBullet).GetPoolObject();

            playerBullet.transform.position = this.transform.position;

            playerBullet.transform.rotation = Quaternion.EulerAngles(0, 0, randDeg);

            playerBullet.SetActive(true);

        }

        this.gameObject.SetActive(false);
    }

    protected void MoveToPlayer()
    {
        rb.velocity = playerDir * moveSpeed;
    }

    IEnumerator SpawnEnemy()
    {
        canMove = false;
        moveSpeed = defaultMoveSpeed;
        charModel.gameObject.SetActive(false);
        spawnSlider.gameObject.SetActive(false);
        spawnSlider.value = 0;

        yield return new WaitForEndOfFrame();

        spawnSlider.gameObject.SetActive(true);

        while (spawnSlider.value < 1)
        {
            spawnSlider.value += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        spawnSlider.gameObject.SetActive(false);
        charSpriteRenderer.color = defaultColor;
        charModel.gameObject.SetActive(true);
        canMove = true;
    }

    private void OnTriggerStay2D(Collider2D _coll)
    {
        if(_coll.CompareTag("SlowDownZone") && canBeSlowedDown)
        {
            moveSpeed = slowedSpeed;
        }
    }
    private void OnTriggerExit2D(Collider2D _coll)
    {
        if (_coll.CompareTag("SlowDownZone"))
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

}
