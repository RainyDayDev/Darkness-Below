using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName ="Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;
    public int armorModifier;
    public int damageModifier;
    public int healthModifier;
    public int magicModifier;

    public void init(int armorMod, int damageMod, int healthMod, int magicMod) {
        this.armorModifier = armorMod;
        this.damageModifier = damageMod;
        this.healthModifier = healthMod;
        this.magicModifier = magicMod;
        this.description = "Health +" + healthMod + "\nArmor +" + armorMod + "\nDamage +" + damageMod + "\nMagic +" + magicMod;
    }

    void Start() {
        this.description = "Health +" + this.healthModifier + "\nArmor +" + this.armorModifier + "\nDamage +" + this.damageModifier + "\nMagic +" + this.magicModifier;
    }

    public override void Use()
    {
        base.Use();
        //Equip the item
        //Remove from inventory
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();

    }

}


public enum EquipmentSlot {Head, Chest, Legs, Weapons}