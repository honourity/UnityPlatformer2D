using System;
using UnityEngine;

namespace Assets.Scripts
{
	public static class EnumsExtensionMethods
	{
		public static Boolean HasFlag(this Enums.UnitStateEnum self, Enums.UnitStateEnum flag)
		{
			return (self & flag) == flag;
		}

		public static Enums.UnitStateEnum NAND(this Enums.UnitStateEnum self, Enums.UnitStateEnum flag)
		{
			return (self | flag) ^ flag;
		}

		public static void LogState(this Enums.UnitStateEnum self)
		{
			foreach (var value in Enum.GetValues(typeof(Enums.UnitStateEnum)))
			{
				Debug.Log(String.Format("{0}: {1}", Enum.GetName(typeof(Enums.UnitStateEnum), value), self.HasFlag((Enums.UnitStateEnum)value)));
			}
		}
	}

	public static class Enums
	{
		[Flags]
		public enum UnitStateEnum
		{
			Grounded = 0x0001,
			Moving = 0x0002,
			FacingRight = 0x0004,
			FacingLeft = 0x0008,
			Attacking = 0x0010,
			Jumping = 0x0020,
			DoubleJumping = 0x0040,
			Falling = 0x0080,
			Landing = 0x0100,
		}
	}
}
