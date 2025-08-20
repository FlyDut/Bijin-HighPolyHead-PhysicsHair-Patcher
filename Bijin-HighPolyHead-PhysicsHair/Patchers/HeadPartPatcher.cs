using Mutagen.Bethesda;
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
    }
}