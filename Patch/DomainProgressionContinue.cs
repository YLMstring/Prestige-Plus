using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using BlueprintCore.Blueprints.References;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.Patch
{
    internal class DomainProgressionContinue : UnitFactComponentDelegate, IUnitSubscriber, ISubscriber
    {

        public override void OnActivate()
        {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            var unit = Owner;
            var list = FeatureSelectionRefs.DomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var f in list)
            {
                TryUpdateProgression(f);
            }
            var list2 = FeatureSelectionRefs.SecondDomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var f in list2)
            {
                TryUpdateProgression(f);
            }
            var list3 = FeatureSelectionRefs.DruidDomainSelection.Reference.Get().m_AllFeatures;
            foreach (var f in list3)
            {
                TryUpdateProgression(f);
            }
            var list4 = FeatureSelectionRefs.SecondDomainsSeparatistSelection.Reference.Get().m_AllFeatures;
            foreach (var f in list4)
            {
                TryUpdateProgression(f);
            }

            void TryUpdateProgression(BlueprintFeatureReference fref)
            {
                BlueprintProgression prog = (BlueprintProgression)fref.Get();
                if (unit.Progression.GetProgression(prog) != null)
                    LevelUpHelper.UpdateProgression(controller.State, unit, prog);
            }
        }
    }
}

