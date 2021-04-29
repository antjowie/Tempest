using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tempest.Equipment
{
    public abstract class BaseEquipment
    {
		public abstract string EquipmentName { get; }
		public abstract string EquipmentLangTokenName { get; }
		public abstract string EquipmentPickupDesc { get; }
		public abstract string EquipmentFullDescription { get; }
		public abstract string EquipmentLore { get; }

		public abstract GameObject EquipmentModel { get; }
		public abstract Sprite EquipmentIcon { get; }

        public virtual bool AppearsInSinglePlayer { get; } = true;
        public virtual bool AppearsInMultiPlayer { get; } = true;
        public virtual bool CanDrop { get; } = true;
        public virtual float Cooldown { get; } = 60f;
        public virtual bool EnigmaCompatible { get; } = true;
        public virtual bool IsBoss { get; } = false;
        public virtual bool IsLunar { get; } = false;

        public EquipmentDef EquipmentDef;

        // You can't enforce a child class to use a certain constructor type
        // Init is a glorified constructor, but it enforces the config parameter
        public abstract void Init(ConfigFile config);
        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void SetupLangTokens()
        {
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_NAME", EquipmentName);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_PICKUP", EquipmentPickupDesc);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_DESCRIPTION", EquipmentFullDescription);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_LORE", EquipmentLore);
        }

        protected void SetupItemDefinition()
        {
            EquipmentDef = ScriptableObject.CreateInstance<EquipmentDef>();
            EquipmentDef.name = "EQUIPMENT_" + EquipmentLangTokenName;
            EquipmentDef.nameToken = "EQUIPMENT_" + EquipmentLangTokenName + "_NAME";
            EquipmentDef.pickupToken = "EQUIPMENT_" + EquipmentLangTokenName + "_PICKUP";
            EquipmentDef.descriptionToken = "EQUIPMENT_" + EquipmentLangTokenName + "_DESCRIPTION";
            EquipmentDef.loreToken = "EQUIPMENT_" + EquipmentLangTokenName + "_LORE";
            EquipmentDef.pickupModelPrefab = EquipmentModel;
            EquipmentDef.pickupIconSprite = EquipmentIcon;
            EquipmentDef.appearsInSinglePlayer = AppearsInSinglePlayer;
            EquipmentDef.appearsInMultiPlayer = AppearsInMultiPlayer;
            EquipmentDef.canDrop = CanDrop;
            EquipmentDef.cooldown = Cooldown;
            EquipmentDef.enigmaCompatible = EnigmaCompatible;
            EquipmentDef.isBoss = IsBoss;
            EquipmentDef.isLunar = IsLunar;

            var itemDisplayRuleDisct = CreateItemDisplayRules();
            ItemAPI.Add(new CustomEquipment(EquipmentDef, itemDisplayRuleDisct));

            On.RoR2.EquipmentSlot.PerformEquipmentAction += TryPerformEquipmentAction;
        }

        private bool TryPerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentDef equipmentDef)
        {
            if (equipmentDef == EquipmentDef)
            {
                return PerformEquipmentAction(self);
            }
            else
            {
                return orig(self, equipmentDef);
            }
        }
        protected abstract bool PerformEquipmentAction(EquipmentSlot slot);

        public abstract void SetupHooks();
    }
}
