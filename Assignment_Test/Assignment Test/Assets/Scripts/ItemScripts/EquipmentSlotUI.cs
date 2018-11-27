using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour {

    public Image icon;
    public Button removeButton;
    public Text descriptionText;
    public Image descriptionImage;
    Equipment item;
    EquipmentManager manager;
    int ItemSlot;

    private void Start()
    {
        manager = EquipmentManager.instance;
    }

    public void AddItem(Equipment newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        descriptionText.enabled = true;
        descriptionImage.enabled = true;
        descriptionText.text = newItem.description;
        ItemSlot = (int)item.equipSlot;

    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        descriptionText.enabled = false;
        descriptionImage.enabled = false;

    }



    public void OnRemoveButton()
    {
        //Bring up confirm dialogue?
        EquipmentManager.instance.Unequip(ItemSlot);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}
