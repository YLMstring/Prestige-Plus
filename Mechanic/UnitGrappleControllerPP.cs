using Kingmaker;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.GameModes;
using JetBrains.Annotations;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Stats;
using static Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart;
using static HarmonyLib.Code;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using BlueprintCore.Blueprints.References;
using UnityEngine;
using Kingmaker.RuleSystem.Rules.Damage;
using static LayoutRedirectElement;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.Blueprints.Root;
using PrestigePlus.Modify;
using PrestigePlus.GrappleMechanic;
using PrestigePlus.Feats;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Visual.Animation.Kingmaker;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.UI.Common;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.Grapple
{
    internal class UnitGrappleControllerPP : BaseUnitController
    {
        public override void TickOnUnit(UnitEntityData unit)
        {
            TickForTarget(unit);
            TickForInitiator(unit);
            TickForKiThrow(unit);
        }

        // Token: 0x0600A6AE RID: 42670 RVA: 0x002B6B08 File Offset: 0x002B4D08s
        private static void TickForTarget(UnitEntityData unit)
        {
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = unit.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null)
            {
                return;
            }
            TimeSpan gameTime = Game.Instance.TimeController.GameTime;
            UnitEntityData value = UnitPartGrappleTargetPP.Initiator.Value;
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = value?.Get<UnitPartGrappleInitiatorPP>();
            if (value == null || UnitPartGrappleInitiatorPP == null || UnitPartGrappleInitiatorPP.Target != unit || !value.Descriptor.State.IsConscious)
            {
                unit.Remove<UnitPartGrappleTargetPP>();
                return;
            }
            if (UnitPartGrappleTargetPP.HasMoved)
            {
                ReleaseGrapple(value);
                return;
            }     
        }

        // Token: 0x0600A6AF RID: 42671 RVA: 0x002B6BD8 File Offset: 0x002B4DD8
        private static void TickForInitiator(UnitEntityData unit)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = unit.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                return;
            }
            if (UnitPartGrappleInitiatorPP.HasMoved)
            {
                ReleaseGrapple(unit);
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = value?.Get<UnitPartGrappleTargetPP>();
            if (value == null || UnitPartGrappleTargetPP == null || UnitPartGrappleTargetPP.Initiator != unit || !value.Descriptor.State.IsConscious)
            {
                unit.Remove<UnitPartGrappleInitiatorPP>();
            }
        }

        private static void TickForKiThrow(UnitEntityData unit)
        {
            UnitPartKiThrow UnitPartGrappleInitiatorPP = unit.Get<UnitPartKiThrow>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                return;
            }
            if (UnitPartGrappleInitiatorPP.HasMoved)
            {
                unit.Remove<UnitPartKiThrow>();
                return;
            }
        }

        public static void ReleaseGrapple(UnitEntityData grappleInitiator)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = grappleInitiator.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            grappleInitiator.Remove<UnitPartGrappleInitiatorPP>();
            if (value == null)
            {
                return;
            }
            value.Remove<UnitPartGrappleTargetPP>();
        }

        public static bool GrappleTrick(UnitEntityData grappleInitiator, bool isMaintain, CommandType Action)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = grappleInitiator.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                PFLog.Default.Warning("UnitPartGrappleInitiatorPP is missing", Array.Empty<object>());
                return false;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = value.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null) { return false; }
            if (grappleInitiator.HasFact(Release))
            {
                UIUtility.SendWarning(grappleInitiator.CharacterName + " releases grapple.");
                return true;
            }
            if (isMaintain)
            {
                grappleInitiator.SpendAction(Action, false, 0);
                //GameHelper.ApplyBuff(grappleInitiator, NoStandard, new Rounds?(1.Rounds()));
            }
            if (!UnitPartGrappleTargetPP.IsTiedUp && !grappleInitiator.Context.TriggerRule(new RuleCombatManeuver(grappleInitiator, value, CombatManeuver.Grapple, null)).Success)
            {
                if (isMaintain)
                {
                    UIUtility.SendWarning(grappleInitiator.CharacterName + " fails to maintain grapple.");
                }
                return isMaintain;
            }
            if (isMaintain)
            {
                UIUtility.SendWarning(grappleInitiator.CharacterName + " maintains grapple.");
            }
            if (grappleInitiator.HasFact(Bear) || grappleInitiator.HasFact(Tiger) || grappleInitiator.HasFact(Lizard) || grappleInitiator.HasFact(Griffon))
            {
                if (!grappleInitiator.HasFact(FreeBuff) && grappleInitiator.HasFact(Base))
                {
                    RunAttackRule(grappleInitiator, value);
                }
            }
            if (grappleInitiator.HasFact(Shamb))
            {
                if (!grappleInitiator.HasFact(FreeBuff) && grappleInitiator.HasFact(Base))
                {
                    RunAttackRule(grappleInitiator, value);
                    RunAttackRule(grappleInitiator, value);
                }
            }
            if (grappleInitiator.HasFact(Kraken))
            {
                GameHelper.ApplyBuff(value, BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff, duration: new Rounds?(1.Rounds()));
                return false;
            }
            if (!UnitPartGrappleTargetPP.IsPinned && !grappleInitiator.HasFact(NoPin))
            {
                UIUtility.SendWarning(grappleInitiator.CharacterName + " pins the opponent.");
                UnitPartGrappleTargetPP.TrySetPinned();
                UnitPartGrappleInitiatorPP.TrySetPinning();
                if (grappleInitiator.HasFact(StagBuff))
                {
                    if (grappleInitiator.HasFact(StagSub) && value.CanBeKnockedOff())
                    {
                        value.Descriptor.State.Prone.ShouldBeActive = true;
                        EventBus.RaiseEvent(delegate (IKnockOffHandler h)
                        {
                            h.HandleKnockOff(grappleInitiator, value);
                        }, true);
                    }
                    else if (grappleInitiator.HasFact(StagSub2))
                    {
                        RunAttackRule(grappleInitiator, value);
                    }
                }
                return false;
            }
            if (UnitPartGrappleTargetPP.IsPinned && !UnitPartGrappleTargetPP.IsTiedUp && !grappleInitiator.HasFact(NoTieUp))
            {
                UIUtility.SendWarning(grappleInitiator.CharacterName + " ties up the opponent.");
                UnitPartGrappleTargetPP.TrySetTiedUp();
                UnitPartGrappleInitiatorPP.TryClearPinning();
                return false;
            }
            if (grappleInitiator.HasFact(FreeBuff) && !grappleInitiator.CombatState.EngagedUnits.Contains(value))
            {
                return false;
            }
            RunAttackRule(grappleInitiator, value);
            if (UnitPartGrappleTargetPP.IsPinned && grappleInitiator.HasFact(KnockOut))
            {
                RunAttackRule(grappleInitiator, value);
            }
            if (UnitPartGrappleTargetPP.IsPinned && grappleInitiator.HasFact(Rend))
            {
                value.AddBuff(Bleed, grappleInitiator);
            }
            if (grappleInitiator.HasFact(Slam))
            {
                if (value.CanBeKnockedOff())
                {
                    value.Descriptor.State.Prone.ShouldBeActive = true;
                    EventBus.RaiseEvent(delegate (IKnockOffHandler h)
                    {
                        h.HandleKnockOff(grappleInitiator, value);
                    }, true);
                }
                if (grappleInitiator.HasFact(Drama))
                {
                    AbilityData spellData = new AbilityData(Display, grappleInitiator);
                    Rulebook.Trigger(new RuleCastSpell(spellData, grappleInitiator)
                    {
                        IsDuplicateSpellApplied = false
                    });
                }
                if (grappleInitiator.HasFact(Uncanny))
                {
                    EnergyDamage Damage = new(new DiceFormula(grappleInitiator.Descriptor.Progression.MythicLevel, DiceType.D6), 0, DamageEnergyType.Divine);
                    Rulebook.Trigger(new RuleDealDamage(grappleInitiator, value, Damage));
                    var feet = 10 * Feet.FeetToMetersRatio * grappleInitiator.Descriptor.Progression.MythicLevel;
                    Vector3 normalized = (value.Position - grappleInitiator.Position).normalized;
                    value.Ensure<UnitPartForceMove>().Push(normalized, feet, false);
                }
                ReleaseGrapple(grappleInitiator);
            }
            return false;
        }

        //private static BlueprintBuffReference NoStandard = BlueprintTool.GetRef<BlueprintBuffReference>(TabletopGrapplingInfusion.TabletopGrapplingbuff3Guid);
        public static bool TryBreakFree(UnitEntityData grappleInitiator, UnitEntityData grappleTarget)
        {
            if (grappleInitiator == null)
            {
                return false;
            }
            if (grappleTarget == null)
            {
                return false;
            }
            if (grappleTarget.Get<UnitPartGrappleTargetPP>() == null) { return false; }
            var context1 = grappleInitiator.Context;
            var context2 = grappleTarget.Context;
            RuleCalculateCMD ruleCalculateCMD = new RuleCalculateCMD(grappleTarget, grappleInitiator, CombatManeuver.Grapple);
            ruleCalculateCMD = (context1?.TriggerRule(ruleCalculateCMD)) ?? Rulebook.Trigger(ruleCalculateCMD);
            int dc = ruleCalculateCMD.Result;
            if (grappleTarget.Get<UnitPartGrappleTargetPP>().IsTiedUp)
            {
                RuleCalculateCMB ruleCalculateCMB2 = new RuleCalculateCMB(grappleInitiator, grappleTarget, CombatManeuver.Grapple);
                ruleCalculateCMB2 = (context1?.TriggerRule(ruleCalculateCMB2)) ?? Rulebook.Trigger(ruleCalculateCMB2);
                dc = Math.Max(ruleCalculateCMB2.Result + 20, dc);
            }
            RuleCalculateCMB ruleCalculateCMB = new RuleCalculateCMB(grappleTarget, grappleInitiator, CombatManeuver.Grapple);
            ruleCalculateCMB = (context2?.TriggerRule(ruleCalculateCMB)) ?? Rulebook.Trigger(ruleCalculateCMB);
            if (grappleTarget.Stats.SkillThievery < ruleCalculateCMB.Result)
            {
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(grappleTarget, grappleInitiator, CombatManeuver.Grapple, null);
                ruleCombatManeuver = (context2?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                return ruleCombatManeuver.Success;
            }
            StatType statType = StatType.SkillThievery;
            return GameHelper.TriggerSkillCheck(new RuleSkillCheck(grappleTarget, statType, dc)
            {
                IgnoreDifficultyBonusToDC = grappleTarget.IsPlayersEnemy
            }, context2, true).Success;
        }

        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.GetThreatHandMelee();
            if (weapon == null) { return; }
            if (weapon.Weapon.Blueprint.IsTwoHanded && !maybeCaster.HasFact(HamatulaStrike))
            {  
                return; 
            }
            if (!weapon.Weapon.Blueprint.IsMelee)
            {
                return; 
            }
            var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
            maybeCaster.View.AnimationManager.Execute(attackAnimation);
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, weapon.Weapon, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = true,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
        }

        private static BlueprintBuffReference Release = BlueprintTool.GetRef<BlueprintBuffReference>("{AD21943C-2AC2-465B-8A1E-F99F3446EBE4}");
        private static BlueprintBuffReference NoPin = BlueprintTool.GetRef<BlueprintBuffReference>("{D3B37428-69BE-43F2-83DA-04A38D35CDCB}");
        private static BlueprintBuffReference NoTieUp = BlueprintTool.GetRef<BlueprintBuffReference>("{B14E98A0-53AC-4212-86A0-29D1CA1D8446}");
        private static BlueprintBuffReference Bleed = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Bleed3d6Buff.ToString());
        private static BlueprintBuffReference Slam = BlueprintTool.GetRef<BlueprintBuffReference>("{0A0ABEDD-D762-4148-8948-272FFCDC336D}");
        private static BlueprintBuffReference Uncanny = BlueprintTool.GetRef<BlueprintBuffReference>("{E4A64303-1E48-4339-AF81-B4D1BD00DB74}");
        private static BlueprintBuffReference HamatulaStrike = BlueprintTool.GetRef<BlueprintBuffReference>("{2AF7906A-C641-4596-B6A7-DF1F0CDA8758}");
        private static BlueprintBuffReference FreeBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D4DD258E-B9F1-42D1-9BD0-ADBD217AFE23}");
        private static BlueprintBuffReference StagBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{21F094D4-1D59-400B-9CEB-558E6218FB0C}");
        private static BlueprintBuffReference StagSub = BlueprintTool.GetRef<BlueprintBuffReference>("{166DF6CC-25B6-4864-9F1D-C9EFF2AA6869}");
        private static BlueprintFeatureReference StagSub2 = BlueprintTool.GetRef<BlueprintFeatureReference>(StagStyle.SubmissionGuid);
        private static BlueprintBuffReference Kraken = BlueprintTool.GetRef<BlueprintBuffReference>("{E355834F-3B1F-4790-A8CA-01F66683550A}");

        private static BlueprintFeatureReference Base = BlueprintTool.GetRef<BlueprintFeatureReference>("{D74F645A-D0F2-470B-B68B-E76EC083A6D8}");

        private static BlueprintFeatureReference Drama = BlueprintTool.GetRef<BlueprintFeatureReference>("{201C54A4-F2D2-43BA-A4AA-7A9F53745896}");
        private static BlueprintFeatureReference Rend = BlueprintTool.GetRef<BlueprintFeatureReference>("{0B5D02BD-68EA-429F-9F2A-BE7BDBEC5484}");
        private static BlueprintFeatureReference KnockOut = BlueprintTool.GetRef<BlueprintFeatureReference>("{42F2A645-479F-4444-A967-0ECBD3CA5585}");

        private static BlueprintFeatureReference Bear = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabBear.ToString());
        private static BlueprintFeatureReference Tiger = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabTiger.ToString());
        private static BlueprintFeatureReference Lizard = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabLizard.ToString());
        private static BlueprintFeatureReference Griffon = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabGriffon.ToString());
        private static BlueprintFeatureReference Shamb = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShamblingMoundGrabFeature.ToString());

        private static BlueprintAbilityReference Display = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.DazzlingDisplayAction.ToString());

    }

    [HarmonyPatch(typeof(GameModesFactory), nameof(GameModesFactory.Initialize))]
    class PatchGrapple
    {
        static void Postfix()
        {
            GameModesFactory.Register(new UnitGrappleControllerPP(), new GameModeType[] { GameModesFactory.Default });
            GameModesFactory.Register(new GiganticAssaultController(), new GameModeType[] { GameModesFactory.Default });
        }
    }
}

