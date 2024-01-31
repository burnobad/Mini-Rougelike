using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickUp : MonoBehaviour
{
    const float SMALL_SCALE = 0.5f;
    const float BIG_SCALE = 0.75f;

    private bool getBigger;

    private void Start()
    {
        transform.localScale = new Vector2 (SMALL_SCALE, SMALL_SCALE);
        getBigger = true;
    }
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -Time.deltaTime * 80);

        if(getBigger)
        {
            transform.localScale = LerpScale(BIG_SCALE);
            if (CheckScale(BIG_SCALE)) 
                getBigger = false;
        }
        else 
        {
            transform.localScale = LerpScale(SMALL_SCALE);
            if (CheckScale(SMALL_SCALE))
                getBigger = true;
        }
    }

    Vector2 LerpScale(float _toLearp)
    {
       return Vector2.Lerp
            (transform.localScale,
            new Vector2(_toLearp, _toLearp), Time.deltaTime * 5);
    }

    bool CheckScale(float _toCompare)
    {
        float dif = transform.localScale.x - _toCompare;
        if (Mathf.Abs(dif) < 0.01f)
        {
            return true;
        }
        return false;
    }

    public void ChangePos()
    {
        float randomX = Random.Range(-10.0f, 10.0f);
        float randomY = Random.Range(-10.0f, 10.0f);
        Vector2 generatedPos = new Vector2(randomX, randomY);

        if (Vector2.Distance(generatedPos, transform.position) < 12)
        {
            ChangePos();    
        }
        else
        {
            transform.localPosition = generatedPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D _coll)
    {
        GameEventsManager.UpgradePickUpEvent();
        ChangePos();
    }

}
