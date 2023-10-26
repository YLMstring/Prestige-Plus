using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.PrestigeClass;
using Kingmaker.UnitLogic;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ContextCalculateAbilityParamsBasedOnClass))]
    [HarmonyPatch("Calculate")]
    [HarmonyPatch(new Type[] { typeof(MechanicsContext), typeof(BlueprintScriptableObject), typeof(UnitEntityData), typeof(AbilityData) })]
    internal class EsoKnightDC
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
        static bool Prefix(ref AbilityParams __result, ref ContextCalculateAbilityParamsBasedOnClass __instance, ref MechanicsContext context, ref BlueprintScriptableObject blueprint, ref UnitEntityData caster, ref AbilityData ability)
        {
            try
            {
                Logger.Info(context.AssociatedBlueprint.NameSafe() + "start");
                if (__instance.CharacterClass == CharacterClassRefs.KineticistClass.Reference.Get() && caster.HasFact(feat))
                {
                    Logger.Info(caster.CharacterName + "start");
                    int klevel = caster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.KineticistClass.ToString()));
                    int elevel = caster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(EsotericKnight.ArchetypeGuid)) / 2;
                    int blast = (klevel + elevel) / 2;
                    int origin = klevel / 2;
                    int update = blast - origin;
                    StatType value = __instance.StatType;
                    if (__instance.UseKineticistMainStat)
                    {
                        UnitPartKineticist unitPartKineticist = (caster != null) ? caster.Get<UnitPartKineticist>() : null;
                        if (unitPartKineticist == null)
                        {
                            PFLog.Default.Error(blueprint, string.Format("Caster is not kineticist: {0} ({1})", caster, blueprint.NameSafe()), Array.Empty<object>());
                        }
                        value = ((unitPartKineticist != null) ? unitPartKineticist.MainStatType : __instance.StatType);
                    }
                    RuleCalculateAbilityParams ruleCalculateAbilityParams = (ability != null) ? new RuleCalculateAbilityParams(caster, ability) : new RuleCalculateAbilityParams(caster, blueprint, null);
                    ruleCalculateAbilityParams.ReplaceStat = new StatType?(value);
                    ruleCalculateAbilityParams.ReplaceCasterLevel = new int?(klevel + elevel);
                    ruleCalculateAbilityParams.ReplaceSpellLevel = new int?(blast);
                    if (context != null)
                    {
                        __result = context.TriggerRule(ruleCalculateAbilityParams).Result;
                    }
                    __result = Rulebook.Trigger(ruleCalculateAbilityParams).Result;
                    return false;
                }
                return true;
            }
            catch (Exception e) { Logger.Error("fail eso", e); return true; }
        }
    }
}
