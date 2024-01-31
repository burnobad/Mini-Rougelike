using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damage : MonoBehaviour
{
    public enum CollisionTypes { NONE, PLAYER, PLAYER_BULLET, ENEMY, WALL }

    public CollisionTypes myCollisionType;

    [Tooltip("What CollisionTypes you can damage")]
    public List<CollisionTypes> dealDamage;

    void OnTriggerEnter2D(Collider2D _coll)
    {
        Damage collDamage = _coll.GetComponent<Damage>();

        if (collDamage != null)
        {
            CheckForDamage(collDamage, _coll.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D _coll)
    {
        Damage collDamage = _coll.gameObject.GetComponentInChildren<Damage>();

        if (collDamage != null)
        {
            CheckForDamage(collDamage, _coll.gameObject);
        }
    }


    void CheckForDamage(Damage _collDamage, GameObject _collGameObject)
    {
    
        foreach (CollisionTypes myDamageType in dealDamage)
        {
            if (myDamageType == _collDamage.myCollisionType)
            {
             
                GameEventsManager.DealDamageEvent(_collGameObject);
                

                break;
            }
        }
    }

}
