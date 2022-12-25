using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _chestAnimationKey;

    private bool _chestOpen;
    public bool Activated {private get; set;}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Activated)
            return;

        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.Coins += _coinsAmount;
            _coinsAmount = 0;
            _chestOpen = true;
            //Debug.Log(player.Coins);
        }
        else _chestOpen = false;
        
        
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            _chestOpen = false;
        }
    }
    

    private void FixedUpdate()
    {
        _animator.SetBool(_chestAnimationKey, _chestOpen);
    }
    
}
