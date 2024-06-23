using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Newtonsoft.Json;
using System.Linq;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SpearFlurryUnlock :
    UnitFactComponentDelegate<SpearFlurryUnlockData>,
    IUnitActiveEquipmentSetHandler,
    IGlobalSubscriber,
    ISubscriber,
    IUnitEquipmentHandler
    {
        public BlueprintArchetype SoheiArchetype;
        public BlueprintFeature[] Flurry1stfact;
        public BlueprintFeature[] Flurry2ndfact;

        public BlueprintUnitFact Flurry1;
        public BlueprintUnitFact Flurry11;

        public override void OnActivate() => CheckEligibility();

        public override void OnDeactivate() => RemoveFact();

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit) => this.CheckEligibility();

        private bool IsDeityMeleeFavoredWeaponWithWeaponFocus()
        {
            if (Owner.Body.PrimaryHand.MaybeWeapon == null) return false;
            var primaryWeapon = Owner.Body.PrimaryHand.Weapon.Blueprint;
            if (primaryWeapon.FighterGroup.Contains(WeaponFighterGroup.Spears) || primaryWeapon.FighterGroup.Contains(WeaponFighterGroup.Polearms))
            {
                return true;
            }
            return false;
        }

        private void CheckEligibility()
        {
            var isSoheiArchetype = Owner.Progression.IsArchetype(SoheiArchetype);

            var hasShield = Owner.Body.SecondaryHand.HasShield;
            if (!isSoheiArchetype)
            {
                //not Sohei
                var noArmor = !Owner.Body.Armor.HasArmor || !Owner.Body.Armor.Armor.Blueprint.IsArmor;
                var isMonkWeapon = Owner.Body.PrimaryHand.Weapon.Blueprint.IsMonk;
                if (!hasShield && noArmor && !isMonkWeapon && IsDeityMeleeFavoredWeaponWithWeaponFocus())
                {
                    AddFact();
                }
                else RemoveFact();
            }
            else
            {
                //Sohei
                bool noArmorOrLightArmor = false;
                if (!Owner.Body.Armor.HasArmor)
                    noArmorOrLightArmor = true;
                else if (Owner.Body.Armor.MaybeArmor != null)
                    noArmorOrLightArmor = !Owner.Body.Armor.MaybeArmor.Blueprint.IsArmor
                        || Owner.Body.Armor.MaybeArmor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light;

                bool wieldingWeaponWithWTOrMonkWeapon = Owner.Get<UnitPartWeaponTraining>() != null
                    ? Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.IsMonk ||
                        Owner.Get<UnitPartWeaponTraining>().IsSuitableWeapon(Owner.Body.PrimaryHand.MaybeWeapon)
                    : Owner.Body.PrimaryHand.MaybeWeapon.Blueprint.IsMonk;

                if (!hasShield && noArmorOrLightArmor && !wieldingWeaponWithWTOrMonkWeapon && IsDeityMeleeFavoredWeaponWithWeaponFocus()) AddFact();
                else RemoveFact();
            }
        }

        private void AddFact()
        {
            if (Data.Flurry1 == null && Flurry1stfact.Any(x => Owner.HasFact(x)))
            {
                Data.Flurry1 = Owner.AddFact(Flurry1);
            }
            if (Data.Flurry11 == null && Flurry2ndfact.Any(x => Owner.HasFact(x)))
            {
                Data.Flurry11 = Owner.AddFact(Flurry11);
            }
        }

        private void RemoveFact()
        {
            if (Data.Flurry1 != null)
            {
                Owner.RemoveFact(Data.Flurry1);
                Data.Flurry1 = null;
            }
            if (Data.Flurry11 != null)
            {
                Owner.RemoveFact(Data.Flurry11);
                Data.Flurry11 = null;
            }
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if ((UnitEntityData)slot.Owner != (UnitDescriptor)Owner || !slot.Active)
                return;
            CheckEligibility();
        }
    }

    public class SpearFlurryUnlockData
    {
        [JsonProperty]
        public EntityFact Flurry1;
        [JsonProperty]
        public EntityFact Flurry11;
    }
}