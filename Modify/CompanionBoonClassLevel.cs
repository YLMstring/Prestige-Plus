using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using BlueprintCore.Utils;
using PrestigePlus.PrestigeClasses;

namespace PrestigePlus.Modify
{
    internal class CompanionBoonClassLevel : UnitFactComponentDelegate<CompanionBoonData>, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber
    {
        // Token: 0x170024A8 RID: 9384
        // (get) Token: 0x0600E211 RID: 57873 RVA: 0x003A08B4 File Offset: 0x0039EAB4
        public BlueprintFeature RankFeature
        {
            get
            {
                BlueprintFeatureReference rankFeature = this.m_RankFeature;
                if (rankFeature == null)
                {
                    return null;
                }
                return rankFeature.Get();
            }
        }

        // Token: 0x0600E212 RID: 57874 RVA: 0x003A08C7 File Offset: 0x0039EAC7
        public override void OnActivate()
        {
            this.Apply();
        }

        // Token: 0x0600E213 RID: 57875 RVA: 0x003A08CF File Offset: 0x0039EACF
        public override void OnDeactivate()
        {
            while (base.Data.AppliedRank > 0)
            {
                base.Owner.RemoveFact(this.RankFeature);
                base.Data.AppliedRank--;
            }
        }

        // Token: 0x0600E214 RID: 57876 RVA: 0x003A0908 File Offset: 0x0039EB08
        private void Apply()
        {
            EntityFact fact = base.Owner.GetFact(this.RankFeature);
            int i = (fact != null) ? fact.GetRank() : 0;
            int characterLevel = base.Owner.Progression.CharacterLevel;
            int bonus = Owner.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(AnchoriteofDawn.ArchetypeGuid));
            while (base.Data.AppliedRank < bonus)
            {
                if (i >= characterLevel)
                {
                    break;
                }
                EntityFact entityFact = base.Owner.AddFact(this.RankFeature, null, null);
                base.Data.AppliedRank++;
                i = entityFact.GetRank();
            }
            while (i > characterLevel)
            {
                base.Owner.RemoveFact(this.RankFeature);
                base.Data.AppliedRank--;
                EntityFact fact2 = base.Owner.GetFact(this.RankFeature);
                i = ((fact2 != null) ? fact2.GetRank() : 0);
            }
        }

        // Token: 0x0600E215 RID: 57877 RVA: 0x003A09D1 File Offset: 0x0039EBD1
        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            this.Apply();
        }

        // Token: 0x04009492 RID: 38034
        [SerializeField]
        [FormerlySerializedAs("RankFeature")]
        public BlueprintFeatureReference m_RankFeature;

        // Token: 0x04009493 RID: 38035
    }
}

