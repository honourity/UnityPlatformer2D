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
					Type = Enums.AttackType.LightAttack,
					AnimationKey = "light quick punch",
					AudioClipName = "",
					DeadlyRangeStart = 0f,
					DeadlyRangeEnd = 0f,
					Cooldown = 0f,
					VelocityBoost = 0f,
					Damage = 0,
					FollowupAttacks = new Attack[]
					{
						new Attack()
						{
							Type = Enums.AttackType.LightAttack,
							AnimationKey = "backwards kick",
							AudioClipName = "",
							DeadlyRangeStart = 0f,
							DeadlyRangeEnd = 0f,
							Cooldown = 0f,
							VelocityBoost = 0f,
							Damage = 0,
							FollowupAttacks = new Attack[]
							{
								new Attack()
								{
									Type = Enums.AttackType.LightAttack,
									AnimationKey = "heavy punch",
									AudioClipName = "",
									DeadlyRangeStart = 0f,
									DeadlyRangeEnd = 0f,
									Cooldown = 0f,
									VelocityBoost = 0f,
									Damage = 0,
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
		public Enums.AttackType Type { get; set; }
		public string AnimationKey { get; set; }
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

