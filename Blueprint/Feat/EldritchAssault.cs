using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.CustomComponent.Feat;

namespace PrestigePlus.Blueprint.Feat
{
    internal class EldritchAssault
    {
        private const string EldritchAssaultFeat3 = "Feat.EldritchAssaultFeat3";
        private static readonly string EldritchAssaultFeatGuid3 = "{48359477-2F43-48E4-91A4-C8D3C22A3329}";

        private const string EldritchAssaultFeatBuff = "Feat.EldritchAssaultFeatBuff";
        private static readonly string EldritchAssaultFeatBuffGuid = "{AA9D6816-0A5E-486E-A91C-6BC78108CD9F}";

        internal const string EldritchAssaultFeatDisplayName = "FeatEldritchAssaultFeat.Name";
        private const string EldritchAssaultFeatDescription = "FeatEldritchAssaultFeat.Description";
        public static void EldritchAssaultFeatFeat()
        {
            var icon = AbilityRefs.SenseVitals.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(EldritchAssaultFeatBuff, EldritchAssaultFeatBuffGuid)
             .SetDisplayName(EldritchAssaultFeatDisplayName)
             .SetDescription(EldritchAssaultFeatDescription)
             .SetIcon(icon)
             .AddComponent<EldritchAssaultCheck>()
             .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveSelf().Build(), true, criticalHit: true)
             //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            FeatureConfigurator.New(EldritchAssaultFeat3, EldritchAssaultFeatGuid3, FeatureGroup.Feat)
              .SetDisplayName(EldritchAssaultFeatDisplayName)
              .SetDescription(EldritchAssaultFeatDescription)
              .SetIcon(icon)
              .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().ApplyBuff(Buff1, ContextDuration.Fixed(1)))
              .AddPrerequisiteFeature(FeatureRefs.BlindFight.ToString())
              .AddPrerequisiteFeature(FeatureRefs.Improved_Initiative.ToString())
              .Configure();

        }
    }
}
