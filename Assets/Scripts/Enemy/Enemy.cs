using System;
using Assets.Scripts;
using UnityEngine;

public class Enemy : Unit
{
	public override void TakeAction()
	{
		//stand there doing nothing like an idiot
	}

	internal void Attacked(int incomingDamage)
	{
		health -= incomingDamage;

		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}
