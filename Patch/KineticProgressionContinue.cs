using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class KineticProgressionContinue : UnitFactComponentDelegate, IUnitSubscriber, ISubscriber
    {

        public override void OnActivate()
        {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            var unit = Owner;
            var pro = ProgressionRefs.KineticBlastProgression.Reference.Get();
            TryUpdateProgression(pro);
            
            void TryUpdateProgression(BlueprintProgression prog)
            {
                if (unit.Progression.GetProgression(prog) != null)
                    LevelUpHelper.UpdateProgression(controller.State, unit, prog);
            }
        }
    }
}
