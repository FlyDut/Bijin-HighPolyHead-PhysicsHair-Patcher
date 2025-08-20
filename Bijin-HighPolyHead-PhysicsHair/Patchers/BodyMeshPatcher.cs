using BijinAIOPathcer.Settings;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using NiflySharp;
using System.Collections.Generic;

namespace BijinAIOPathcer.Patchers
{
    public static class BodyMeshPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
            if (Program.settings.Value.UseYourBodyMesh)
            {
                foreach (IArmorAddonGetter record in mod.ArmorAddons)
                {
                    if(record.EditorID == null)
                    {
                        continue;
                    }
                    if (record.EditorID.Contains("TorsoAP"))
                    {
                        Model nif = new();
                        ArmorAddon item = state.PatchMod.ArmorAddons.GetOrAddAsOverride(record);
                        string FilePath = "actors\\character\\character assets\\femalebody_1.nif";
                        nif.File = FilePath;
                        item.WorldModel = new GenderedItem<Model?>(null, nif);
                    }else if (record.EditorID.Contains("HandsAP"))
                    {
                        Model nif = new();
                        ArmorAddon item = state.PatchMod.ArmorAddons.GetOrAddAsOverride(record);
                        string FilePath = "actors\\character\\character assets\\femalehands_1.nif";
                        nif.File = FilePath;
                        item.WorldModel = new GenderedItem<Model?>(null, nif);
                    }
                    else if (record.EditorID.Contains("FeetAP"))
                    {
                        Model nif = new();
                        ArmorAddon item = state.PatchMod.ArmorAddons.GetOrAddAsOverride(record);
                        string FilePath = "actors\\character\\character assets\\femalefeet_1.nif";
                        nif.File = FilePath;
                        item.WorldModel = new GenderedItem<Model?>(null, nif);
                    }
                }
            }
        }
    }
}