using System;
using System.IO;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class April
    {
        public static string AppData => Application.persistentDataPath;
        public static bool Birthday
        {
            get
            {
                //return true;
                if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) return true;
                return false;
            }
        }
        public static bool Me
        {
            get
            {
                if (Birthday) return true;

                bool ret = Directory.Exists(AppData + "/Mods/") && Directory.Exists(AppData + "/Mods/DayAtFools/") && File.Exists(AppData + "/Mods/DayAtFools/secret.txt");

                return ret;
            }
        }
    }
}
