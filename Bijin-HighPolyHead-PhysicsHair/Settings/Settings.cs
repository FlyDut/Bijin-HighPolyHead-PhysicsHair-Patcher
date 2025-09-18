using Mutagen.Bethesda.Synthesis.Settings;
using static BijinAIOPathcer.Constants;

namespace BijinAIOPathcer.Settings
{
    public class Settings
    {
        public bool UseHighPolyHead { get; set; } = false;

        public bool UseYourBodyMesh { get; set; } = false;

        public bool UseYourSkin { get; set; } = false;

        [SynthesisTooltip("Use Your Skin Normal Map: Effective when Use Your Skin is enabled")]
        [SynthesisDescription("Use Your Skin Normal Map: Effective when Use Your Skin is enabled")]
        public bool UseYourSkinNormalMap { get; set; } = false;
        
        [SynthesisTooltip("Head Mesh Output: Effective when UseYourSkin is enabled, it modifies FaceGen to match your skin textures.")]
        [SynthesisDescription("Head Mesh Output: Effective when UseYourSkin is enabled, it modifies FaceGen to match your skin textures.")]
        public string HeadMeshOutput { get; set; } = "";

        [SynthesisTooltip("When you do not check this option, it will use the color of the \"Hi babe\" hairstyle by default.")]
        [SynthesisDescription("When you do not check this option, it will use the color of the \"Hi babe\" hairstyle by default.")]
        public AdrianneOption Adrianne = new();

        public ValericaOption Valerica = new();
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

