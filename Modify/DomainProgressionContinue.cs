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

namespace PrestigePlus.Modify
{
    internal class DomainProgressionContinue : UnitFactComponentDelegate, IUnitLevelUpHandler
    {
        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
        {
            var list = FeatureSelectionRefs.DomainsSelection.Reference.Get().m_AllFeatures;
            foreach ( var f in list )
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
        public void HandleUnitBeforeLevelUp(UnitEntityData unit) { }
    }
}

