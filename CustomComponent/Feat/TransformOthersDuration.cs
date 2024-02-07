using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Controllers;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class TransformOthersDuration : UnitFactComponentDelegate, IUnitBuffHandler, IGlobalSubscriber, ISubscriber
    {
        public void HandleBuffDidAdded(Buff buff)
        {
            MechanicsContext maybeContext = buff.MaybeContext;
            if (maybeContext?.MaybeCaster == Owner && buff.TimeLeft <= 12.Hours())
            {
                AbilityExecutionContext sourceAbilityContext = buff.Context?.SourceAbilityContext;
                AbilityData abilityData = sourceAbilityContext?.Ability;
                if (abilityData?.ConvertedFrom?.Blueprint == Trans.Get())
                {
                    buff.SetEndTime(12.Hours() + buff.AttachTime);
                }
            }
        }

        // Token: 0x0600E5DC RID: 58844 RVA: 0x003B15F1 File Offset: 0x003AF7F1
        public void HandleBuffDidRemoved(Buff buff)
        {
        }

        private static BlueprintAbilityReference Trans = BlueprintTool.GetRef<BlueprintAbilityReference>(DeificObedience.LanternKing2Ablity3Guid);
    }
}
