using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Cache.Internals.Implementations;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using NiflySharp;
using NiflySharp.Blocks;
using Noggog;
using System.Drawing;
namespace BijinAIOPathcer.Patchers
{
    public static class HairColor
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
            ColorRecord hairColor = new(state.PatchMod);
            if (Program.settings.Value.AdrianneUseVanillaColor) {
                hairColor.Color = ColorTranslator.FromHtml("#5e5248");
            }
            else
            {
                hairColor.Color = ColorTranslator.FromHtml("#362424");
            }
            state.PatchMod.Colors.Add(hairColor);
        }
    }
}