using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.CustomAction.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class CloakDaggerTac : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (CloakDaggerManeuver.IsChosenWeapon(Owner) && Owner.Get<AbilityCustomVitalStrike.VitalStrikePart>() != null)
            {
                var maneuver = CombatManeuver.None;
                var caster = Owner;
                if (caster.HasFact(DirtyBlind))
                {
                    maneuver = CombatManeuver.DirtyTrickBlind;
                }
                else if (caster.HasFact(DirtyEntangle))
                {
                    maneuver = CombatManeuver.DirtyTrickEntangle;
                }
                else if (caster.HasFact(DirtySicken))
                {
                    maneuver = CombatManeuver.DirtyTrickSickened;
                }
                if (SweepManeuver.ActManeuver(caster, evt.Target, 0, maneuver))
                {
                    int dc = 10 + Owner.Stats.BaseAttackBonus / 2 + Owner.Stats.Intelligence.Bonus;
                    bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, dc)
                    {
                        IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
                    }, evt.Target.Context, true).Success;
                    if (!pass)
                    {
                        if (GiveNextBuff(evt.Target, BuffRefs.Confusion.Reference.Get()))
                        {
                            if (GiveNextBuff(evt.Target, BuffRefs.Staggered.Reference.Get()))
                            {
                                if (GiveNextBuff(evt.Target, BuffRefs.Exhausted.Reference.Get()))
                                {
                                    GiveNextBuff(evt.Target, BuffRefs.CantMove.Reference.Get());
                                }
                            }
                        }
                    }
                }
                Buff.Remove();
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.AttackRoll.IsHit && CloakDaggerManeuver.IsChosenWeapon(Owner) && (Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted || evt.Target.CombatState.IsFlanked))
            {
                var maneuver = CombatManeuver.None;
                var caster = Owner;
                if (caster.HasFact(DirtyBlind))
                {
                    maneuver = CombatManeuver.DirtyTrickBlind;
                }
                else if (caster.HasFact(DirtyEntangle))
                {
                    maneuver = CombatManeuver.DirtyTrickEntangle;
                }
                else if (caster.HasFact(DirtySicken))
                {
                    maneuver = CombatManeuver.DirtyTrickSickened;
                }
                if (SweepManeuver.ActManeuver(caster, evt.Target, 0, maneuver))
                {
                    int dc = 10 + Owner.Stats.BaseAttackBonus / 2 + Owner.Stats.Intelligence.Bonus;
                    bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, dc)
                    {
                        IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
                    }, evt.Target.Context, true).Success;
                    if (!pass)
                    {
                        if (GiveNextBuff(evt.Target, BuffRefs.Confusion.Reference.Get()))
                        {
                            if (GiveNextBuff(evt.Target, BuffRefs.Staggered.Reference.Get()))
                            {
                                if (GiveNextBuff(evt.Target, BuffRefs.Exhausted.Reference.Get()))
                                {
                                    GiveNextBuff(evt.Target, BuffRefs.CantMove.Reference.Get());
                                }
                            }
                        }
                    }
                }
                Buff.Remove();
            }
        }

        private static bool GiveNextBuff(UnitEntityData target, BlueprintBuff buff)
        {
            if (target.HasFact(buff))
            {
                return true;
            }
            else
            {
                GameHelper.ApplyBuff(target, buff, new Rounds?(1.Rounds()));
                if (target.HasFact(buff))
                {
                    return false;
                }
                return true;
            }
        }

        private static BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleBlindbuffGuid);
        private static BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleEntanglebuffGuid);
        private static BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleSickenbuffGuid);
    }
}