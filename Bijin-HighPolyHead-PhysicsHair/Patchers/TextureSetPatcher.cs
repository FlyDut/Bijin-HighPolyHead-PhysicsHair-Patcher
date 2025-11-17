using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BijinAIOPathcer.Patchers
{
    public static class TextureSetPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
            if (Program.settings.Value.UseYourBodySkin)
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
                        item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                    }
                    else if (record.EditorID.Contains("HandsTex"))
                    {
                        TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                        item.Diffuse = "actors\\character\\female\\femalehands_1.dds";
                        item.GlowOrDetailMap = "actors\\character\\female\\femalehands_1_sk.dds";
                        item.BacklightMaskOrSpecular = "actors\\character\\female\\femalehands_1_s.dds";
                        item.NormalOrGloss = "actors\\character\\female\\femalehands_1_msn.dds";
                    }
                }
            }
        }
        public static void ApplyValerica(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (Program.settings.Value.UseYourBodySkin)
            {
                if (cache.TryResolve<ITextureSetGetter>("ValericaTorsoTex", out var record))
                {
                    TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                    item.Diffuse = "actors\\character\\female\\femalebody_1.dds";
                    item.GlowOrDetailMap = "actors\\character\\female\\femalebody_1_sk.dds";
                    item.BacklightMaskOrSpecular = "actors\\character\\female\\femalebody_1_s.dds";
                    item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                }
                if (cache.TryResolve<ITextureSetGetter>("ValericaHandsTex", out record))
                {
                    TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                    item.Diffuse = "actors\\character\\female\\femalebody_1.dds";
                    item.GlowOrDetailMap = "actors\\character\\female\\femalebody_1_sk.dds";
                    item.BacklightMaskOrSpecular = "actors\\character\\female\\femalebody_1_s.dds";
                    item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                }
            }
        }

        public static void ApplySerana(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (Program.settings.Value.UseYourBodySkin)
            {
                if (cache.TryResolve<ITextureSetGetter>("SeranaTorsoTex", out var record))
                {
                    TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                    item.Diffuse = "actors\\character\\female\\femalebody_1.dds";
                    item.GlowOrDetailMap = "actors\\character\\female\\femalebody_1_sk.dds";
                    item.BacklightMaskOrSpecular = "actors\\character\\female\\femalebody_1_s.dds";
                    item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                }
                if (cache.TryResolve<ITextureSetGetter>("SeranaHandsTex", out record))
                {
                    TextureSet item = state.PatchMod.TextureSets.GetOrAddAsOverride(record);
                    item.Diffuse = "actors\\character\\female\\femalebody_1.dds";
                    item.GlowOrDetailMap = "actors\\character\\female\\femalebody_1_sk.dds";
                    item.BacklightMaskOrSpecular = "actors\\character\\female\\femalebody_1_s.dds";
                    item.NormalOrGloss = "actors\\character\\female\\femalebody_1_msn.dds";
                }
            }
        }
    }
}