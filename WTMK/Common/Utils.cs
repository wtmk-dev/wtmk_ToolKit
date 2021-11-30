using System;
namespace WTNK.Common
{
    public class Utilitie 
    {
        public static T Min<T>(T itemI, T itemII) where T : IComparable<T>
        {
            return (itemI.CompareTo(itemII) < 0) ? itemI : itemII;
        }
    }
}