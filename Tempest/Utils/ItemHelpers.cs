using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using Tempest.Items;
using UnityEngine;

namespace Tempest.Utils
{
    public class ItemHelpers
    {
        public static int ItemCount(BaseItem item, CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(item.ItemDef);
        }

        public static int ItemCount(BaseItem item, CharacterMaster body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(item.ItemDef);
        }

        public static CharacterModel.RendererInfo[] ItemDisplaySetup(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }

            return renderInfos;
        }

        // Gets the component, if it doesn't exist, add it
        public static T TryGetComponent<T>(GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() ?? obj.AddComponent<T>();
        }
    }
}
