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
					Type = Enums.AttackKeyCode.LightAttack,
					Name = Enums.AttackName.LightPunch,
					DeadlyRangeStart = 0.2f,
					DeadlyRangeEnd = 0.8f,
					Cooldown = 1f,
					AnimationLength = 1f,
					VelocityBoost = 0f,
					Damage = 1,
					FollowupAttacks = new Attack[]
					{
						new Attack()
						{
							Type = Enums.AttackKeyCode.LightAttack,
							Name = Enums.AttackName.LightKick,
							DeadlyRangeStart = 0.2f,
							DeadlyRangeEnd = 0.8f,
							Cooldown = 1f,
							AnimationLength = 1f,
							VelocityBoost = 0f,
							Damage = 1,
							FollowupAttacks = new Attack[]
							{
								new Attack()
								{
									Type = Enums.AttackKeyCode.LightAttack,
									Name = Enums.AttackName.HeavyPunch,
									DeadlyRangeStart = 0f,
									DeadlyRangeEnd = 0f,
									Cooldown = 1f,
									AnimationLength = 1f,
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
		//public string AnimationKey { get; set; }
		public string AudioClipName { get; set; }
		public float DeadlyRangeStart { get; set; }
		public float DeadlyRangeEnd { get; set; }
		public float Cooldown { get; set; }
		public float VelocityBoost { get; set; }
		public float AnimationLength { get; set; }
		public int Damage { get; set; }
		public Attack[] FollowupAttacks { get; set; }
	}
}

