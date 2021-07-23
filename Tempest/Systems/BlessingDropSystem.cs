using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using Tempest.Items;

namespace Tempest.Systems
{
    class BlessingDropSystem : BaseSystem
    {
        public override bool StripFromRelease => false;
        
        protected int Counter = 0;
        
        public override void Init(Tempest tempest)
        {
            On.RoR2.CharacterDeathBehavior.OnDeath += DeathCounter;
        }

        private void DeathCounter(On.RoR2.CharacterDeathBehavior.orig_OnDeath orig, CharacterDeathBehavior self)
        {
            Counter++;

            if (Counter == 1)
            {
                ItemIndex i = Tempest.CustomItems["ITEM_REFLECTIVE_ASPECT"].itemIndex;
                Chat.AddMessage("How grim");
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(i), transform.position, transform.forward * 20f);
            }
            orig(self);
        }

    }
}
