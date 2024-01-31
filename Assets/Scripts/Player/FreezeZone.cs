using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeZone : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, -Time.deltaTime * 180);
    }
    public void SetSize(float _size)
    {
        transform.localScale = new Vector2(_size, _size);
    }
}
