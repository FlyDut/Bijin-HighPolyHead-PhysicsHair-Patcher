using DynamicData;
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
    public static class NpcPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {

            ModKey skyrimModKey = ModKey.FromNameAndExtension("Skyrim.esm");
            if (!state.LinkCache.TryResolve<IRaceGetter>(new FormKey(skyrimModKey, 0x000019), out var defaultRace))
            {
                return;
            }
            ExtendedList<IFormLinkGetter<IRaceGetter>> additionalRaces = [];
            foreach (uint key in Constants.AdditionalRaceIds)
            {
                if (state.LinkCache.TryResolve<IRaceGetter>(new FormKey(skyrimModKey, key), out var item))
                {
                    additionalRaces.Add(item.ToLinkGetter());
                }
            }
            ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache = mod.ToImmutableLinkCache();
            HashSet<string> handledNpc = [];
            if (!Program.settings.Value.HighPolyHeadOutput.Equals(""))
            {
                CreateFolder(Program.settings.Value.HighPolyHeadOutput + "\\meshes\\actors\\character\\FaceGenData\\FaceGeom\\Skyrim.esm");
                CreateFolder(Program.settings.Value.HighPolyHeadOutput + "\\meshes\\actors\\character\\FaceGenData\\FaceGeom\\Dawnguard.esm");
                CreateFolder(Program.settings.Value.HighPolyHeadOutput + "\\meshes\\actors\\character\\FaceGenData\\FaceGeom\\HearthFires.esm");
                CreateFolder(Program.settings.Value.HighPolyHeadOutput + "\\meshes\\actors\\character\\FaceGenData\\FaceGeom\\Dragonborn.esm");
            }
            
            foreach (INpcGetter record in mod.Npcs)
            {

                IFormLinkNullableGetter<IArmorGetter> wornArmor = record.WornArmor;
                wornArmor.TryResolve(cache, out var context);

                string npcName = context!.EditorID!.Replace("BodyArmor", "");
                if (!handledNpc.Contains(npcName))
                {
                    ArmorAddon hair = new(state.PatchMod);

                    BodyTemplate bodyt = new()
                    {
                        ArmorType = ArmorType.Clothing,
                        FirstPersonFlags = BipedObjectFlag.Hair
                    };


                    hair.EditorID = npcName + "HairAA";
                    hair.BodyTemplate = bodyt;
                    hair.Race = defaultRace.ToNullableLink();

                    Model nif = new();
                    string FilePath = "actors\\character\\" + npcName + "\\hair\\" + npcName + "Hair_1.nif";
                    if (File.Exists(Constants.BasePath + FilePath))
                    {
                        nif.File = FilePath;
                        hair.WeightSliderEnabled = new GenderedItem<bool>(false, true);
                    }
                    else
                    {
                        FilePath = "actors\\character\\" + npcName + "\\hair\\" + npcName + "Hair.nif";
                        nif.File = FilePath;
                    }
                    hair.WorldModel = new GenderedItem<Model?>(null, nif);
                    hair.AdditionalRaces.AddRange(additionalRaces);
                    state.PatchMod.ArmorAddons.Add(hair);

                    Armor armor = state.PatchMod.Armors.GetOrAddAsOverride(context);
                    armor.BodyTemplate!.FirstPersonFlags |= BipedObjectFlag.Hair;
                    armor.Armature.Add(new FormLink<IArmorAddonGetter>(new FormKey(state.PatchMod.ModKey, hair.FormKey.ID)));

                    if (Program.settings.Value.UseHighPolyHead)
                    {
                        if (Program.settings.Value.UseYourSkin)
                        {
                            string fileName = record.FormKey.IDString().PadLeft(8, '0');
                            string modName = record.FormKey.ModKey.ToString();
                            string filePath = "";
                            if (modName.Equals("Skyrim.esm"))
                            {
                                filePath = "actors\\character\\FaceGenData\\FaceGeom\\Skyrim.esm\\" + fileName + ".nif";

                            }
                            else if (modName.Equals("Dawnguard.esm"))
                            {
                                filePath = "actors\\character\\FaceGenData\\FaceGeom\\Dawnguard.esm\\" + fileName + ".nif";
                            }
                            else if (modName.Equals("HearthFires.esm"))
                            {
                                filePath = "actors\\character\\FaceGenData\\FaceGeom\\HearthFires.esm\\" + fileName + ".nif";
                            }
                            else if (modName.Equals("Dragonborn.esm"))
                            {
                                filePath = "actors\\character\\FaceGenData\\FaceGeom\\Dragonborn.esm\\" + fileName + ".nif";
                            }

                            if (!File.Exists(Constants.BasePath + filePath))
                            {
                                Console.WriteLine($"Could not find {filePath}. Skipping.");
                                continue;
                            }
                            NifFile nifFile = new();
                            nifFile.Load(Constants.BasePath + filePath);
                            BSDynamicTriShape shape = nifFile.FindBlockByName<BSDynamicTriShape>(npcName + "HeadHP");
                            if (shape == null)
                            {
                                continue;
                            }
                            BSLightingShaderProperty shaderProperty = (BSLightingShaderProperty)nifFile.GetBlock(shape.ShaderPropertyRef);
                            BSShaderTextureSet textureSet = nifFile.GetBlock(shaderProperty.TextureSetRef);

                            textureSet.Textures[0].Content = "textures\\actors\\character\\female\\FemaleHead.dds";
                            textureSet.Textures[2].Content = "textures\\actors\\character\\female\\FemaleHead_sk.dds";
                            textureSet.Textures[7].Content = "textures\\actors\\character\\female\\FemaleHead_S.dds";
                            if (Program.settings.Value.UseYourSkinNormalMap)
                            {
                                textureSet.Textures[1].Content = "textures\\actors\\character\\female\\FemaleHead_msn.dds";
                            }
                            string path = Program.settings.Value.HighPolyHeadOutput.TrimEnd('\\') + "\\meshes\\" + filePath;
                            nifFile.Save(path, Constants.SaveOptions);
                        }
                    }


                    if (npcName.Equals("Jenassa"))
                    {
                        Npc Adrianne = state.PatchMod.Npcs.GetOrAddAsOverride(record);
                        Adrianne.HeadParts.Add(new FormLink<IHeadPartGetter>(new FormKey(mod.ModKey, 0x8FF)));
                    }
                    if (npcName.Equals("Adrianne"))
                    {
                        ColorRecord hairColor = new(state.PatchMod);
                        if (Program.settings.Value.AdrianneUseVanillaColor)
                        {
                            hairColor.Color = ColorTranslator.FromHtml("#5e5248");
                        }
                        else
                        {
                            hairColor.Color = ColorTranslator.FromHtml("#362424");
                        }
                        state.PatchMod.Colors.Add(hairColor);
                        Npc Adrianne = state.PatchMod.Npcs.GetOrAddAsOverride(record);
                        Adrianne.HairColor = hairColor.ToNullableLink();
                    }

                    handledNpc.Add(npcName);
                }
            }
        }

        private static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception) {
                    throw new Exception("High Poly Head Output: Must input incorrect folder location");
                }
            }
        }
    }
}