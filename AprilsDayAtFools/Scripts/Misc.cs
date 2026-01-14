using System.IO;
using UnityEngine;

namespace AprilsDayAtFools
{
    public static class April
    {
        public static string AppData => Application.persistentDataPath;
        public static bool Me
        {
            get
            {
                bool ret = Directory.Exists(AppData + "/Mods/") && Directory.Exists(AppData + "/Mods/DayAtFools/") && File.Exists(AppData + "/Mods/DayAtFools/secret.txt");

                return ret;
            }
        }
    }
}
