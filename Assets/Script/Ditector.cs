using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C100
{
    public static class Ditector
    {
        readonly static int multi = 2;
        public static float scaledDeltaTime=>delta;
        private static float delta;
        private static List<Physics> list;
        public static void Init()
        {
            list = new List<Physics>();
        }

        public static void Add(Physics s)
        {
            list.Add(s);
        }

        public static void Update()
        {
            delta = Time.deltaTime / multi;
            for (int time = 0; time < multi; time++)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].UpdateParametar();
                }
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        list[i].CollisionBase(list[j]);
                    }
                }
            }
        }
    }

}
