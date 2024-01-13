using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;
using System.Security.AccessControl;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;

namespace PrestigePlus.Modify
{
    internal class GiganticSteedLogic : UnitFactComponentDelegate, IOwnerGainLevelHandler, IUnitSubscriber, ISubscriber
    {
        public override void OnTurnOn()
        {
            this.Apply();
        }

        // Token: 0x0600E570 RID: 58736 RVA: 0x003AB2CE File Offset: 0x003A94CE
        public override void OnTurnOff()
        {
            TryRemove();
        }

        // Token: 0x0600E571 RID: 58737 RVA: 0x003AB2EB File Offset: 0x003A94EB
        public void HandleUnitGainLevel()
        {
            this.Apply();
        }

        // Token: 0x0600E572 RID: 58738 RVA: 0x003AB2F4 File Offset: 0x003A94F4
        private void Apply()
        {
            TryRemove();
            TryAdd();
        }

        // Token: 0x0400968F RID: 38543
        private static BlueprintCharacterClassReference rider = BlueprintTool.GetRef<BlueprintCharacterClassReference>("{FBD0C6EB-0513-483B-862F-A087D32B03D1}");
        private void TryAdd()
        {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in base.Owner.Pets)
            {
                int level = Owner.Descriptor.Progression.GetClassLevel(rider);
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = (entity != null) ? entity.Get<UnitPartPet>() : null;
                if (unitPartPet != null && unitPartPet.Type == PetType.AnimalCompanion)
                {
                    if (level >= 9)
                    {
                        entity.Stats.Strength.AddModifier(10, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.Constitution.AddModifier(6, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.AC.AddModifier(4, base.Runtime, ModifierDescriptor.ArmorFocus);
                    }
                    else if (level >= 7) 
                    {
                        entity.Stats.Strength.AddModifier(8, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.Constitution.AddModifier(4, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.AC.AddModifier(3, base.Runtime, ModifierDescriptor.ArmorFocus);
                    }
                    else if (level >= 5)
                    {
                        entity.Stats.Strength.AddModifier(6, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.Constitution.AddModifier(4, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.AC.AddModifier(2, base.Runtime, ModifierDescriptor.ArmorFocus);
                    }
                    else if (level >= 3)
                    {
                        entity.Stats.Strength.AddModifier(4, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.Constitution.AddModifier(2, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.AC.AddModifier(1, base.Runtime, ModifierDescriptor.ArmorFocus);
                    }
                    else
                    {
                        entity.Stats.Strength.AddModifier(2, base.Runtime, ModifierDescriptor.UntypedStackable);
                        entity.Stats.Constitution.AddModifier(2, base.Runtime, ModifierDescriptor.UntypedStackable);
                    }
                    //entity.Stats.Dexterity.AddModifier(-2, base.Runtime, ModifierDescriptor.Penalty);
                }
            }
        }

        // Token: 0x0600E0DC RID: 57564 RVA: 0x0039C84C File Offset: 0x0039AA4C
        private void TryRemove()
        {
            foreach (EntityPartRef<UnitEntityData, UnitPartPet> entityPartRef in base.Owner.Pets)
            {
                UnitEntityData entity = entityPartRef.Entity;
                UnitPartPet unitPartPet = (entity != null) ? entity.Get<UnitPartPet>() : null;
                if (unitPartPet != null)
                {
                    entity.Stats.Strength.RemoveModifiersFrom(base.Runtime);
                    //entity.Stats.Dexterity.RemoveModifiersFrom(base.Runtime);
                    entity.Stats.Constitution.RemoveModifiersFrom(base.Runtime);
                    entity.Stats.AC.RemoveModifiersFrom(base.Runtime);
                }
            }
        }
    }
}
