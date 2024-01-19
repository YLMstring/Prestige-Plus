using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.Patch
{
    internal class FakeLevelUpClass : UnitFactComponentDelegate, IUnitSubscriber, ISubscriber
    {
        public override void OnActivate()
        {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            var data = Owner.Progression.GetClassData(clazz);
            if (data != null) { return; }
            data.Level += 1;
            LevelUpHelper.UpdateProgression(controller.State, Owner, clazz.Progression);
        }

        public BlueprintCharacterClass clazz;
    }
}
