using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;
    public Text descriptionText;
    public Image descriptionImage;
    Item item;

    public void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        descriptionText.enabled = true;
        descriptionImage.enabled = true;
        descriptionText.text = newItem.description;

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



    public void OnRemoveButton() {
        //Bring up confirm dialogue?
        Inventory.instance.Remove(item);
    }

    public void UseItem() {
        if (item != null) {
            item.Use();
        }
    }

}

