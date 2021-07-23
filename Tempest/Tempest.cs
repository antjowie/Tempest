using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tempest.Equipment;
using Tempest.Items;
using Tempest.Systems;
using Tempest.Utils;
using UnityEngine;

namespace Tempest
{
    /**
     * Tempest is a general Risk of Rain 2 content mod. A little experiment project in which we add items, equipment and whatever else comes to mind
     * Special thanks to KomradeSpectre. His mod Aetherium has been referenced a lot during the development of this project
     * You can see that mod here: https://github.com/KomradeSpectre/AetheriumMod
     */

    //[BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGUID, ModName, ModVer)]
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ProjectileAPI),
                              nameof(LanguageAPI), nameof(PrefabAPI), nameof(ResourcesAPI))]
    public class Tempest : BaseUnityPlugin
    {
        public const string ModGUID = "com.tomodachi.tempest";
        public const string ModName = "Tempest";
        public const string ModVer = "0.0.1";

        public static BepInEx.Logging.ManualLogSource ModLogger;

        public event EventHandler OnUpdate = delegate { };

        public List<BaseItem> Items = new List<BaseItem>();
        public List<BaseEquipment> Equipments = new List<BaseEquipment>();
        public List<BaseSystem> Systems = new List<BaseSystem>();

        public static AssetBundle MainAssets;

        public static Dictionary<string, ItemDef> CustomItems = new Dictionary<string, ItemDef>();

        public void Awake()
        {
            
            ModLogger = this.Logger;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tempest.tempest_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

            //ModLogger.LogInfo("----------------------ASSETS--------------------");
            //foreach (var asset in MainAssets.GetAllAssetNames())
            //{
            //    ModLogger.LogMessage(asset);
            //}

            if (Helpers.InDebugMode())
            {
                ModLogger.LogWarning("Tempest has been compiled in debug mode!");
            }

            ModLogger.LogInfo("----------------------SYSTEMS--------------------");
            var SystemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BaseSystem)));
            foreach (var systemType in SystemTypes)
            {
                BaseSystem system = (BaseSystem)System.Activator.CreateInstance(systemType);

                if (!Helpers.InDebugMode() && system.StripFromRelease)
                {
                    ModLogger.LogWarning("System: " + systemType.Name + " stripped from release!");
                    continue;
                }

                system.Init(this);
                ModLogger.LogInfo("System: " + systemType.Name + " initialized!");
            }

            ModLogger.LogInfo("----------------------ITEMS--------------------");
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BaseItem)));
            foreach (var itemType in ItemTypes)
            {
                BaseItem item = (BaseItem)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init(Config);
                    CustomItems.Add(item.ItemDef.name, item.ItemDef);
                    ModLogger.LogInfo("Item: " + item.ItemName + " initialized!");
                }
                else
                {
                    ModLogger.LogWarning("Item: " + item.ItemName + " disabled!");
                }
            }

            ModLogger.LogInfo("----------------------EQUIPMENT--------------------");
            var EquipmetTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BaseEquipment)));
            foreach (var EquipmetType in EquipmetTypes)
            {
                BaseEquipment equipment = (BaseEquipment)System.Activator.CreateInstance(EquipmetType);
                equipment.Init(Config);

                ModLogger.LogInfo("Equpment: " + equipment.EquipmentName + " initialized!");
            }

            ModLogger.LogInfo("----------------------TEMPEST DONE--------------------");
        }

        public bool ValidateItem(BaseItem item, List<BaseItem> itemList)
        {
            var enabled = Config.Bind<bool>("Item: " + item.ItemName, "Enable Item?", true, "Should this item appear in runs?").Value;
            var aiBlacklist = Config.Bind<bool>("Item: " + item.ItemName, "Blacklist Item from AI Use?", false, "Should the AI not be able to obtain this item?").Value;

            if (enabled)
            {
                itemList.Add(item);
                if (aiBlacklist)
                {
                    item.AIBlacklisted = true;
                }
            }
            return enabled;
        }

        public void Update()
        {
            OnUpdate(this, EventArgs.Empty);
        }
    }
}