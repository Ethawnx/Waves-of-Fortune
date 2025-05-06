using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHitCollider : MonoBehaviour
{
    private int _attackDamage;
    private PlayerStats _playerStats;
    private GameObject _deathVFX;
    private Snake snake;
    //public bool isHitPlayer;

    void Start()
    {
        _attackDamage = (int)GetComponentInParent<Snake>().AttackDamage;
        _playerStats = FindFirstObjectByType<PlayerStats>();
        _deathVFX = GetComponentInParent<Snake>().DeathVFX;
        snake = GetComponentInParent<Snake>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerStats._ishitBySnake = true;
            StartCoroutine(_playerStats.ResetIsHitState(snake._AttackCooldown));
            if (_playerStats.IsOnMashroom)
            {
                _playerStats.TakeDamage(_attackDamage - 1);
                UIManager.instance.UpdateHealth();
                Destroy(snake.gameObject);
                GameObject VFX = Instantiate(_deathVFX, transform.position, Quaternion.identity);
                Destroy(VFX, 3f);
            } 
            else 
            {
                _playerStats.TakeDamage(_attackDamage);
                UIManager.instance.UpdateHealth();
            }
        }
    }
}
