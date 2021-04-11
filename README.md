# Tempest

Tempest is a content mod for Risk of Rain 2.

# Setup

A lot can be found on the great [wiki resource](https://github.com/risk-of-thunder/R2Wiki/wiki).  
The structure of this project is inspired by [Komrade's tutorial](https://github.com/KomradeSpectre/AetheriumMod/blob/rewrite-master/Tutorials/Item%20Mod%20Creation.md#visual-studio-project).

1. Install [.NET SDK 2.0](https://dotnet.microsoft.com/download/visual-studio-sdks)
2. Install [BepInEx](https://github.com/risk-of-thunder/R2Wiki/wiki/BepInEx) (the mod loader)
3. Install [R2API](https://thunderstore.io/package/tristanmcpherson/R2API/)
4. Install [MMOHooks](https://thunderstore.io/package/RiskofThunder/HookGenPatcher/)
5. Copy all libs from `Risk of Rain 2/BepInEx/[core,plugins]` to `Tempest/Lib` (see below)
6. Copy relevant libs (see below) from `Risk of Rain 2/Risk Of Rain 2_Data/Managed` to `Tempest/Libs`

```
UnityEngine
UnityEngine.Networking
UnityEngine.CoreModule
Assembly-CSharp
```
