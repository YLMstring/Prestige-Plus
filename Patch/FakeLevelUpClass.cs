using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.Patch
{
    internal class FakeLevelUpClass : UnitFactComponentDelegate<FakeLevelUpClass.ComponentData>, IUnitSubscriber, ISubscriber
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override void OnActivate()
        {
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            var data = Owner.Progression.GetClassData(clazz);
            if (data == null) { return; }
            data.Level += 1;
            Data.added += 1;
            LevelUpHelper.UpdateProgression(controller.State, Owner, clazz.Progression);
        }

        public override void OnDeactivate()
        {
            var data = Owner.Progression.GetClassData(clazz);
            if (data == null) { return; }
            data.Level -= Data.added;
            Data.added = 0;
        }
        public class ComponentData
        {
            public int added = 0;
        }

        public BlueprintCharacterClass clazz;
    }
}
