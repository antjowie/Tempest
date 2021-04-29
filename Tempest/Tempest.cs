using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Linq;
using UnityEngine;

namespace Tempest
{
    // It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGUID, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(LanguageAPI))]
    public class Tempest : BaseUnityPlugin
    {
        public const string ModGUID = "com.tomodachi.tempest";
        public const string ModName = "Tempest";
        public const string ModVer = "0.0.1";

        // We need our item definition to persist through our functions, and therefore make it a class field.
        private static ItemDef myItemDef;

        public void Awake()
        {
            myItemDef = new ItemDef
            {
                name = "EXAMPLE_CLOAKONKILL_NAME",
                nameToken = "EXAMPLE_CLOAKONKILL_NAME",
                pickupToken = "EXAMPLE_CLOAKONKILL_PICKUP",
                descriptionToken = "EXAMPLE_CLOAKONKILL_DESC",
                loreToken = "EXAMPLE_CLOAKONKILL_LORE",
                tier = ItemTier.Tier2,
                //pickupIconPath = "Textures/MiscIcons/texMysteryIcon",
                //pickupModelPath = "Prefabs/PickupModels/PickupMystery",
                canRemove = true,
                hidden = false
            };
            AddTokens();

            // You can add your own display rules here, where the first argument passed are the
            // default display rules: the ones used when no specific display rules for a character are found.
            // For this example, we are omitting them, as they are quite a pain to set up.
            var displayRules = new ItemDisplayRuleDict(null);

            ItemAPI.Add(new CustomItem(myItemDef, displayRules));

            // But now we have defined an item, but it doesn't do anything yet. So we'll need to define that ourselves.
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport report)
        {
            // If a character was killed by the world, we shouldn't do anything.
            if (!report.attacker || !report.attackerBody)
                return;

            CharacterBody attacker = report.attackerBody;
            // We need an inventory to do check for our item
            if (attacker.inventory)
            {
                ChatMessage.SendColored("Rolled the item", Color.red);
                // store the amount of our item we have
                int itemCount = attacker.inventory.GetItemCount(myItemDef.itemIndex);
                if (itemCount > 0 &&
                    // Roll for our 5% chance.
                    Util.CheckRoll(5, attacker.master))
                {
                    // Since we passed all checks, we now give our attacker the cloaked buff.
                    // For some reason BuffIndex enum only has -1 for us?
                    //attacker.AddTimedBuff(BuffIndex.Cloak, 3 + (garbCount));
                }
            }
        }

        private void AddTokens()
        {
            R2API.LanguageAPI.Add("EXAMPLE_CLOAKONKILL_NAME", "Cuthroat's Garb");
            R2API.LanguageAPI.Add("EXAMPLE_CLOAKONKILL_PICKUP", "Chance to cloak on kill");
            R2API.LanguageAPI.Add("EXAMPLE_CLOAKONKILL_DESC", "Whenever you <style=cIsDamage>kill an enemy</style>, you have a <style=cIsUtility>5%</style> chance to cloak for <style=cIsUtility>4s</style> <style=cStack>(+1s per stack)</style.");
            R2API.LanguageAPI.Add("EXAMPLE_CLOAKONKILL_LORE", "Those who visit in the night are either praying for a favour, or preying on a neighbour.");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // Get a random item from all possible pickups
                var items = PickupCatalog.allPickups;
                int index = Random.Range(0, items.Count());
                //PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(myItemDef.itemIndex), transform.position, transform.forward * 20f);
                PickupDropletController.CreatePickupDroplet(items.ElementAt(index).pickupIndex, transform.position, transform.forward * 20f);
            }
        }
    }
}