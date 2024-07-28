using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private Animator anim;
    public float currentHealth { get; private set; }
    
    private bool dead;
    private Animator anim1;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim1 = GetComponent<Animator>();
    }
    public void TakeDamage(float _damage) 
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        Debug.Log(currentHealth.ToString());
        if (currentHealth > 0)
        {
            anim1.SetTrigger("hurt");
        }
        else
        {
            if (!dead) 
            {
                anim1.SetTrigger("die");
                GetComponent<Playermovement>().enabled = false;
                dead = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            TakeDamage(1);
        }
    }
}
