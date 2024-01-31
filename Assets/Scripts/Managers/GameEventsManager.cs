using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager 
{
    public delegate void VoidDelegate();
    public delegate void LevelProgDelegate(int _curLevel, int _curUpgPicked);
    public delegate void ObjDelegate(GameObject _obj);
    public delegate void UpgradeArrayDelegate(Upgrades[] _availableUpgrades);
    public delegate void UpgradeDelegate(Upgrades _chosenUpgrade);

    public static VoidDelegate UpgradePickUpEvent;
    public static UpgradeArrayDelegate AddLevelEvent;
    public static UpgradeDelegate UpgradeChosenEvent;
    public static LevelProgDelegate UpdateLevelData;

    public static ObjDelegate DealDamageEvent;
    public static VoidDelegate GameOverEvent;
    public static VoidDelegate RestartGame;

    // for upgrdes
    public static VoidDelegate AddShootSpeedEvent;
    public static VoidDelegate AddMoveSpeedEvent;
    public static VoidDelegate AddEnemyDeathBulletsEvent;
    public static VoidDelegate AddDashRechargeTimeEvent;
    public static VoidDelegate AddSlowDownZoneEvent;

    // upgrade related sound

}
