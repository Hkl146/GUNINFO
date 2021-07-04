using MelonLoader;
using System.Reflection;

[assembly: AssemblyTitle(GUNINFO.BuildInfo.Description)]
[assembly: AssemblyDescription(GUNINFO.BuildInfo.Description)]
[assembly: AssemblyCompany(GUNINFO.BuildInfo.Company)]
[assembly: AssemblyProduct(GUNINFO.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + GUNINFO.BuildInfo.Author)]
[assembly: AssemblyTrademark(GUNINFO.BuildInfo.Company)]
[assembly: AssemblyVersion(GUNINFO.BuildInfo.Version)]
[assembly: AssemblyFileVersion(GUNINFO.BuildInfo.Version)]
[assembly: MelonInfo(typeof(GUNINFO.GUNINFO), GUNINFO.BuildInfo.Name, GUNINFO.BuildInfo.Version, GUNINFO.BuildInfo.Author, GUNINFO.BuildInfo.DownloadLink)]


// Create and Setup a MelonGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonGameAttribute is found or any of the Values for any MelonGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonMame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]