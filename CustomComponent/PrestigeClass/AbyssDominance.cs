using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AbyssDominance : FavoredEnemy
    {

        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override void OnTurnOn()
        {
            var guid = "{59352D08-B18B-45B8-B2BB-528479DAFC8F}";
            var archetype = BlueprintTool.GetRef<BlueprintFeatureReference>(guid);
            if (Owner == null)
            {
                //Logger.Info("owner null");
                Owner.Ensure<UnitPartFavoredEnemy>().AddEntry(CheckedFacts.ToArray(), Fact, 0 * Fact.GetRank());
                return;
            }
            if (Owner.Descriptor == null)
            {
                //Logger.Info("descriptor null");
                Owner.Ensure<UnitPartFavoredEnemy>().AddEntry(CheckedFacts.ToArray(), Fact, 0 * Fact.GetRank());
                return;
            }
            if (!Owner.Descriptor.HasFact(archetype))
            {
                //Logger.Info("no dominance");
                Owner.Ensure<UnitPartFavoredEnemy>().AddEntry(CheckedFacts.ToArray(), Fact, 0 * Fact.GetRank());
                return;
            }
            //Logger.Info("work favor");
            base.OnTurnOn();
        }
    }
}
