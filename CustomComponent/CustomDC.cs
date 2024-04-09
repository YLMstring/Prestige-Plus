using BlueprintCore.Utils;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Properties;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.JsonSystem;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.Modify
{
    [TypeId("{1826B09B-766F-46C0-B964-9A231BAD1E81}")]
    internal class CustomDC : ContextCalculateAbilityParams
    {
        public override AbilityParams Calculate(MechanicsContext context)
        {
            UnitEntityData maybeCaster = context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return context.Params;
            }
            BlueprintScriptableObject associatedBlueprint = context.AssociatedBlueprint;
            UnitEntityData caster = maybeCaster;
            AbilityExecutionContext sourceAbilityContext = context.SourceAbilityContext;
            return this.MyCalculate(context, associatedBlueprint, caster, (sourceAbilityContext != null) ? sourceAbilityContext.Ability : null);
        }

        private AbilityParams MyCalculate([CanBeNull] MechanicsContext context, [NotNull] BlueprintScriptableObject blueprint, [NotNull] UnitEntityData caster, [CanBeNull] AbilityData ability)
        {
            StatType value = Property;
            RuleCalculateAbilityParams ruleCalculateAbilityParams = (ability != null) ? new RuleCalculateAbilityParams(caster, ability) : new RuleCalculateAbilityParams(caster, blueprint, null);
            ruleCalculateAbilityParams.ReplaceStat = new StatType?(value);
            if (Property2 != StatType.Unknown)
            {
                if (caster.Stats.GetStat<ModifiableValueAttributeStat>(Property2) > caster.Stats.GetStat<ModifiableValueAttributeStat>(Property))
                {
                    ruleCalculateAbilityParams.ReplaceStat = new StatType?(Property2);
                }
            }
            if (this.StatTypeFromCustomProperty)
            {
                ruleCalculateAbilityParams.ReplaceStatBonusModifier = new int?(this.m_CustomProperty.Get().GetInt(caster));
            }
            int level = caster.Descriptor.Progression.CharacterLevel;
            ruleCalculateAbilityParams.ReplaceCasterLevel = level;
            if (!characterlv)
            {
                var archetype = BlueprintTool.GetRef<BlueprintCharacterClassReference>(classguid);
                level = caster.Descriptor.Progression.GetClassLevel(archetype);
                ruleCalculateAbilityParams.ReplaceCasterLevel = level;
            }
            if (isRogue)
            {
                level += caster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.SlayerClass.ToString()));
                level += caster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(ShadowDancer.ArchetypeGuid));
            }
            ruleCalculateAbilityParams.ReplaceSpellLevel = new int?(level);
            if (halfed)
            {
                ruleCalculateAbilityParams.ReplaceSpellLevel /= 2;
            }
            if (context != null)
            {
                return context.TriggerRule(ruleCalculateAbilityParams).Result;
            }
            return Rulebook.Trigger(ruleCalculateAbilityParams).Result;
        }

        public string classguid;
        public StatType Property;

        public bool halfed = false;
        public StatType Property2 = StatType.Unknown;
        public bool isRogue = false;

        public bool characterlv = false;
    }
}
