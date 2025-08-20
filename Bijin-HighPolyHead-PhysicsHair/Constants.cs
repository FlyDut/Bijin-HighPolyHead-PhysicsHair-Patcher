using NiflySharp;

namespace BijinAIOPathcer
{
    public static class Constants
    {
        public static readonly uint[] AdditionalRaceIds = [0x013741, 0x013742, 0x013743, 0x013744, 0x013746, 0x013747,
            0x013748, 0x013749, 0x02C659, 0x02C65A, 0x02C65B, 0x02C65C, 0x067CD8, 0x088794, 0x08883D,
            0x088840, 0x088844, 0x088846, 0x088884, 0x0A82B9, 0x0A82BA, 0x108272];

        public static readonly NifFileSaveOptions SaveOptions = new()
        {
            RemoveUnreferencedBlocks = false,
            SortBlocks = false
        };

        public static string BasePath { get; set; } = "";
    }
}