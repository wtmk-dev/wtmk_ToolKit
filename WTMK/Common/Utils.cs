using System;
namespace WTNK.Common
{
    public class Utility
    {
        public static T Min<T>(T itemI, T itemII) where T : IComparable<T>
        {
            return (itemI.CompareTo(itemII) < 0) ? itemI : itemII;
        }
    }
}