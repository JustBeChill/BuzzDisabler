using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using System.Reflection;

namespace BuzzDisabler
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [HarmonyPatch(typeof(FlickeringNeonSign))]
        [HarmonyPatch(nameof(FlickeringNeonSign.OnEnable), MethodType.Normal)]
        public class FlickeringNeonSignPatch
        {
            public static bool Prefix(FlickeringNeonSign __instance, ref float ____sparksVolume)
            {
                ____sparksVolume = 0f;
                return true;
            }
        }

        internal static string hid = "com.justbechill.BuzzDisabler";
        internal Harmony harmony;
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("BuzzDisabler initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            new GameObject("BuzzDisablerController").AddComponent<BuzzDisablerController>();

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }

        [OnEnable]
        public void OnEnable()
        {
            harmony = new Harmony(hid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }



        public static void Postfix(FlickeringNeonSign __instance)
        {

        }
    }
}
