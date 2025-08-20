using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BijinAIOPathcer.Patchers
{
    public static class TextureSetPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
            if (Program.settings.Value.UseYourSkin)
            {
                foreach (ITextureSetGetter record in mod.TextureSets)
                {
                    if (record.EditorID == null)
                    {
                        continue;
                    }
                    if (record.EditorID.Contains("TorsoTex"))
                    {
                        TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                        item.Diffuse = "actors\\character\\female\\femalebody_1.dds";
                        item.GlowOrDetailMap = "actors\\character\\female\\femalebody_1_sk.dds";
                        item.BacklightMaskOrSpecular = "actors\\character\\female\\femalebody_1_s.dds";
                        if (Program.settings.Value.UseYourSkinNormalMap)
                        {
                            item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                        }
                    }
                    else if (record.EditorID.Contains("HandsTex"))
                    {
                        TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                        item.Diffuse = "actors\\character\\female\\femalehands_1.dds";
                        item.GlowOrDetailMap = "actors\\character\\female\\femalehands_1_sk.dds";
                        item.BacklightMaskOrSpecular = "actors\\character\\female\\femalehands_1_s.dds";
                        if (Program.settings.Value.UseYourSkinNormalMap)
                        {
                            item.NormalOrGloss = "actors\\character\\female\\femalehands_1_msn.dds";
                        }
                    }
                    else if (record.EditorID.Contains("HandsTex"))
                    {
                        TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                        item.Diffuse = "actors\\character\\female\\femalehands_1.dds";
                        item.GlowOrDetailMap = "actors\\character\\female\\femalehands_1_sk.dds";
                        item.BacklightMaskOrSpecular = "actors\\character\\female\\femalehands_1_s.dds";
                        if (Program.settings.Value.UseYourSkinNormalMap)
                        {
                            item.NormalOrGloss = "actors\\character\\female\\femalehands_1_msn.dds";
                        }
                    }
                }
            }
        }
    }
}