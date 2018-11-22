﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    Player player;

    private void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        player = FindObjectOfType<Player>();
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem) {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
            health.AddModifier(newItem.healthModifier);
            magic.AddModifier(newItem.magicModifier);
            maxHealth = health.GetValue();
            player.updateUI();
        }
        if (oldItem != null) {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
            health.AddModifier(oldItem.healthModifier);
            magic.AddModifier(oldItem.magicModifier);
            player.updateUI();
        }
    }

}
