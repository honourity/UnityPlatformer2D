using UnityEngine;

namespace Assets.Scripts
{
	public static class AttackGraph
	{
		public static Attack[] RootAttacks;

		static AttackGraph()
		{
			//load .json file and deserialise into Attack graph
		}
	}

	public class Attack
	{
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

/*

	RootAttacks : [
		{
			AnimationKey : "HorizontalAttack1",
			AudioClipName : "HorizontalAttack1AudioClip",
			DeadlyRangeStart : 0,
			DeadlyRangeEnd : 0,
			Cooldown : 0,
			VelocityBoot : 0,
			AnimationLength : 0,
			Damage : 0,
			FollowupAttacks : [
				{
					AnimationKey : "HorizontalAttack2",
					AudioClipName : "HorizontalAttack2AudioClip",
					DeadlyRangeStart : 0,
					DeadlyRangeEnd : 0,
					Cooldown : 0,
					VelocityBoot : 0,
					AnimationLength : 0,
					Damage : 0,
					FollowupAttacks : [
						{
							AnimationKey : "HorizontalAttack3",
							AudioClipName : "HorizontalAttack3AudioClip",
							DeadlyRangeStart : 0,
							DeadlyRangeEnd : 0,
							Cooldown : 0,
							VelocityBoot : 0,
							AnimationLength : 0,
							Damage : 0,
							FollowupAttacks : []					
						}
					]
				}
			]
		}
	]
*/
