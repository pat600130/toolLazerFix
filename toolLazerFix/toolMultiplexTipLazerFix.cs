using Elements.Core;

using FrooxEngine;

using HarmonyLib;

using ResoniteModLoader;

using static System.Net.WebRequestMethods;

namespace net.pat600.toolMultiplexTipLazerFix;
//More info on creating mods can be found https://github.com/resonite-modding-group/ResoniteModLoader/wiki/Creating-Mods
public class toolMultiplexTipLazerFix : ResoniteMod {
	internal const string VERSION_CONSTANT = "1.0.0"; //Changing the version here updates it in all locations needed
	public override string Name => "toolMultiplexTipLazerFix";
	public override string Author => "Pat600130";
	public override string Version => VERSION_CONSTANT;
	public override string Link => "https://github.com/pat600130/toolLazerFix";

	[AutoRegisterConfigKey]
	private static readonly ModConfigurationKey<bool> enabled = new ModConfigurationKey<bool>("enabled", "Should the mod be enabled", () => true); //Optional config settings

	private static ModConfiguration Config; //If you use config settings, this will be where you interface with them.

	public override void OnEngineInit() {
		Config = GetConfiguration(); //Get the current ModConfiguration for this mod
		Config.Save(true); //If you'd like to save the default config values to file
		Harmony harmony = new("net.pat600.toolMultiplexTipLazerFix");
		harmony.PatchAll();
	}

	[HarmonyPatch(typeof(ToolMultiplexer), nameof(ToolMultiplexer.LocalTip),MethodType.Getter)]
	public static class ToolMultiplexer_LocalTip_Patch {
		static void Postfix(ToolMultiplexer __instance, ref float3 __result) {
			if (!Config.GetValue(enabled))
				return;
			// tip = __instance.TipReference;
			// __result = tip!= null? tip.Position_Field : __result; 				
			float3 point = __instance.ActiveTool.Tip;
			__result = __instance.Slot.GlobalPointToLocal(in point);
		}
	}
}
