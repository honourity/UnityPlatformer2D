using Assets.Scripts;

namespace Assets.Scripts
{
	public static class AttackGraph
	{
		public static Attack[] RootAttacks;

		static AttackGraph()
		{
			RootAttacks = new Attack[]
			{
				new Attack()
				{
					//during cooldown and/or animation it queues up the same attack? not the next one
					Type = Enums.AttackKeyCode.LightAttack,
					Name = Enums.AttackName.LightPunch,
					DeadlyRangeStart = 0.04f,
					DeadlyRangeEnd = 0.15f,
					Cooldown = 1f,
					AnimationLength = 0.183f,
					VelocityBoost = 0f,
					Damage = 1,
					FollowupAttacks = new Attack[]
					{
						new Attack()
						{
							Type = Enums.AttackKeyCode.LightAttack,
							Name = Enums.AttackName.LightKick,
							DeadlyRangeStart = 0.05f,
							DeadlyRangeEnd = 0.5f,
							Cooldown = 1f,
							AnimationLength = 0.517f,
							VelocityBoost = 0f,
							Damage = 1,
							FollowupAttacks = new Attack[]
							{
								new Attack()
								{
									Type = Enums.AttackKeyCode.LightAttack,
									Name = Enums.AttackName.HeavyPunch,
									DeadlyRangeStart = 0.1f,
									DeadlyRangeEnd = 0.7f,
									Cooldown = 1f,
									AnimationLength = 0.767f,
									VelocityBoost = 0f,
									Damage = 1,
									FollowupAttacks = new Attack[0]
								}
							}
						}
					}
				}
			};
		}
	}

	public class Attack
	{
		public Enums.AttackKeyCode Type { get; set; }
		public Enums.AttackName Name { get; set; }
		public float DeadlyRangeStart { get; set; }
		public float DeadlyRangeEnd { get; set; }
		public float Cooldown { get; set; }
		public float VelocityBoost { get; set; }
		public float AnimationLength { get; set; }
		public int Damage { get; set; }
		public Attack[] FollowupAttacks { get; set; }
	}
}

