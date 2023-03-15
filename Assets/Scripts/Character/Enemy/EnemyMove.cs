using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] Transform player;
    Enemy character;
    SpriteRenderer spriteRenderer;
    public bool isDead;
    Vector2 direction;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        isDead = false;
    }

    void Update()
    {
        /*direction = (player.position - transform.position).normalized;*/
        
        // enemy find path to player. if enemy blocked by wall, enemy will find another path
        if (isDead == false)
        {
            direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, character.GetSpeed()/15f * Time.deltaTime);
        }
        
        

        if (direction.x >= 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        /*if (!isDead)
            transform.Translate(direction.normalized * character.GetSpeed()/15f * Time.deltaTime);*/
    }

    void Initialize()
    {
        character = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = new Vector2();
    }

    public Vector2 GetDirection()
    {
        return direction;
    }
}
