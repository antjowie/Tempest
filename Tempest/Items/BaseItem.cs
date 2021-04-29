using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx.Configuration;
using System.Linq;

namespace Tempest.Items
{
    public abstract class BaseItem
    {
        public abstract string ItemName { get; }
        public abstract string ItemLangTokenName { get; }
        public abstract string ItemPickupDesc { get; }
        public abstract string ItemFullDescription { get; }
        public abstract string ItemLore { get; }

        public abstract ItemTier Tier { get; }
        // For example, Utility means this item can spawn in a utility chest
        public virtual ItemTag[] ItemTags { get; set; } = new ItemTag[] { };

        public abstract GameObject ItemModel { get; }
        public abstract Sprite ItemIcon { get; }

        public virtual bool CanRemove { get; } = true;
        public virtual bool Hidden { get; } = false;
        public virtual bool AIBlacklisted { get; set; } = false;

        public ItemDef ItemDef;

        // You can't enforce a child class to use a certain constructor type
        // Init is a glorified constructor, but it enforces the config parameter
        public abstract void Init(ConfigFile config);
        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void SetupLangTokens()
        {
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_NAME", ItemName);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_PICKUP", ItemPickupDesc);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_DESCRIPTION", ItemFullDescription);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_LORE", ItemLore);
        }

        protected void SetupItemDefinition()
        {
            if (AIBlacklisted)
            {
                ItemTags = new List<ItemTag>(ItemTags) { ItemTag.AIBlacklist }.ToArray();
            }

            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "ITEM_" + ItemLangTokenName;
            ItemDef.nameToken = "ITEM_" + ItemLangTokenName + "_NAME";
            ItemDef.pickupToken = "ITEM_" + ItemLangTokenName + "_PICKUP";
            ItemDef.descriptionToken = "ITEM_" + ItemLangTokenName + "_DESCRIPTION";
            ItemDef.loreToken = "ITEM_" + ItemLangTokenName + "_LORE";
            ItemDef.pickupModelPrefab = ItemModel;
            ItemDef.pickupIconSprite = ItemIcon;
            ItemDef.hidden = false;
            ItemDef.canRemove = CanRemove;
            ItemDef.tier = Tier;

            if (ItemTags.Length > 0) { ItemDef.tags = ItemTags; }

            ItemAPI.Add(new CustomItem(ItemDef, CreateItemDisplayRules()));
        }

        public abstract void SetupHooks();
    }
}
