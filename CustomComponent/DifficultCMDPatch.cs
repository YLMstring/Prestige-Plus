using Kingmaker.Blueprints.Root;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Settings.Difficulty;
using Kingmaker.Settings;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.RuleSystem;

namespace PrestigePlus.CustomComponent
{
    internal class DifficultCMDPatch : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateCMD>, IRulebookHandler<RuleCalculateCMD>, ISubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateCMD evt)
        {
            if (!Owner.IsPlayersEnemy) { return; }
            int num = SettingsRoot.Difficulty.EnemyDifficulty - 1;
            DifficultyPresetsList.StatsAdjustmentPreset adjustmentPreset = BlueprintRoot.Instance.DifficultyList.GetAdjustmentPreset(SettingsRoot.Difficulty.StatsAdjustments);
            int value = 4 * (adjustmentPreset.BasicStatBonusMultiplier + num);
            UnitPartSummonedMonster unitPartSummonedMonster = base.Owner.Get<UnitPartSummonedMonster>();
            int? num4;
            if (unitPartSummonedMonster == null)
            {
                num4 = null;
            }
            else
            {
                UnitEntityData summoner = unitPartSummonedMonster.Summoner;
                num4 = ((summoner != null) ? new int?(summoner.CR) : null);
            }
            int cr = num4 ?? base.Owner.CR;
            value = DifficultyStatAdvancement.ApplyBonusProgression(adjustmentPreset, cr, value, true);
            if (Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted)
            {
                value /= 2;
            }
            evt.AddModifier(-value, Fact, Kingmaker.Enums.ModifierDescriptor.Penalty);
        }

        public void OnEventDidTrigger(RuleCalculateCMD evt)
        {
            
        }
    }
}
