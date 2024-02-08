using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    private static WorldData instace;

    public static WorldData Instance
    { get { return instace; } }

    public bool gameStarted = false;

    [SerializeField]
    private PlayerController player;

    public Vector3 PlayerPos
    { get { return player.transform.position; } }

    [SerializeField]
    private Camera cam;

    public Camera PlayerCam
    { get { return cam; } }

    public int enemyDeathBullets
    { get; private set; }

    public void AddEnemyDeathBullets()
    {
        enemyDeathBullets += 2;
    }

    // returns Vector 2 that point at mouse dir
    public Vector2 GetDirToMouse(Transform _toRotate)
    {
        Vector2 mouseScreenPos = cam.ScreenToWorldPoint(Input.mousePosition);


        return (mouseScreenPos - (Vector2)_toRotate.position).normalized;
    }

    private void Awake()
    {
        instace = this;
        RestartGame();
    }

    private void Start()
    {
        GameEventsManager.RestartGame += RestartGame;
        GameEventsManager.AddEnemyDeathBulletsEvent += AddEnemyDeathBullets;
    }

    private void RestartGame()
    {
        enemyDeathBullets = 0;
    }
}
