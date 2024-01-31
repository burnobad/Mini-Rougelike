using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [SerializeField]
    protected Rigidbody2D rb;
    public Rigidbody2D RB
    { get { return rb; } }

    [SerializeField]
    [Tooltip("Model, used for rotation")]
    protected Transform charModel;

    [SerializeField]
    protected float defaultMoveSpeed;

    [SerializeField]
    private float maxHealth;

    protected float currentHealth;

    protected SpriteRenderer charSpriteRenderer;
    protected Color defaultColor;
    protected Color hitColor = Color.white;

    protected virtual void IOjOnEnable() { }

    protected virtual void IOjOnStart() { }
    protected virtual void IOjOnGameRestart() { }
    protected virtual void IObjUpdate() { }
    protected virtual void IObjFixedUpdate() { }

    private void OnEnable()
    {
        IOjOnEnable();
        RestartGame();
    }

    private void RestartGame()
    {
        currentHealth = maxHealth;
        IOjOnGameRestart();
    }

    private void Start()
    {
        GameEventsManager.RestartGame += RestartGame;

        charSpriteRenderer = charModel.GetComponent<SpriteRenderer>();
        defaultColor = charSpriteRenderer.color;
        IOjOnStart();
    }

    private void Update()
    {
        IObjUpdate();
    }

    private void FixedUpdate()
    {
        IObjFixedUpdate();
    }

    public virtual void ReceiveDamage()
    {

        currentHealth--;
       

        if(currentHealth <= 0)
        {
            OnDeath();
        }
    } 

    protected virtual void OnDeath()
    {
        
    }

    protected IEnumerator FlashWhite(float _flashDur, int _times)
    {
        for(int i = 0; i < _times;  i++)
        {
            charSpriteRenderer.color = hitColor;

            yield return new WaitForSeconds(_flashDur);

            charSpriteRenderer.color = defaultColor;

            yield return new WaitForSeconds(_flashDur);
        }
    }

}
