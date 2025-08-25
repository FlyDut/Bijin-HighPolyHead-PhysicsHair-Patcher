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
using static BijinAIOPathcer.Constants;
namespace BijinAIOPathcer.Patchers
{
    public static class NpcPatcher
    {
        public static void Apply(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ISkyrimModGetter mod)
        {
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
                if (!wornArmor.TryResolve(cache, out var context)) {
                    continue;
                }

                string npcName = context.EditorID!.Replace("BodyArmor", "");
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
                    hair.Race = Constants.defaultRace;

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
                    hair.AdditionalRaces.AddRange(Constants.additionalRaces);
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
                        hairColor.EditorID = "AdrianneHairColor";
                        if (Program.settings.Value.Adrianne.HairColor == AdrianneHairColor.VanillaBased)
                        {
                            hairColor.Color = ColorTranslator.FromHtml("#2f2a24");
                        }
                        else
                        {
                            hairColor.Color = ColorTranslator.FromHtml("#181212");
                        }
                        state.PatchMod.Colors.Add(hairColor);
                        Npc Adrianne = state.PatchMod.Npcs.GetOrAddAsOverride(record);
                        Adrianne.HairColor = hairColor.ToNullableLink();

                    }

                    handledNpc.Add(npcName);
                }
            }
        }
        public static void ApplyValerica(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (cache.TryResolve<INpcGetter>("DLC1Valerica", out var record))
            {
                IFormLinkNullableGetter<IArmorGetter> wornArmor = record.WornArmor;
                IArmorGetter context = wornArmor.Resolve(cache);

                String npcName = "Valerica";
                ArmorAddon hair = new(state.PatchMod);

                BodyTemplate bodyt = new()
                {
                    ArmorType = ArmorType.Clothing,
                    FirstPersonFlags = BipedObjectFlag.Hair
                };


                hair.EditorID = npcName + "HairAA";
                hair.BodyTemplate = bodyt;
                hair.Race = Constants.defaultRace;

                Model nif = new();
                string FilePath = "actors\\character\\Valerica\\hair\\ValericaHair_1.nif";
                nif.File = FilePath;
                hair.WeightSliderEnabled = new GenderedItem<bool>(false, true);

                hair.WorldModel = new GenderedItem<Model?>(null, nif);
                hair.AdditionalRaces.AddRange(Constants.additionalRaces);
                state.PatchMod.ArmorAddons.Add(hair);

                Armor armor = state.PatchMod.Armors.GetOrAddAsOverride(context);
                armor.BodyTemplate!.FirstPersonFlags |= BipedObjectFlag.Hair;
                armor.Armature.Add(new FormLink<IArmorAddonGetter>(new FormKey(state.PatchMod.ModKey, hair.FormKey.ID)));

                ColorRecord hairColor = new(state.PatchMod);
                hairColor.EditorID = "ValericaHairColor";
                if (Program.settings.Value.Valerica.HairColor == ValericaHairColor.Gery)
                {
                    hairColor.Color = ColorTranslator.FromHtml("#1e1e1e");
                }
                else if (Program.settings.Value.Valerica.HairColor == ValericaHairColor.WineRed)
                {
                   
                    hairColor.Color = ColorTranslator.FromHtml("#180b0e");
                    
                }
                else
                {
                    hairColor.Color = ColorTranslator.FromHtml("#101010");
                }
                state.PatchMod.Colors.Add(hairColor);
                Npc Adrianne = state.PatchMod.Npcs.GetOrAddAsOverride(record);
                Adrianne.HairColor = hairColor.ToNullableLink();


                if (Program.settings.Value.UseHighPolyHead)
                {
                    if (Program.settings.Value.UseYourSkin)
                    {
                        string fileName = record.FormKey.IDString().PadLeft(8, '0');
                        string modName = record.FormKey.ModKey.ToString();
                        string filePath = "actors\\character\\FaceGenData\\FaceGeom\\Dawnguard.esm\\" + fileName + ".nif";

                        if (!File.Exists(Constants.BasePath + filePath))
                        {
                            Console.WriteLine($"Could not find {filePath}. Skipping.");
                            return;
                        }
                        NifFile nifFile = new();
                        nifFile.Load(Constants.BasePath + filePath);
                        BSDynamicTriShape shape = nifFile.FindBlockByName<BSDynamicTriShape>(npcName + "HeadHP");
                        if (shape == null)
                        {
                            return;
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
            }
        }

        public static void ApplySerana(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, ImmutableModLinkCache<ISkyrimMod, ISkyrimModGetter> cache)
        {
            if (cache.TryResolve<INpcGetter>("DLC1Serana", out var record))
            {
                IFormLinkNullableGetter<IArmorGetter> wornArmor = record.WornArmor;
                IArmorGetter context = wornArmor.Resolve(cache);

                String npcName = "Serana";
                ArmorAddon hair = new(state.PatchMod);

                BodyTemplate bodyt = new()
                {
                    ArmorType = ArmorType.Clothing,
                    FirstPersonFlags = BipedObjectFlag.Hair
                };


                hair.EditorID = npcName + "HairAA";
                hair.BodyTemplate = bodyt;
                hair.Race = Constants.defaultRace;

                Model nif = new();
                string FilePath = "actors\\character\\Serana\\hair\\SeranaHair_1.nif";
                nif.File = FilePath;
                hair.WeightSliderEnabled = new GenderedItem<bool>(false, true);

                hair.WorldModel = new GenderedItem<Model?>(null, nif);
                hair.AdditionalRaces.AddRange(Constants.additionalRaces);
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
                        string filePath = "actors\\character\\FaceGenData\\FaceGeom\\Dawnguard.esm\\" + fileName + ".nif";

                        if (!File.Exists(Constants.BasePath + filePath))
                        {
                            Console.WriteLine($"Could not find {filePath}. Skipping.");
                            return;
                        }
                        NifFile nifFile = new();
                        nifFile.Load(Constants.BasePath + filePath);
                        BSDynamicTriShape shape = nifFile.FindBlockByName<BSDynamicTriShape>(npcName + "HeadHP");
                        if (shape == null)
                        {
                            return;
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