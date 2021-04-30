using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using static Tempest.Tempest;

namespace Tempest.Equipment
{
    class Gemini : BaseEquipment
    {
        public override string EquipmentName => "Gemini";

        public override string EquipmentLangTokenName => "Gemini";

        public override string EquipmentPickupDesc => "And now there were two";

        public override string EquipmentFullDescription => "IDK";

        public override string EquipmentLore => "A star next to the moon";

        public override GameObject EquipmentModel => MainAssets.LoadAsset<GameObject>("Gemini.prefab");

        public override Sprite EquipmentIcon => MainAssets.LoadAsset<Sprite>("GeminiIcon.png");

        public override float Cooldown => 6f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    followerPrefab = EquipmentModel, // the prefab that will show up on the survivor
                    childName = "Chest", // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
                    localScale = new Vector3(.02f, .02f, .02f), // scale the model
                    localAngles = new Vector3(0f, 180f, 0f), // rotate the model
                    localPos = new Vector3(-0.35f, -0.1f, 0f), // position offset relative to the childName, here the survivor Chest        }
                }
            });

            return rules;
        }

        public override void Init(ConfigFile config)
        {
            SetupConfig(config);
            SetupLangTokens();
            SetupItemDefinition();
            SetupHooks();
        }

        private void SetupConfig(ConfigFile config)
        {

        }


        public override void SetupHooks()
        {

        }


        protected override bool PerformEquipmentAction(EquipmentSlot slot)
        {
            return true;
        }

    }
}