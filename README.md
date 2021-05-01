# Tempest

Tempest is a content mod for Risk of Rain 2.

# Setup

A lot can be found on the great [wiki resource](https://github.com/risk-of-thunder/R2Wiki/wiki).

1. Install [.NET SDK 2.0](https://dotnet.microsoft.com/download/visual-studio-sdks)
2. Install [BepInEx](https://github.com/risk-of-thunder/R2Wiki/wiki/BepInEx) (the mod loader)
3. Libs in the Managed folder are from RoR2 installation and stripped

The solution copies binaries to the RoR2 installation after building. When you build the mod, a ror2.txt file is created
in the Resources folder. Put your RoR2 installation path in that file. An example of such a path is the following: `D:\Games\SteamLibrary\steamapps\common\Risk of Rain 2`
