using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using static Kingmaker.RuleSystem.RulebookEvent;
using static Kingmaker.UnitLogic.Abilities.Components.AbilityCustomVitalStrike;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class HeritorMightyStrike : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            try
            {
                if (!evt.IsAttackOfOpportunity)
                {
                    var caster = Owner;
                    VitalStrikePart vitalStrikePart = caster.Ensure<VitalStrikePart>();
                    vitalStrikePart.Caster = caster;
                    vitalStrikePart.VitalStrikeMod = 3;
                    if (caster.HasFact(VitalFeat3))
                    {
                        vitalStrikePart.VitalStrikeMod = 4;
                    }
                    vitalStrikePart.MythicFact = caster.GetFact(VitalFeat4);
                    vitalStrikePart.Rowdy = caster.HasFact(VitalFeat5);
                    vitalStrikePart.Fact = caster.Facts.m_Facts.First();
                }

            }
            catch (Exception e)
            {
                Logger.Error("HeritorMightyStrike.OnEventAboutToTrigger", e);
            }
        }

        private static readonly BlueprintFeatureReference VitalFeat3 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeatureGreater.ToString());
        private static readonly BlueprintFeatureReference VitalFeat4 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeMythicFeat.ToString());
        private static readonly BlueprintFeatureReference VitalFeat5 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.RowdyVitalDamage.ToString());
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            try
            {
                var vitalStrikePart = Owner.Get<VitalStrikePart>();
                if (vitalStrikePart != null)
                {
                    vitalStrikePart.SetDeferredRules(evt);
                    vitalStrikePart.RemoveSelfIfDone();
                    Buff.Remove();
                }
            }
            catch (Exception e)
            {
                Logger.Error("HeritorMightyStrike.OnEventDidTrigger", e);
            }
        }
    }
}
