using BepInEx;
using HarmonyLib;
using RWF.GameModes;
using UnityEngine;
using TMPro;

namespace ColouredNames
{
    // These are the mods required for our Mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("io.olavim.rounds.rwf", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our Mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our Mod Is associated with
    [BepInProcess("Rounds.exe")]
    public class Main : BaseUnityPlugin
    {
        private const string ModId = "dev.scyye.rounds.namecolours";
        private const string ModName = "Name Colours";
        public const string Version = "1.0.0";

        public static Main instance { get; private set; }

        public void Log(string str)
        {
            UnityEngine.Debug.Log($"[{ModName}] {str}");
        }

        void Awake()
        {
            instance = this;
            Debug.Log(ModId);

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
    }

    // Sets the class being patched
    [HarmonyPatch(typeof(RWFGameMode))]
    public class GameStartPatch
    {
        // Sets the method being patched
        [HarmonyPatch(nameof(RWFGameMode.StartGame))]

        // Tells harmony this should happen BEFORE the original StartGame method
        [HarmonyPrefix]
        static void Prefix()
        {

            // Loops through all players
            foreach (Player player in PlayerManager.instance.players)
            {
                // The GameObject of the player
                GameObject playerObject = player.gameObject;

                // The PlayerName that houses the TextMeshProUGUI for the player's name
                var playerName = playerObject.transform.GetChild(4).GetChild(0).GetChild(0).GetChild(1);

                // The TextMeshProUGUI of our PlayerName
                var textMeshPro = playerName.GetComponent<TextMeshProUGUI>();

                // Sets the colour of the name text to the the colour of the player
                textMeshPro.color = player.GetTeamColors().color;
            }
        }
    }
}
