using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Prerequisites;

namespace PrestigePlus.Blueprint.Feat
{
    internal class MageHandTrick
    {
        private static readonly string MageHandMainFeatName = "MageHandMageHandMain";
        private static readonly string MageHandMainFeatGuid = "{0FF806EB-BAF4-4B36-A773-2204A9582E33}";

        private static readonly string MageHandMainDisplayName = "MageHandMageHandMain.Name";
        private static readonly string MageHandMainDescription = "MageHandMageHandMain.Description";

        public static void ConfigureMageHandMain()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            FeatureConfigurator.New(MageHandMainFeatName, MageHandMainFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(MageHandMainDisplayName)
                    .SetDescription(MageHandMainDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteSpellKnown>(c => { c.m_Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.Jolt.ToString()); c.RequireSpellbook = true; })
                    .AddFeatureIfHasFact(FeatureRefs.ImprovedDirtyTrick.ToString(), ConfigureDirtyMagicTrick(), false)
                    .AddFeatureOnSkill(new() { ConfigureRangedAid() }, 1, StatType.BaseAttackBonus)
                    .AddFeatureIfHasFact(FeatureRefs.ImprovedUnarmedStrike.ToString(), ConfigureThrowPunch(), false)
                    .Configure();
        }

        private static readonly string DirtyMagicTrickFeatName = "MageHandDirtyMagicTrick";
        private static readonly string DirtyMagicTrickFeatGuid = "{F9E7A9DC-96A4-4D4E-BB71-E68809BDB9D0}";

        private static readonly string DirtyMagicTrickDisplayName = "MageHandDirtyMagicTrick.Name";
        private static readonly string DirtyMagicTrickDescription = "MageHandDirtyMagicTrick.Description";

        public static BlueprintFeature ConfigureDirtyMagicTrick()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            return FeatureConfigurator.New(DirtyMagicTrickFeatName, DirtyMagicTrickFeatGuid)
                    .SetDisplayName(DirtyMagicTrickDisplayName)
                    .SetDescription(DirtyMagicTrickDescription)
                    .SetIcon(icon)
                    .AddAutoMetamagic(new() { AbilityRefs.DirtyTrickBlindnessAction.ToString(), AbilityRefs.DirtyTrickEntangleAction.ToString(), AbilityRefs.DirtyTrickSickenedAction.ToString() },
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
                    .Configure();
        }

        private static readonly string RangedAidFeatName = "MageHandRangedAid";
        private static readonly string RangedAidFeatGuid = "{1D567F1A-8883-4279-B928-22F3650EEDFF}";

        private static readonly string RangedAidDisplayName = "MageHandRangedAid.Name";
        private static readonly string RangedAidDescription = "MageHandRangedAid.Description";

        public static BlueprintFeature ConfigureRangedAid()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;
            var AidAnotherAbility = "1eea0d80-c6b7-407a-a8a9-02f6e750ea95";
            var AidAnotherDefenceAbility = "4c2985ad-5b00-4fed-b94b-3467b80b793a";
            var AidAnotherOffenceAbility = "0b051b7c-166c-4b90-93ed-dbc42af2bb45";

            var SwiftAidAnotherAbility = "840e5398-3d98-4348-9554-f76cc764b776";
            var SwiftAidAnotherDefenceAbility = "c341e4ae-f26c-4189-af2a-1d72c994c1e3";
            var SwiftAidAnotherOffenceAbility = "eda56623-9a34-4d69-bc78-7a4fcaabc092";

            return FeatureConfigurator.New(RangedAidFeatName, RangedAidFeatGuid)
                    .SetDisplayName(RangedAidDisplayName)
                    .SetDescription(RangedAidDescription)
                    .SetIcon(icon)
                    .AddAutoMetamagic(new() { AidAnotherAbility, AidAnotherDefenceAbility, AidAnotherOffenceAbility, SwiftAidAnotherAbility, SwiftAidAnotherDefenceAbility, SwiftAidAnotherOffenceAbility },
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
                    .Configure();
        }

        private static readonly string ThrowPunchFeatName = "MageHandThrowPunch";
        private static readonly string ThrowPunchFeatGuid = "{01B800EC-BBFF-49CF-841C-0657DF53F356}";

        private static readonly string ThrowPunchDisplayName = "MageHandThrowPunch.Name";
        private static readonly string ThrowPunchDescription = "MageHandThrowPunch.Description";

        private const string ThrowPunchbuff = "MageHandTrick.ThrowPunchbuff";
        public static readonly string ThrowPunchbuffGuid = "{A6997B23-0803-42F1-84F6-4506793A888E}";

        private const string ThrowPunchActivatableAbility = "MageHandTrick.ThrowPunchActivatableAbility";
        private static readonly string ThrowPunchActivatableAbilityGuid = "{4E0D0E93-F74C-4CB3-B6D7-FF0B705B3E2C}";

        public static BlueprintFeature ConfigureThrowPunch()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(ThrowPunchbuff, ThrowPunchbuffGuid)
              .SetDisplayName(ThrowPunchDisplayName)
              .SetDescription(ThrowPunchDescription)
              .SetIcon(icon)
              .AddWeaponDamageOverride(formula: new DiceFormula(1, DiceType.D3), overrideDice: true, weaponTypes: new() { WeaponTypeRefs.Unarmed.Cast<BlueprintWeaponTypeReference>() })
              .AddComponent<ThrowPunchStuff>()
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(ThrowPunchActivatableAbility, ThrowPunchActivatableAbilityGuid)
                .SetDisplayName(ThrowPunchDisplayName)
                .SetDescription(ThrowPunchDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            return FeatureConfigurator.New(ThrowPunchFeatName, ThrowPunchFeatGuid)
                    .SetDisplayName(ThrowPunchDisplayName)
                    .SetDescription(ThrowPunchDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
