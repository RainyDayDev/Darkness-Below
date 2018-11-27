using UnityEngine;

public class ItemPickup : Interactable {

    public Item item;

    private void Start()
    {
    }

    public override void Interact()
    {
        if (item == null) {
            Equipment newItem = ScriptableObject.CreateInstance<Equipment>();
            newItem.equipSlot = EquipmentSlot.Chest;

            newItem.init(1, 1, 1, 1);
            item = newItem;
        }
        base.Interact();

        PickUp();
    }

    void PickUp() {
        Debug.Log("Picking up "+item.name);
        //Add to inventory
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

    }

}
