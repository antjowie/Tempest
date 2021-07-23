# Tempest

Tempest is a content mod for Risk of Rain 2. Specifics are still under heavy development and not set in stone!

# Setup

A lot can be found on the great [wiki resource](https://github.com/risk-of-thunder/R2Wiki/wiki). To start developing for this mod, do the following:

1. Install [Visual Studio 2019 with C# workload](https://visualstudio.microsoft.com/vs/) (NOTE: You may need to install [.NET SDK 2.0](https://dotnet.microsoft.com/download/visual-studio-sdks) explicitly. For me the workload was enough).
2. Setup RoR2 to support mods.  
   `NOTE: Using R2ModMan may suffice, but the following are the bare minimum that is required`  
   a. Install [BepInEx](https://github.com/risk-of-thunder/R2Wiki/wiki/BepInEx).  
   b. Install [R2API](https://github.com/risk-of-thunder/R2Wiki/wiki/BepInEx).  
   c. Install [HookGenPatcher](https://github.com/risk-of-thunder/R2Wiki/wiki/BepInEx).
    > The assemblies from these libraries are included with the project, so if you manually install them, be sure to update the assemblies in `Tempest/Libs/Mods`. Same goes for the Managed assemblies (which should be stripped, they contain game code and can be found in your RoR2 installation).
3. Build the project in Visual Studio.
    > The solution copies binaries to the RoR2 installation after building. When you build the mod, a ror2.txt file is created
    > in the Resources folder. Put your RoR2 installation path in that file. An example of such a path is the following: `D:\Games\SteamLibrary\steamapps\common\Risk of Rain 2`
