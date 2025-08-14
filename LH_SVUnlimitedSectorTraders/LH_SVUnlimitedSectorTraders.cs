using UnityEngine;
using HarmonyLib;
using BepInEx;
using System.Collections.Generic;

namespace LH_SVUnlimitedSectorTraders
{

    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class LH_SVUnlimitedSectorTraders : BaseUnityPlugin
    {
        public const string pluginGuid = "LH_SVUnlimitedSectorTraders";
        public const string pluginName = "LH_SVUnlimitedSectorTraders";
        public const string pluginVersion = "0.0.1";

        public void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(LH_SVUnlimitedSectorTraders));
        }

        [HarmonyPatch(typeof(GameManager), "PopulateSector")]
        [HarmonyPostfix]

        public static void UnlimitTraders(TSector ___currSector, bool afterStart, GameManager __instance)
        {
            if (!___currSector.IsBeingAttacked && GameData.data.CharacterSystem.GetTradersInSector(___currSector.Index, false).Count > 0 && afterStart)
            {
                //Debug.Log("I have " + GameData.data.CharacterSystem.GetTradersInSector(___currSector.Index, false).Count + " traders at the door, spawning...");
                List<DynamicCharacter> tradersInSector = GameData.data.CharacterSystem.GetTradersInSector(___currSector.Index, false);
                for (int i = 0; i < tradersInSector.Count; i++) 
                {
                    DynamicCharacter dynamicCharacter = tradersInSector[i];
                    AccessTools.Method(typeof(GameManager), "SpawnNPCTraderShip").Invoke(__instance, new object[] { dynamicCharacter, true, null });
                    tradersInSector[i].SetActiveState(true);
                    //Debug.Log("Spawned trader " + tradersInSector[i].name);
                }                
            }
        }
    }
}
