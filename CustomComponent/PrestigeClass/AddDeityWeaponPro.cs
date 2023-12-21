using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AddDeityWeaponPro : UnitFactComponentDelegate<AddDeityWeaponPro.ComponentData>
    {
        public override void OnTurnOn()
        {
            Data.cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
            if (Data.cat.Count() == 0)
            {
                return;
            }
            foreach (var item in Data.cat)
            {
                Owner.Proficiencies.Add(item);
            }
        }

        // Token: 0x0600C23C RID: 49724 RVA: 0x0032B7EC File Offset: 0x003299EC
        public override void OnTurnOff()
        {
            if (Data.cat.Count() == 0)
            {
                return;
            }
            foreach (var item in Data.cat)
            {
                Owner.Proficiencies.Remove(item);
            }
        }

        public class ComponentData
        {
            public List<WeaponCategory> cat = new() { };
        }
    }
}
