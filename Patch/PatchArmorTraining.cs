using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchArmorTraining
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            try
            {
                var ArmorTrainingSelection = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>("354f1a44-26d2-4ea3-8718-905108f48e72")?.Get();
                if (ArmorTrainingSelection == null) { Logger.Info("not found ttt at"); return; }
                var FighterClass = CharacterClassRefs.FighterClass.Reference.Get();
                var ArmorTraining = FeatureRefs.ArmorTraining.Reference.Get();

                foreach (var Archetype in FighterClass.Archetypes)
                {
                    Archetype.RemoveFeatures
                        .Where(entry => entry.Level > 3)
                        .Where(entry => entry.m_Features.Contains(ArmorTraining.ToReference<BlueprintFeatureBaseReference>()))
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(ArmorTrainingSelection.ToReference<BlueprintFeatureBaseReference>());
                            entry.m_Features.Remove(ArmorTraining.ToReference<BlueprintFeatureBaseReference>());
                        });
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit armor training.", e); }
            try
            {
                var ExpertTrainer = BlueprintTool.GetRef<BlueprintFeatureReference>("ae97a4eb-750d-499c-8379-88f62a24e0de")?.Get();
                if (ExpertTrainer == null) { Logger.Info("not found ttt et"); return; }
                BlueprintTool.GetRef<BlueprintArchetypeReference>(Constable.ArchetypeGuid).Get().RemoveFeatures[2].m_Features = new() { ExpertTrainer.ToReference<BlueprintFeatureBaseReference>() };
            }
            catch (Exception e) { Logger.Error("Failed to edit ExpertTrainer.", e); }
            
        }
    }
}
