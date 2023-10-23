using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Reflection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Blueprints.Configurators.Classes;

namespace PrestigePlus.Modify
{
    internal class SpellbookLevelUp
    {
        

        private const string spellupgrade = "Spellupgrade";
        private static readonly string spellupgradeGuid = "{E995A687-EC47-4A29-9AFE-1C2715E0CC24}";

        internal const string SpellupgradeDisplayName = "Spellupgrade.Name";
        private const string SpellupgradeDescription = "Spellupgrade.Description";

        public static void Select()
        {
        FeatureSelectionConfigurator.New(spellupgrade, spellupgradeGuid)
              .SetDisplayName(SpellupgradeDisplayName)
              .SetDescription(SpellupgradeDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .SetAllFeatures(new Blueprint<BlueprintFeatureReference>[] { SpellbookLevelUp.AngelfireApostle(), SpellbookLevelUp.Cleric(), SpellbookLevelUp.Crusader(), SpellbookLevelUp.Paladin(), SpellbookLevelUp.Warpriest()})
              .Configure();
        }

        public static readonly List<BlueprintReference<BlueprintAbility>> ChannelPositiveHeal =
        new()
        {
          AbilityRefs.ChannelEnergy.Reference,
          AbilityRefs.ChannelEnergyHospitalerHeal.Reference,
          AbilityRefs.ChannelEnergyEmpyrealHeal.Reference,
          AbilityRefs.ChannelEnergyPaladinHeal.Reference,
          AbilityRefs.ShamanLifeSpiritChannelEnergy.Reference,
          AbilityRefs.OracleRevelationChannelAbility.Reference,
          AbilityRefs.WarpriestChannelEnergy.Reference,
          AbilityRefs.HexChannelerChannelEnergy.Reference,
          
        };

        public static readonly List<Blueprint<BlueprintAbilityReference>> CureOrInflict =
        new()
        {
          AbilityRefs.CureLightWoundsCast.Reference.ToString(),
          AbilityRefs.CureModerateWoundsCast.Reference.ToString(),
          AbilityRefs.CureSeriousWoundsCast.Reference.ToString(),
          AbilityRefs.CureCriticalWoundsCast.Reference.ToString(),
          AbilityRefs.CureLightWoundsMass.Reference.ToString(),
          AbilityRefs.CureModerateWoundsMass.Reference.ToString(),
          AbilityRefs.CureSeriousWoundsMass.Reference.ToString(),
          AbilityRefs.CureCriticalWoundsMass.Reference.ToString(),
          AbilityRefs.InflictLightWoundsCast.Reference.ToString(),
          AbilityRefs.InflictModerateWoundsCast.Reference.ToString(),
          AbilityRefs.InflictSeriousWoundsCast.Reference.ToString(),
          AbilityRefs.InflictCriticalWoundsCast.Reference.ToString(),
          AbilityRefs.InflictLightWoundsMass.Reference.ToString(),
          AbilityRefs.InflictModerateWoundsMass.Reference.ToString(),
          AbilityRefs.InflictSeriousWoundsMass.Reference.ToString(),
          AbilityRefs.InflictCriticalWoundsMass.Reference.ToString(),
          AbilityRefs.CureLightWounds.Reference.ToString(),
          AbilityRefs.CureModerateWounds.Reference.ToString(),
          AbilityRefs.CureSeriousWounds.Reference.ToString(),
          AbilityRefs.CureCriticalWounds.Reference.ToString(),
          AbilityRefs.InflictLightWounds.Reference.ToString(),
          AbilityRefs.InflictModerateWounds.Reference.ToString(),
          AbilityRefs.InflictSeriousWounds.Reference.ToString(),
          AbilityRefs.InflictCriticalWounds.Reference.ToString(),
        };


        public static readonly string AngelfireApostleName = "AngelfireApostle";
        public static readonly string AngelfireApostleGuid = "8ed3202d-a978-4961-83cc-1b0776856021";

        public static readonly string AngelfireApostleDisplayName = "AngelfireApostle.Name";
        public static readonly string AngelfireApostleDescription = "AngelfireApostle.Description";

        public static BlueprintFeature AngelfireApostle()
        {
            return FeatureConfigurator.New(AngelfireApostleName, AngelfireApostleGuid)
                .SetDisplayName(AngelfireApostleDisplayName)
                .SetDescription(AngelfireApostleDescription)
                .AddSpellbookLevel(SpellbookRefs.AngelfireApostleSpellbook.ToString())
                .SetRanks(10).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.AngelfireApostleSpellbook.ToString(); })
                .Configure();
        }

        

        public static readonly string ClericName = "Cleric";
        public static readonly string ClericGuid = "c165fefd-08a3-4e39-9eaf-48848ec56f70";

        public static readonly string ClericDisplayName = "Cleric.Name";
        public static readonly string ClericDescription = "Cleric.Description";

        public static BlueprintFeature Cleric()
        {
            return FeatureConfigurator.New(ClericName, ClericGuid)
                .SetDisplayName(ClericDisplayName)
                .SetDescription(ClericDescription)
                .AddSpellbookLevel(SpellbookRefs.ClericSpellbook.ToString())
                .SetRanks(10).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ClericSpellbook.ToString(); })
                .Configure();
        }

        

        public static readonly string CrusaderName = "Crusader";
        public static readonly string CrusaderGuid = "e94e1663-1335-4e3b-883e-1d1bf2e904f2";

        public static readonly string CrusaderDisplayName = "Crusader.Name";
        public static readonly string CrusaderDescription = "Crusader.Description";

        public static BlueprintFeature Crusader()
        {
            return FeatureConfigurator.New(CrusaderName, CrusaderGuid)
                .SetDisplayName(CrusaderDisplayName)
                .SetDescription(CrusaderDescription)
                .AddSpellbookLevel(SpellbookRefs.CrusaderSpellbook.ToString())
                .SetRanks(10).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.CrusaderSpellbook.ToString(); })
                .Configure();
        }

        

        public static readonly string PaladinName = "Paladin";
        public static readonly string PaladinGuid = "c0a69873-97ab-494f-aa49-dad3945d45f1";

        public static readonly string PaladinDisplayName = "Paladin.Name";
        public static readonly string PaladinDescription = "Paladin.Description";

        public static BlueprintFeature Paladin()
        {
            return FeatureConfigurator.New(PaladinName, PaladinGuid)
                .SetDisplayName(PaladinDisplayName)
                .SetDescription(PaladinDescription)
                .AddSpellbookLevel(SpellbookRefs.PaladinSpellbook.ToString())
                .SetRanks(10).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.PaladinSpellbook.ToString(); })
                .Configure();
        }


        public static readonly string WarpriestName = "Warpriest";
        public static readonly string WarpriestGuid = "52c1f2dc-a31c-457a-a9da-3aa6346117f0";

        public static readonly string WarpriestDisplayName = "Warpriest.Name";
        public static readonly string WarpriestDescription = "Warpriest.Description";

        public static BlueprintFeature Warpriest()
        {
            return FeatureConfigurator.New(WarpriestName, WarpriestGuid)
                .SetDisplayName(WarpriestDisplayName)
                .SetDescription(WarpriestDescription)
                .AddSpellbookLevel(SpellbookRefs.WarpriestSpellbook.ToString())
                .SetRanks(10).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.WarpriestSpellbook.ToString(); })
                .Configure();
        }

        
    }

}

