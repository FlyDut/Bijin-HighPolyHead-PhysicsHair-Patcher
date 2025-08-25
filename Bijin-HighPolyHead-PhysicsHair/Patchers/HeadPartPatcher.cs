using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BijinAIOPathcer.Patchers
{
    public static class HeadPartPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
            if (Program.settings.Value.UseHighPolyHead)
            {
                foreach (IHeadPartGetter record in mod.HeadParts)
                { 
                    if (record.EditorID == null)
                    {
                        continue;
                    }
                    if (record.EditorID.Contains("HeadHP"))
                    {
                        HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                        item.Model!.File = "KL\\High Poly Head\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                        foreach (Part part in item.Parts)
                        {
                            part.FileName = "KL\\High Poly Head\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                        }
                    }
                    if (record.EditorID.Contains("BrowsHP"))
                    {
                        HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                        item.Model!.File = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                        foreach (Part part in item.Parts)
                        {
                            part.FileName = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                        }
                    }
                }
            }
        }
        public static void ApplyValerica(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (Program.settings.Value.UseHighPolyHead)
            {
                if (cache.TryResolve<IHeadPartGetter>("ValericaHeadHP", out var record))
                {
                    HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                    item.Model!.File = "KL\\High Poly Head\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                    foreach (Part part in item.Parts)
                    {
                        part.FileName = "KL\\High Poly Head\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                    }
                }
                if (cache.TryResolve<IHeadPartGetter>("ValericaBrowsHP", out record))
                {
                    HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                    item.Model!.File = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                    foreach (Part part in item.Parts)
                    {
                        part.FileName = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                    }
                }
                
            }
        }

        public static void ApplySerana(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (Program.settings.Value.UseHighPolyHead)
            {
                if (cache.TryResolve<IHeadPartGetter>("SeranaHeadHP", out var record))
                {
                    HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                    item.Model!.File = "KL\\High Poly Head\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                    foreach (Part part in item.Parts)
                    {
                        part.FileName = "KL\\High Poly Head\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                    }
                }
                if (cache.TryResolve<IHeadPartGetter>("SeranaBrowsHP", out record))
                {
                    HeadPart item = state.PatchMod.HeadParts.GetOrAddAsOverride(record);
                    item.Model!.File = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(item.Model!.File.DataRelativePath.ToString());
                    foreach (Part part in item.Parts)
                    {
                        part.FileName = "KL\\High Poly Head\\FaceParts\\" + Path.GetFileName(part.FileName!.DataRelativePath.ToString());
                    }
                }

            }
        }
    }
}