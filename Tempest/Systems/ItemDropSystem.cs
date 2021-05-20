using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Tempest.Systems
{
    public class ItemDropSystem : BaseSystem
    {
        public override bool StripFromRelease => true;

        public override void Init(Tempest tempest)
        {
            tempest.OnUpdate += (object sender, EventArgs e) =>
            {
                // Spawn some random items used to test your own items
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

                    int index = UnityEngine.Random.Range(0, list.Count);
                    Tempest.ModLogger.LogInfo($"index {index} cap {list.Count}");

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
            };
        }
    }
}
