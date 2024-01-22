using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Mechanic;

namespace PrestigePlus.CustomAction
{
    internal class AlignedClassClear : ContextAction
    {
        public override string GetCaption()
        {
            return "AlignedClassClear";
        }

        public override void RunAction()
        {
            Target?.Unit?.Ensure<UnitPartAlignedClass>().Clear();
        }
    }
}