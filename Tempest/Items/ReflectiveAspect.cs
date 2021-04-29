using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Tempest.Tempest;
using static Tempest.Utils.ItemHelpers;

namespace Tempest.Items
{
    public class ReflectiveAspect : BaseItem
    {
        public override string ItemName => "Reflective Aspect";
        public override string ItemLangTokenName => "REFLECTIVE_ASPECT";
        public override string ItemPickupDesc => "Reflects a bit of the damage you retrieve to the instigator";

        //https://github.com/risk-of-thunder/R2Wiki/wiki/Style-Reference-Sheet
        public override string ItemFullDescription =>
            $"When you are damaged, reflect <style=cIsDamage>{DamageFactor * 100}%</style> <style=cStack>(+{AdditionalDamagePerStack * 100}%) percent of the damage";

        public override string ItemLore => "A special item used by developers to \"make their own item\" or whatever that could mean. One could say that this is the origin of all items";

        public override ItemTier Tier => ItemTier.Tier1;

        public override GameObject ItemModel => MainAssets.LoadAsset<GameObject>("ReflectiveAspect.prefab");
        public override Sprite ItemIcon => MainAssets.LoadAsset<Sprite>("ReflectiveAspectIcon.png");

        public static GameObject ItemBodyModelPrefab;
        public GameObject AspectProjectile; 

        public float DamageFactor;
        public float AdditionalDamagePerStack;

        public override void Init(ConfigFile config)
        {
            SetupConfig(config);
            SetupLangTokens();
            SetupProjectile();
            SetupItemDefinition();
            SetupHooks();
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            // This function defines how an item should show up on our character
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                // Check this out to change the transform of the item during runtime and fine tune it
                // https://thunderstore.io/package/Twiner/RuntimeInspector/
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(5, 5, 5)
                }
            });

            return rules;
        }

        public override void SetupHooks()
        {
            //On.RoR2.DamageReport
            On.RoR2.HealthComponent.TakeDamage += OnTakeDamage;
            On.RoR2.CharacterBody.Update += OnUpdate;
        }

        private void OnUpdate(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                var pickupIndex = PickupCatalog.FindPickupIndex(ItemDef.itemIndex);
                PickupDropletController.CreatePickupDroplet(pickupIndex, transform.position, transform.forward * 20f);
                ModLogger.LogMessage("Dropping " + PickupCatalog.GetPickupDef(pickupIndex).internalName + " tier " + ItemDef.tier.ToString());
            }

            orig(self);
        }

        private void SetupConfig(ConfigFile config)
        {
            DamageFactor = config.Bind<float>(
                "Item: " + ItemName,
                "Damage factor", 0.2f,
                "What factor of the retrieved damage should be returned as base value?").Value;
            AdditionalDamagePerStack = config.Bind<float>(
                "Item: " + ItemName,
                "Damage factor per stack", 0.1f,
                "With how much should the returned damage increase per stack (additive)?").Value;
        }

        private void SetupProjectile()
        {
            //CharacterBody component = this.owner.GetComponent<CharacterBody>();
            //if (component)
            //{

            //    RoR2.Projectile.ProjectileManager.instance.FireProjectile()
            //    Vector2 vector = UnityEngine.Random.insideUnitCircle * this.randomCircleRange;
            //    ProjectileManager.instance.FireProjectile(this.projectilePrefab, base.transform.position + new Vector3(vector.x, 0f, vector.y), Util.QuaternionSafeLookRotation(Vector3.up + new Vector3(vector.x, 0f, vector.y)), this.owner, component.damage * this.damageCoefficient, 200f, this.crit, DamageColorIndex.Item, null, -1f);
            //}

            AspectProjectile = PrefabAPI.InstantiateClone(
                Resources.Load<GameObject>("prefabs/projectiles/MissileProjectile"), "Aspect Projectile",true);
            
            // Add required components
            // May be redundant since I already copy from missile, but it doesn't hurt to configure it again
            // Who knows, maybe they remove some of the components in the future
            TryGetComponent<ProjectileTargetComponent>(AspectProjectile);

            var projectileDirectionalTargetFinder = TryGetComponent<ProjectileDirectionalTargetFinder>(AspectProjectile);
            projectileDirectionalTargetFinder.lookRange = 25;
            projectileDirectionalTargetFinder.lookCone = 20;
            projectileDirectionalTargetFinder.targetSearchInterval = 0.1f;
            projectileDirectionalTargetFinder.onlySearchIfNoTarget = true;
            projectileDirectionalTargetFinder.allowTargetLoss = false;
            projectileDirectionalTargetFinder.testLoS = false;
            projectileDirectionalTargetFinder.ignoreAir = false;
            projectileDirectionalTargetFinder.flierAltitudeTolerance = float.PositiveInfinity;

            var projectileHoming = TryGetComponent<ProjectileSteerTowardTarget>(AspectProjectile);
            projectileHoming.rotationSpeed = 90;
            projectileHoming.yAxisOnly = false;

            var projectileSimple = TryGetComponent<ProjectileSimple>(AspectProjectile);
            projectileSimple.enableVelocityOverLifetime = true;
            projectileSimple.updateAfterFiring = true;
            projectileSimple.velocityOverLifetime = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(2, 70) });

            PrefabAPI.RegisterNetworkPrefab(AspectProjectile);
            ProjectileAPI.Add(AspectProjectile);
        }

        private void OnTakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            // If a character was killed by the world, we shouldn't do anything.
            if (!damageInfo.attacker || !damageInfo.inflictor)
                return;

            // We need an inventory to do check for our item
            var count = ItemCount(this, self.body);
            if (count > 0)
            {
                Chat.AddMessage("Reflecting projectile");

                // Generate projectile
                var projectileInfo = new FireProjectileInfo
                { 
                    position = self.transform.position,
                    rotation = Util.QuaternionSafeLookRotation(damageInfo.inflictor.transform.position - self.transform.position),
                    owner = self.gameObject,
                    target = damageInfo.inflictor,
                    //useSpeedOverride = ,
                    //useFuseOverride = ,
                    damage = damageInfo.damage * (DamageFactor + AdditionalDamagePerStack * count),
                    //force = ,
                    //crit = ,
                    damageColorIndex = DamageColorIndex.Default,
                    procChainMask = default,
                    damageTypeOverride = null,

                };

                ProjectileManager.instance.FireProjectile(projectileInfo);
            }

            orig(self,damageInfo);
        }
    }
}
