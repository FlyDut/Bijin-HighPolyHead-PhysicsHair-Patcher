using Mutagen.Bethesda.Synthesis.Settings;
using static BijinAIOPathcer.Constants;

namespace BijinAIOPathcer.Settings
{
    public class Settings
    {
        public bool UseHighPolyHead { get; set; } = false;

        public bool UseYourBodyMesh { get; set; } = false;

        public bool UseYourBodySkin { get; set; } = false;

        //[SynthesisTooltip("Use Your Skin Normal Map: Effective when Use Your Skin is enabled")]
        //[SynthesisDescription("Use Your Skin Normal Map: Effective when Use Your Skin is enabled")]
        //public bool UseYourBodySkinNormalMap { get; set; } = false;

        [SynthesisTooltip("When you do not check this option, it will use the color of the \"Hi babe\" hairstyle by default.")]
        [SynthesisDescription("When you do not check this option, it will use the color of the \"Hi babe\" hairstyle by default.")]
        public AdrianneOption Adrianne = new();

        public ValericaOption Valerica = new();

        [SynthesisTooltip("separate esps with a semicolon (;)")]
        [SynthesisDescription("separate esps with a semicolon (;)")]
        public string ModsToSkip { get; set; } = "";
    }

    public class AdrianneOption
    {
        public AdrianneHairColor HairColor { get; set; }

}

    public class ValericaOption
    {
        public ValericaHairColor HairColor { get; set; }

    }
}

