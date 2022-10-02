using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
	private bool _isAttacking;
	private Animator _animator;
	private int contador;
	public Text puntos;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void LateUpdate()
	{
		// Animator
		if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Atack"))
		{
			_isAttacking = true;
		}
		else
		{
			_isAttacking = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_isAttacking == true)
		{
			
			if (collision.CompareTag("Enemy") || collision.CompareTag("Bullet"))
			{
				collision.SendMessageUpwards("AddDamage");
				contador = contador + 1;
				puntos.text = "puntos: " + contador;
			}
		}
	}
    private void OnDisable()
    {
		contador = 0;
    }
}
