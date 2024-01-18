using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PrestigePlus.Patch
{
    internal class PatchSkaldSharing
    {
        public static void Patch()
        {
            Add(FatedChampion.ShieldForesightFeatGuid);
            Add(BullRushFeats.Feat2Guid);
            Add(DrunkenBrute.AtavismGuid);
            Add(DrunkenBrute.GreaterAtavismGuid);
            Add(DrunkenBrute.GlareGuid);
            Add(StrengthSurge.StrengthSurgeGuid);
            Add(FatedChampion.ShieldForesightFeatGuid);
        }

        public static void Add(string guid)
        {
            var feat = BlueprintTool.GetRef<BlueprintUnitFactReference>(guid);
            if (feat?.Get() != null)
            {
                BuffConfigurator.For(BuffRefs.InspiredRageEffectBuff)
                    .EditComponent<AddFactsFromCaster>(c => { c.m_Facts = c.m_Facts.AppendToArray(feat); })
                    .Configure();
            }
        }
    }
}
