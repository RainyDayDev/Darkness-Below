using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public string description;
    public int cost;

    public virtual void Use() {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory() {
        Inventory.instance.Remove(this);
    }

}
