using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : InteractableObject
{
    #region Unity Components

    [SerializeField]
    [Tooltip("Parent Object for all things that should face mouse")]
    private Transform mouseRotParent;

    [SerializeField]
    private FreezeZone slowDownZone;

    [SerializeField]
    protected AudioSource shootAudioSource;

    [SerializeField]
    protected List<AudioClip> shootClips;

    [SerializeField]
    protected AudioSource defaultAudioSource;

    [SerializeField]
    protected List<AudioClip> dashClips;

    [SerializeField]
    protected AudioClip dashAvailableClip;

    [SerializeField]
    protected AudioSource deathAudioSource;
    [SerializeField]
    protected AudioClip deathClip;

    #endregion

    #region Varaibles 
    const float DEFAULT_DASH_TIME = 0.15f;
    const float BUTTON_PRESS_TIMER = 0.3f;
    const float DASH_DELAY_TIME = 1.35f;
    private const float defaultShootSpeed = 0.35f;

    [SerializeField]
    private float dashForce;

    private Vector2 moveDir;

    private bool shootButtonPress;
    private bool canShoot;

    private float dashTimer;
    private bool isDashing;
    private float dashDelayTime;
    private float dashDelayTimer;

    // -1 can't flash / 1 can flash / 0 used for correct reseting
    private int canFlash;
    private bool isFlashing;

    private float currentShootSpeed;
    private float currentMoveSpeed;

    private float slowDownZoneRadius;

    protected bool canReceiveDamage;

    #endregion

    protected override void IOjOnEnable()
    {
        IOjOnGameRestart();
    }

    protected override void IOjOnGameRestart()
    {
        canShoot = true;

        dashTimer = 0;
        dashDelayTime = DASH_DELAY_TIME;
        dashDelayTimer = 0;
        isDashing = false;

        canFlash = -1;
        isFlashing = false;

        transform.position = new Vector2(0, 0.8f);

        currentShootSpeed = defaultShootSpeed;
        currentMoveSpeed = defaultMoveSpeed;

        slowDownZoneRadius = 0;
        slowDownZone.SetSize(0.1f);

        canReceiveDamage = true;
    }

    protected override void IOjOnStart()
    {
        GameEventsManager.AddShootSpeedEvent += AddShootSpeed;
        GameEventsManager.AddMoveSpeedEvent += AddMoveSpeed;
        GameEventsManager.AddDashRechargeTimeEvent += DashDelayTime;
        GameEventsManager.AddSlowDownZoneEvent += AddSlowDownZone;
    }

    protected override void IObjUpdate()
    {
        float horInput = Input.GetAxisRaw("Horizontal");
        float vertInput = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(horInput, vertInput).normalized;

        if(Input.GetMouseButton(0))
        {
            shootButtonPress = true;
        }
        else
        {
            shootButtonPress = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            dashTimer = BUTTON_PRESS_TIMER;
        }
        else if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        if (dashDelayTimer > 0)
        {
            if(!isFlashing)
            {
                canFlash = 0;
            }
            dashDelayTimer -= Time.deltaTime;
        }
        else
        {
            if(canFlash == 0)
            {
                canFlash = 1;
            }

            isFlashing = false;
        }
    }

    protected override void IObjFixedUpdate()
    {
        mouseRotParent.transform.up = WorldData.Instance.GetDirToMouse(mouseRotParent);;

        if (CanDash() && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
        else if (!isDashing)
        {
            rb.velocity = moveDir * currentMoveSpeed;
        }
        if (shootButtonPress && canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }
        if (canFlash == 1)
        {
            canFlash = -1;
            isFlashing = true;

            defaultAudioSource.PlayOneShot(dashAvailableClip);

            StartCoroutine(FlashWhite(0.1f, 2));
        }
    }

    public override void ReceiveDamage()
    {
        if(canReceiveDamage)
        {
            base.ReceiveDamage();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        deathAudioSource.PlayOneShot(deathClip);

        GameEventsManager.GameOverEvent();
    }

    protected bool CanDash()
    {
        return dashTimer > 0 && dashDelayTimer <= 0;
    }

    protected void PlayRandSound(AudioSource _audio, List<AudioClip> _clipsList)
    {
        int rand = Random.Range(0, _clipsList.Count);

        _audio.PlayOneShot(_clipsList[rand]); 
    }

    #region Coroutines

    IEnumerator ShootCoroutine()
    {
        canShoot = false;

        GameObject bullet = 
            ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.PlayerBullet);

        bullet.transform.position = this.transform.position;

        bullet.transform.up = WorldData.Instance.GetDirToMouse(bullet.transform);

        bullet.SetActive(true);

        PlayRandSound(shootAudioSource, shootClips);

        yield return new WaitForSeconds(currentShootSpeed);

        canShoot = true;
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        canReceiveDamage = false;
        rb.velocity = Vector2.zero;

        rb.AddForce
            (moveDir * dashForce, ForceMode2D.Impulse);

        PlayRandSound(defaultAudioSource, dashClips);

        yield return new WaitForSeconds(DEFAULT_DASH_TIME);

        rb.velocity = Vector2.zero;
        canReceiveDamage = true;
        isDashing = false;
        dashDelayTimer = dashDelayTime;
    }

    #endregion

    #region For Events

    private void AddShootSpeed()
    {
        currentShootSpeed -= 0.05f;
    }
    private void AddMoveSpeed()
    {
        currentMoveSpeed += 1f;
    }
    private void DashDelayTime()
    {
        dashDelayTime -= 0.27f;
    }

    private void AddSlowDownZone()
    {
        slowDownZoneRadius += 3.3f;
        slowDownZone.SetSize(slowDownZoneRadius);
    }

    #endregion
}
