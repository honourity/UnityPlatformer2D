using System;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class EnumsExtensionMethods
    {
        public static Boolean HasFlag(this Enums.UnitStateEnun self, Enums.UnitStateEnun flag)
        {
            return (self & flag) == flag;
        }

        public static Enums.UnitStateEnun NAND(this Enums.UnitStateEnun self, Enums.UnitStateEnun flag)
        {
            return (self | flag) ^ flag;
        }

        public static string StateSummary(this Enums.UnitStateEnun self)
        {
            var result = new StringBuilder("Debug State Info\n");

            foreach (var value in Enum.GetValues(typeof(Enums.UnitStateEnun)))
            {
                //Debug.Log(String.Format("{0}: {1}", Enum.GetName(typeof(Enums.UnitStateEnum), value), self.HasFlag((Enums.UnitStateEnum)value)));

                if (self.HasFlag((Enums.UnitStateEnun)value))
                {
                    result.AppendLine(Enum.GetName(typeof(Enums.UnitStateEnun), value));
                }
            }

            return result.ToString();
        }
    }

    public static class Enums
    {
        [Flags]
        public enum UnitStateEnun
        {
            Grounded = 0x0001,
            Moving = 0x0002,
            FacingRight = 0x0004,
            FacingLeft = 0x0008,
            Jumping = 0x0010,
            DoubleJumping = 0x0020,
            Falling = 0x0040,
            Landing = 0x0080,
            Attacking = 0x0100,
        }
    }
}
