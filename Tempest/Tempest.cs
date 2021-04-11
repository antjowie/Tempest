using BepInEx;
using R2API;
using R2API.Utils;

namespace Tempest
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(ModGUID, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(ResourcesAPI), nameof(ItemAPI))]
    public class Tempest : BaseUnityPlugin
    {
        public const string ModGUID = "com.tomodachi.tempest";
        public const string ModName = "Tempest";
        public const string ModVer = "0.0.1";

        public void Awake()
        {
            Logger.LogInfo($"Loaded {ModName}");
            // Seems to be deprecated. TODO: Research ResourceAPI
            //using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tempest.tempest_assets"))
            //{
            //    var bundle = AssetBundle.LoadFromStream(stream);
            //    var provider = new AssetBundleResourcesProvider($"@{ModName}", bundle);
            //    ResourcesAPI.AddProvider(provider);
            //}
        }
    }
}