using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AprilsDayAtFools
{
    public static class MarchCheck
    {
        public static bool IsMarch
        {
            get
            {
                return !(!LoadedAssetsHandler.LoadedEnemyBundles.Keys.Contains("MarchBoss") && LoadedAssetsHandler.LoadEnemyBundle("MarchBoss") == null);
            }
        }
    }
}
