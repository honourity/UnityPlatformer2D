using System;
using System.Text;
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

        public static string StateSummary(this Enums.UnitStateEnum self)
        {
            var result = new StringBuilder("Debug State Info\n");

            foreach (var value in Enum.GetValues(typeof(Enums.UnitStateEnum)))
            {
                //Debug.Log(String.Format("{0}: {1}", Enum.GetName(typeof(Enums.UnitStateEnum), value), self.HasFlag((Enums.UnitStateEnum)value)));

                if (self.HasFlag((Enums.UnitStateEnum)value))
                {
                    result.AppendLine(Enum.GetName(typeof(Enums.UnitStateEnum), value));
                }
            }

            return result.ToString();
        }
    }

    public static class Enums
    {
        [Flags]
        public enum UnitStateEnum
        {
            Grounded =		0x0001,
            Moving =		0x0002,
            FacingRight =	0x0004,
            FacingLeft =	0x0008,
            Jumping =		0x0010,
            DoubleJumping = 0x0020,
            Falling =		0x0040,
            Landing =		0x0080,
            Attacking =		0x0100,
            AttackingH1 =	0x0200,
            AttackingH2 =	0x0400,
            AttackingH3 =	0x0800,
        }
    }
}
