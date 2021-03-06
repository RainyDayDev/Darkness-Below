﻿using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    InventorySlot[] slots;
    Inventory inventory;
    public GameObject inventoryUI;
    public GameObject equipmentUI;
    // Use this for initialization
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    void Start () {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            equipmentUI.SetActive(!equipmentUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                Time.timeScale = 0;
            }
            else {
                Time.timeScale = 1;
            }
        }

		
	}

    void UpdateUI() {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);

            }
            else {
                slots[i].ClearSlot();
            }
        }
    }
}
