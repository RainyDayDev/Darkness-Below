using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("More than one instance of Inventory");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public delegate void onItemChanged();
    public onItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();

    public int space = 20;


    public bool Add(Item item) {
        if (!item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Out of space");
                return false;

            }

            items.Add(item);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }

        }

        return true;

    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }


}
