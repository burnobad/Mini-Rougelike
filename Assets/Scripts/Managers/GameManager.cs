using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Upgrades 
{ 
    None = -1, ShootSpeed = 0, MoveSpeed, EnemyDeathBullets, DashRechargeTime,
    SlowDownZone
}
public class GameManager : MonoBehaviour
{
    [SerializeField]
    protected AudioSource upgSFXSource;
    [SerializeField]
    protected AudioClip upgSFXPickUp;
    [SerializeField]
    protected AudioClip upgSFXNextLvl;

    private bool gameOver;
    private int curLvl;
    private int curUpgPickUp;
    private int upgForLvl;

    private Upgrades[] availableUpgrades = new Upgrades[3];
    
    private readonly int numOfUpgrades 
        = System.Enum.GetValues(typeof(Upgrades)).Length;

    private void OnEnable()
    {
        curLvl = 1;
        curUpgPickUp = 0;
        upgForLvl = 4;
        gameOver = false;

    }
    void Start()
    {
        GameEventsManager.UpgradePickUpEvent += UpgradePickUp;
        GameEventsManager.DealDamageEvent += DealDamage;
        GameEventsManager.UpgradeChosenEvent += UpgradeChosen;
        GameEventsManager.GameOverEvent += GameOver;
        GameEventsManager.RestartGame += RestartGame;

        GameEventsManager.UpdateLevelData(curLvl, curUpgPickUp);

        SummonUpgradePickUp();
    }

    private void Update()
    {
        if(gameOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
               GameEventsManager.RestartGame();
            }
        }
    }
    private void AddLevel()
    {
        PauseGame();
        curUpgPickUp -= upgForLvl;
        curLvl++;
        GenerateUpgrades();

        GameEventsManager.AddLevelEvent(availableUpgrades);
    }

    private void SummonUpgradePickUp()
    {
        GameObject upgradePickUp =
        ObjectPoolsManager.Instance.GetObjFromPool(ObjectPoolTypes.UpgragePickUp);

        upgradePickUp.GetComponent<UpgradePickUp>().ChangePos();

        upgradePickUp.SetActive(true);
    }

    private void GenerateUpgrades()
    {
        for (int i = 0; i < availableUpgrades.Length; i++)
        {
            availableUpgrades[i] = Upgrades.None;
        }
        for (int i = 0; i < availableUpgrades.Length; )
        {
            int randomNum = Random.Range(0, numOfUpgrades - 1);
            bool isUnique = false;

            if (availableUpgrades[i] == Upgrades.None)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    isUnique = true;
                    if (availableUpgrades[j] == (Upgrades)randomNum)
                    {
                        isUnique = false;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("ERROR, availableUpgrades[i] != Upgrades.None");
            }

            if (isUnique)
            {
                availableUpgrades[i] = (Upgrades)randomNum;
                i++;
            }
        }


    }

    private void PauseGame()
    {
        Debug.Log("Game Paused");

        Time.timeScale = 0;
    }
    private void UnpauseGame()
    {
        Debug.Log("Game Unpaused");

        Time.timeScale = 1;
    }

    #region For Events
    private void RestartGame()
    {
        curLvl = 1;
        curUpgPickUp = 0;
        upgForLvl = 4;
        gameOver = false;

        GameEventsManager.UpdateLevelData(curLvl, curUpgPickUp);

        SummonUpgradePickUp();

        UnpauseGame();
    }
    private void DealDamage(GameObject _obj)
    {
        if(_obj.activeInHierarchy)
        {
            _obj.GetComponentInParent<InteractableObject>().ReceiveDamage();
        }
    }

    private void UpgradePickUp()
    {
        curUpgPickUp++;

        upgSFXSource.PlayOneShot(upgSFXPickUp);

        GameEventsManager.UpdateLevelData(curLvl, curUpgPickUp);
        if (curUpgPickUp >= upgForLvl)
        {
            AddLevel();
        }
    }

    private void UpgradeChosen(Upgrades _upg)
    {
        switch(_upg)
        {
            case Upgrades.ShootSpeed:
                GameEventsManager.AddShootSpeedEvent();
                break;
            case Upgrades.MoveSpeed:
                GameEventsManager.AddMoveSpeedEvent();
                break;
            case Upgrades.EnemyDeathBullets:
                GameEventsManager.AddEnemyDeathBulletsEvent();
                break;  
            case Upgrades.DashRechargeTime:
                GameEventsManager.AddDashRechargeTimeEvent();
                break;
            case Upgrades.SlowDownZone:
                GameEventsManager.AddSlowDownZoneEvent();
                break;
        }

        upgSFXSource.PlayOneShot(upgSFXNextLvl);
        GameEventsManager.UpdateLevelData(curLvl, curUpgPickUp);
        UnpauseGame(); 
    }

    private void GameOver()
    {
        PauseGame();
        gameOver = true;
    }


    #endregion
}
