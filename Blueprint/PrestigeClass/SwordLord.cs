using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.Patch;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomComponent.Spell;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class SwordLord
    {
        private const string QuickDraw = "SwordLord.QuickDraw";
        private static readonly string QuickDrawGuid = "{40E0F0A9-A447-4891-B888-E75CC2E5CFDD}";

        internal const string QuickDrawDisplayName = "SwordLordQuickDraw.Name";
        private const string QuickDrawDescription = "SwordLordQuickDraw.Description";
        //"QuickDraw": "b7b65e54-2278-4145-8590-31fa48b785e9",
        public static void CreateQuickDraw()
        {
            var icon = FeatureRefs.Alertness.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(QuickDraw, QuickDrawGuid)
              .SetDisplayName(QuickDrawDisplayName)
              .SetDescription(QuickDrawDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact("b7b65e54-2278-4145-8590-31fa48b785e9", FeatureRefs.DuelingMastery.ToString(), false)
              .AddFeatureIfHasFact(FeatureRefs.Alertness.ToString(), FeatureRefs.DuelingMastery.ToString(), false)
              .SetReapplyOnLevelUp()
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.SwordlordProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }

        private const string HellDomain = "SwordLord.HellDomain";
        private static readonly string HellDomainGuid = "{09698710-8D01-47A0-B86A-6A8265E13BBC}";

        private const string HellDomainfeat = "SwordLord.HellDomainfeat";
        public static readonly string HellDomainfeatGuid = "{C014978C-E88C-4302-801E-5B1C9B5ECA6C}";

        public static BlueprintProgression HellDomainFeat()
        {
            var icon = FeatureRefs.WarriorPriest.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(HellDomainfeat, HellDomainfeatGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIsClassFeature(true)
              .SetRanks(20)
              .SetHideInUI(true)
              .Configure();

            return ProgressionConfigurator.New(HellDomain, HellDomainGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteFeature(FeatureRefs.WarriorPriest.ToString())
              .AddToClasses(CharacterClassRefs.HellknightSigniferClass.ToString())
              .AddToLevelEntry(1, feat1, FeatureRefs.SelectiveChannel.ToString())
              .AddToLevelEntry(2, feat1)
                .AddToLevelEntry(3, feat1)
                .AddToLevelEntry(4, feat1)
                .AddToLevelEntry(5, feat1)
                .AddToLevelEntry(6, feat1)
                .AddToLevelEntry(7, feat1)
                .AddToLevelEntry(8, feat1)
                .AddToLevelEntry(9, feat1)
                .AddToLevelEntry(10, feat1)
              .Configure();
        }

        private const string Catechesis = "SwordLord.Catechesis";
        private static readonly string CatechesisGuid = "{A23D1F1C-2EAB-4BEC-BA7C-4B6BE9BABCD2}";

        internal const string CatechesisDisplayName = "SwordLordCatechesis.Name";
        private const string CatechesisDescription = "SwordLordCatechesis.Description";
        public static void CreateCatechesis()
        {
            var icon = FeatureRefs.WarriorPriest.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(Catechesis, CatechesisGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIcon(icon)
              .AddToAllFeatures(HellDomainFeat())
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.HellknightSigniferProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }

        private const string VariantBloodline = "SwordLord.VariantBloodline";
        public static readonly string VariantBloodlineGuid = "{D8F534B2-F81C-45A5-9CEE-2D4CA8B30980}";

        internal const string EsotericDragons2DisplayName = "SwordLordEsotericDragons2.Name";
        private const string EsotericDragons2Description = "SwordLordEsotericDragons2.Description";
        public static BlueprintProgression VariantBloodlineFeat()
        {
            var icon = AbilityRefs.AzaraDragonBreathWeapon.Reference.Get().Icon;

            return ProgressionConfigurator.New(VariantBloodline, VariantBloodlineGuid)
              .SetDisplayName(EsotericDragons2DisplayName)
              .SetDescription(EsotericDragons2Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToClasses(CharacterClassRefs.DragonDiscipleClass.ToString())
                .AddToLevelEntry(1, CreateVariantBloodlineArcana3())
                .AddToLevelEntry(3, CreateVariantBloodlineArcana6())
                .AddToLevelEntry(5, CreateVariantBloodlineArcana9())
                .AddToLevelEntry(7, CreateVariantBloodlineArcana12())
                .AddToLevelEntry(9, CreateVariantBloodlineArcana15())
                .AddToLevelEntry(10, CreateVariantBloodlineArcana18())
              .Configure();
        }

        private const string EsotericDragons = "SwordLord.EsotericDragons";
        private static readonly string EsotericDragonsGuid = "{2D1E7F39-EAF1-4099-ADB7-9D4144A34BEB}";

        internal const string EsotericDragonsDisplayName = "SwordLordEsotericDragons.Name";
        private const string EsotericDragonsDescription = "SwordLordEsotericDragons.Description";
        public static void CreateEsotericDragons()
        {
            var icon = AbilityRefs.AzaraDragonBreathWeapon.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(EsotericDragons, EsotericDragonsGuid)
              .SetDisplayName(EsotericDragonsDisplayName)
              .SetDescription(EsotericDragonsDescription)
              .SetIcon(icon)
              .AddToAllFeatures(VariantBloodlineFeat())
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DragonDiscipleProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }

        private const string VariantBloodlineArcana3 = "DragonDisciple.VariantBloodlineArcana3";
        public static readonly string VariantBloodlineArcana3Guid = "{F4D45CBA-CE7D-457E-BF33-9F4A2F84DDBD}";

        internal const string VariantBloodlineArcana3DisplayName = "DragonDiscipleVariantBloodlineArcana3.Name";
        private const string VariantBloodlineArcana3Description = "DragonDiscipleVariantBloodlineArcana3.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana3()
        {
            var icon = AbilityRefs.BestowGraceCast.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana3, VariantBloodlineArcana3Guid)
              .SetDisplayName(VariantBloodlineArcana3DisplayName)
              .SetDescription(VariantBloodlineArcana3Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(HermeanPotential.HermeanPotentialAbilityGuid);
                  c.level = 1;
              })
              .Configure();
        }

        private const string VariantBloodlineArcana6 = "DragonDisciple.VariantBloodlineArcana6";
        public static readonly string VariantBloodlineArcana6Guid = "{6A4DF308-96E6-48CC-8E4C-7289A4C6762C}";

        internal const string VariantBloodlineArcana6DisplayName = "DragonDiscipleVariantBloodlineArcana6.Name";
        private const string VariantBloodlineArcana6Description = "DragonDiscipleVariantBloodlineArcana6.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana6()
        {
            var icon = AbilityRefs.CacophonousCall.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana6, VariantBloodlineArcana6Guid)
              .SetDisplayName(VariantBloodlineArcana6DisplayName)
              .SetDescription(VariantBloodlineArcana6Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPain.DebilitatingPainAbilityGuid);
                  c.level = 3;
              })
              .Configure();
        }

        private const string VariantBloodlineArcana9 = "DragonDisciple.VariantBloodlineArcana9";
        public static readonly string VariantBloodlineArcana9Guid = "{442B4834-678A-4F3E-AF9A-0A05FC61FED3}";

        internal const string VariantBloodlineArcana9DisplayName = "DragonDiscipleVariantBloodlineArcana9.Name";
        private const string VariantBloodlineArcana9Description = "DragonDiscipleVariantBloodlineArcana9.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana9()
        {
            var icon = AbilityRefs.CacophonousCallMass.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana9, VariantBloodlineArcana9Guid)
              .SetDisplayName(VariantBloodlineArcana9DisplayName)
              .SetDescription(VariantBloodlineArcana9Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPainMass.DebilitatingPainMassAbilityGuid);
                  c.level = 5;
              })
              .Configure();
        }

        private const string VariantBloodlineArcana12 = "DragonDisciple.VariantBloodlineArcana12";
        public static readonly string VariantBloodlineArcana12Guid = "{43036927-40FD-4B5E-8E13-DDB8880D191D}";

        internal const string VariantBloodlineArcana12DisplayName = "DragonDiscipleVariantBloodlineArcana12.Name";
        private const string VariantBloodlineArcana12Description = "DragonDiscipleVariantBloodlineArcana12.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana12()
        {
            var icon = AbilityRefs.ConstrictingCoils.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana12, VariantBloodlineArcana12Guid)
              .SetDisplayName(VariantBloodlineArcana12DisplayName)
              .SetDescription(VariantBloodlineArcana12Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(PhantomLimb.PhantomLimbAbilityGuid);
                  c.level = 6;
              })
              .Configure();
        }

        private const string VariantBloodlineArcana15 = "DragonDisciple.VariantBloodlineArcana15";
        public static readonly string VariantBloodlineArcana15Guid = "{476372AD-3343-4718-B1D1-F19853077AF4}";

        internal const string VariantBloodlineArcana15DisplayName = "DragonDiscipleVariantBloodlineArcana15.Name";
        private const string VariantBloodlineArcana15Description = "DragonDiscipleVariantBloodlineArcana15.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana15()
        {
            var icon = AbilityRefs.TrueSeeingCommunal.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana15, VariantBloodlineArcana15Guid)
              .SetDisplayName(VariantBloodlineArcana15DisplayName)
              .SetDescription(VariantBloodlineArcana15Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(GlimpseAkashic.GlimpseAkashicAbilityGuid);
                  c.level = 8;
              })
              .Configure();
        }

        private const string VariantBloodlineArcana18 = "DragonDisciple.VariantBloodlineArcana18";
        public static readonly string VariantBloodlineArcana18Guid = "{9179337D-A6B9-4523-BB01-7692FDDAF11D}";

        internal const string VariantBloodlineArcana18DisplayName = "DragonDiscipleVariantBloodlineArcana18.Name";
        private const string VariantBloodlineArcana18Description = "DragonDiscipleVariantBloodlineArcana18.Description";

        public static BlueprintFeature CreateVariantBloodlineArcana18()
        {
            var icon = AbilityRefs.BrilliantInspiration.Reference.Get().Icon;

            return FeatureConfigurator.New(VariantBloodlineArcana18, VariantBloodlineArcana18Guid)
              .SetDisplayName(VariantBloodlineArcana18DisplayName)
              .SetDescription(VariantBloodlineArcana18Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AkashicForm.AkashicFormAbilityGuid);
                  c.level = 9;
              })
              .Configure();
        }
    }
}
