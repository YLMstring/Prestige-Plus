using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Items.Slots;
using Kingmaker.Blueprints.Facts;

namespace PrestigePlus.CustomComponent
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("{C9384AA7-88A4-490D-8685-7AE4641602A6}")]
    internal class ArmorUnlockPP : UnitFactComponentDelegate<ArmorUnlockPP.ArmorFeatureUnlockData>,
        IUnitActiveEquipmentSetHandler,
        IUnitLevelUpHandler,
        IGlobalSubscriber, ISubscriber,
        IUnitEquipmentHandler
    {
        public override void OnTurnOn()
        {
            base.OnTurnOn();
            Update();
        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();
            RemoveFact();
        }

        public override void OnActivate()
        {
            Update();
        }

        public override void OnDeactivate()
        {
            RemoveFact();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            Update();
        }

        private bool ShouldApply()
        {
            bool naked = false;
            if (Owner.Body.IsPolymorphed) { naked = true; }
            var Armor = Owner.Body.Armor?.MaybeArmor;
            if (Armor == null) { naked = true; }
            if (naked) { return NoArmor; }
            return RequiredArmor.Contains(Armor.Blueprint.ProficiencyGroup) == !Invert;
        }

        private void AddFact()
        {
            if (Data.AppliedFact == null)
            {
                Data.AppliedFact = Owner.AddFact(NewFact, null, null);
            }
        }

        private void RemoveFact()
        {
            if (Data.AppliedFact != null)
            {
                Owner.RemoveFact(Data.AppliedFact);
                Data.AppliedFact = null;
            }
        }

        private void Update()
        {
            if (ShouldApply())
            {
                AddFact();
            }
            else
            {
                RemoveFact();
            }
        }
        public void HandleUnitBeforeLevelUp(UnitEntityData unit)
        {
        }

        public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
        {
            Update();
        }

        void IUnitEquipmentHandler.HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if (slot.Owner != Owner)
            {
                return;
            }
            if (!slot.Active)
            {
                return;
            }
            Update();
        }

        [SerializeField]
        [FormerlySerializedAs("NewFact")]
        public BlueprintUnitFactReference NewFact;
        public ArmorProficiencyGroup[] RequiredArmor = new ArmorProficiencyGroup[0];
        public bool Invert = false;
        public bool NoArmor = false;

        [TypeId("{B8C047E0-4E9F-4492-803C-D4410F2C101B}")]
        public class ArmorFeatureUnlockData
        {
            [JsonProperty]
            public EntityFact AppliedFact;
        }
    }
}