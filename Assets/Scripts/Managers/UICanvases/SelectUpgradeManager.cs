using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectUpgradeManager : MonoBehaviour
{
    [SerializeField]
    private Image[] upgChoises;

    [SerializeField]
    private TMP_Text upgNameText;

    [SerializeField]
    private TMP_Text upgDescText;

    int screenChosen;

    private Upgrades[] availableUpgrades;

    Dictionary<Upgrades, Sprite> upgImages;

   [SerializeField]
    NewDict newDict;

    [Serializable]
    public class NewDict
    {
        [SerializeField]
        private NewDictItem[] thisDictItems;

        public Dictionary<Upgrades, Sprite> ToDictionary()
        {
            Dictionary<Upgrades, Sprite> newDict = new Dictionary<Upgrades, Sprite>();

            foreach (var item in thisDictItems)
            {
                newDict.Add(item.upgrade, item.sprite);
            }

            return newDict;
        }
    }

    [Serializable]
    public class NewDictItem
    {
        [SerializeField]
        public Upgrades upgrade;
        [SerializeField]
        public Sprite sprite;
    }

    void OnEnable()
    {
        screenChosen = -1;
        upgNameText.text = "";
        upgDescText.text = "";

        upgImages = newDict.ToDictionary();

        for (int i = 0; i < availableUpgrades.Length; i++) 
        {
            if(i < upgChoises.Length)
            {
                upgChoises[i].sprite = SetUpgradeImages(availableUpgrades[i]);
            }
            else
            {
                Debug.LogError("Too many availableUpgrades");
            }
        }
    }

    public void SetArray(Upgrades[] _availableUpgrades)
    {
        availableUpgrades = _availableUpgrades;
    }

    public void FirstChoise()
    {
        ManageChoise(1);
    }
    public void SecondChoise()
    {
        ManageChoise(2);
    }
    public void ThirdChoise()
    {
        ManageChoise(3);
    }

    private Sprite SetUpgradeImages(Upgrades _upgToManage)
    {
        Sprite upgSprite = null;

        upgImages.TryGetValue(_upgToManage, out upgSprite);

        return upgSprite;
    }

    private void ManageChoise(int _screenChosen)
    {
        if (screenChosen == _screenChosen)
        {
            GameEventsManager.UpgradeChosenEvent(availableUpgrades[_screenChosen - 1]);
        }
        else
        {
            screenChosen = _screenChosen;
            ManageLowerText(availableUpgrades[_screenChosen - 1]);
        }
    }

    void ManageLowerText(Upgrades _upgToManage)
    {
        switch(_upgToManage)
        {
            case Upgrades.ShootSpeed:
                upgNameText.text = "[MY FRIEND PABLO]";
                upgDescText.text = "Increases shoot speed";
                break;
            case Upgrades.MoveSpeed:
                upgNameText.text = "[BEBEY SHOES]";
                upgDescText.text = "Increases move speed";
                break;
            case Upgrades.EnemyDeathBullets:
                upgNameText.text = "Enemy Death Bullets";
                upgDescText.text = "Enemies explode with bullets on death";
                break;
            case Upgrades.DashRechargeTime:
                upgNameText.text = "[WITCH TIME]";
                upgDescText.text = "Decreases dash recharge time";
                break;
            case Upgrades.SlowDownZone:
                upgNameText.text = "Creates Slow Down Zone";
                upgDescText.text = "Creates a zone, that decreases enemy speed";
                break;
        }
    }
}
