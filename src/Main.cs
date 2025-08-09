using HBS.Logging;
using System;
using System.Reflection;

namespace SurvivableArms
{
    public class Main
    {
        internal static Harmony harmony;
        internal static ILog Log { get; private set; }

        public static void Init()
        {
            Log = Logger.GetLogger("SurvivableArms");
            Logger.SetLoggerLevel("SurvivableArms", LogLevel.Debug);

            try
            {
                harmony = new Harmony("Battletech.realitymachina.SurvivableArms");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Log.Log("Mod Initialized!");
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }
    }
}
