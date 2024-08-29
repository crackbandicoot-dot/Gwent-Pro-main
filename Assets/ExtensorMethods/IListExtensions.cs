
using System;
using System.Collections.Generic;
using Unity.Mathematics;

public static class IListExtensions
 {
        private static System.Random random = new ();
        public static T PickRandom<T>(this IList<T> list)
        {
            int rand = random.Next(0, list.Count - 1);
             return list[rand];
        }
   }

