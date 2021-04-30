using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tempest.Equipment;
using Tempest.Items;
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

        internal static BepInEx.Logging.ManualLogSource ModLogger;

        public List<BaseItem> Items = new List<BaseItem>(); 
        public List<BaseEquipment> Equipments = new List<BaseEquipment>();

        public static AssetBundle MainAssets;
        public void Awake()
        {
            ModLogger = this.Logger;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tempest.tempest_assets"))
            {
                MainAssets = AssetBundle.LoadFromStream(stream);
            }

            ModLogger.LogInfo("----------------------ASSETS--------------------");
            foreach(var asset in MainAssets.GetAllAssetNames())
            {
                ModLogger.LogMessage(asset);
            }

            // Add all items we've created
            ModLogger.LogInfo("----------------------ITEMS--------------------");

            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BaseItem)));
            foreach (var itemType in ItemTypes)
            {
                BaseItem item = (BaseItem)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init(Config);

                    ModLogger.LogInfo("Item: " + item.ItemName + " Initialized!");
                }
            }
            
            var EquipmetTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(BaseEquipment)));
            foreach (var EquipmetType in EquipmetTypes)
            {
                BaseEquipment equipment = (BaseEquipment)System.Activator.CreateInstance(EquipmetType);
                equipment.Init(Config);

               ModLogger.LogInfo("Item: " + equipment.EquipmentName + " Initialized!");
            }
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
            // Spawn a random item Tier 1 item
            int tier = 0;
            if (Input.GetKeyDown(KeyCode.F1)) tier = 1;
            if (Input.GetKeyDown(KeyCode.F2)) tier = 2;
            if (Input.GetKeyDown(KeyCode.F3)) tier = 3;
            if (Input.GetKeyDown(KeyCode.F4)) tier = 4;
            if (Input.GetKeyDown(KeyCode.F5)) 
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(RoR2Content.Artifacts.commandArtifactDef.artifactIndex), transform.position, transform.forward * 20f);

            }
            //if (Input.GetKeyDown(KeyCode.F5))
            //{
            //    var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
            //    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(Items[0].ItemDef.itemIndex), transform.position, transform.forward * 20f);
            //}

            if (tier != 0)
            {
                List<PickupIndex> list = new List<PickupIndex>();
                switch (tier)
                {
                    case 1:
                        list = Run.instance.availableTier1DropList;
                        break;
                    case 2:
                        list = Run.instance.availableTier2DropList;
                        break;
                    case 3:
                        list = Run.instance.availableTier3DropList;
                        break;
                    case 4:
                        list = Run.instance.availableEquipmentDropList;
                        break;
                }

                int index = Random.Range(0, list.Count);
                ModLogger.LogInfo($"index {index} cap {list.Count}");

                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(list[index], transform.position, transform.forward * 20f);

                //PickupDropletController
                //var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                //ItemDropAPI.ChestItems.Tier1
                //// Get a random item from all possible pickups
                //var items = PickupCatalog.allPickups;
                //int index = Random.Range(0, items.Count());
                ////PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(myItemDef.itemIndex), transform.position, transform.forward * 20f);
                //PickupDropletController.CreatePickupDroplet(items.ElementAt(index).pickupIndex, transform.position, transform.forward * 20f);
            }
        }
    }
}