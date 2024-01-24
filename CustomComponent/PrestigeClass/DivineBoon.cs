using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.UnitLogic;
using Kingmaker.PubSubSystem;
using Kingmaker.Designers.Mechanics.Facts;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Blueprints.Classes;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class DivineBoon: UnitFactComponentDelegate<AddFeatureIfHasFactData>, IUnitSubscriber, ISubscriber
	{
        // Token: 0x0600E3C2 RID: 58306 RVA: 0x003AA3DF File Offset: 0x003A85DF
        public override void OnActivate()
        {
            this.Apply();
        }

        // Token: 0x0600E3C3 RID: 58307 RVA: 0x003AA3E7 File Offset: 0x003A85E7
        public override void OnDeactivate()
        {
            base.Owner.RemoveFact(base.Data.AppliedFact);
            base.Data.AppliedFact = null;
        }
        // Token: 0x0600E3C5 RID: 58309 RVA: 0x003AA414 File Offset: 0x003A8614
        private void Apply()
        {
            if (base.Data.AppliedFact != null)
            {
                return;
            }
            var list = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(DeificObedience.DeificObedienceGuid)?.Get()?.AllFeatures;
            foreach (var feat in list)
            {
                if (!Owner.HasFact(feat)) { continue; }
                var pro = feat as BlueprintProgression;
                pro ??= feat.IsPrerequisiteFor?.First()?.Get() as BlueprintProgression;
                if (pro == null || !Owner.HasFact(pro)) { continue; }
                var boon = pro.GetLevelEntry(level)?.Features?.First();
                if (boon == null) { continue; }
                base.Data.AppliedFact = Owner.AddFact(boon, null, null);
            }
        }

        public int level;
    }
}
