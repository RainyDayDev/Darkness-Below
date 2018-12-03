using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemPickup : Interactable {

    public Item item;
    Player Player;
    bool hasInit = false;
    public Text descriptionText;
    public GameObject canvas;
    //sprite icons
    public Sprite head1;
    public Sprite head2;
    public Sprite head3;

    public Sprite chest1;
    public Sprite chest2;
    public Sprite chest3;

    public Sprite leg1;
    public Sprite leg2;
    public Sprite leg3;

    public Sprite wep1;
    public Sprite wep2;
    public Sprite wep3;

    private void initialize()
    {
        //Player player = FindObjectOfType<Player>();
        if (item == null)
        {
            Equipment newItem = ScriptableObject.CreateInstance<Equipment>();
            int iconChance = Random.Range(1, 4);
            int chance = Random.Range(1, 5);
            int qualityChance = Random.Range(0, 2);
            int attackChance = Random.Range(0, 100);
            int armorChance = Random.Range(0, 100);
            int healthChance = Random.Range(0, 100);
            int magicChance = Random.Range(0, 100);
            int level;
            int attack = 0, armor = 0, health = 0, magic = 0;
            //Player player = FindObjectOfType<Player>();
            if (SceneManager.GetActiveScene().name == "Tavern")
            {
                level = Player.farthestLevel;
            }
            else
            {
                level = Player.currentLevel;
            }
            if (chance == 1)
            {
                newItem.equipSlot = EquipmentSlot.Head;
                if (iconChance == 1)
                {
                    newItem.icon = head1;
                }
                else if (iconChance == 2)
                {
                    newItem.icon = head2;
                }
                else {
                    newItem.icon = head3;
                }
                if (attackChance >= 70)
                {
                    attack = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (armorChance >= 0)
                {
                    armor = 1 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (healthChance >= 25)
                {
                    health = 5 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (magicChance >= 70)
                {
                    magic = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }

            }
            else if (chance == 2)
            {
                if (iconChance == 1)
                {
                    newItem.icon = chest1;
                }
                else if (iconChance == 2)
                {
                    newItem.icon = chest2;
                }
                else
                {
                    newItem.icon = chest3;
                }
                newItem.equipSlot = EquipmentSlot.Chest;
                if (attackChance >= 70)
                {
                    attack = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (armorChance >= 0)
                {
                    armor = 3 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (healthChance >= 25)
                {
                    health = 10 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (magicChance >= 70)
                {
                    magic = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }

            }
            else if (chance == 3)
            {
                if (iconChance == 1)
                {
                    newItem.icon = leg1;
                }
                else if (iconChance == 2)
                {
                    newItem.icon = leg2;
                }
                else
                {
                    newItem.icon = leg3;
                }
                newItem.equipSlot = EquipmentSlot.Legs;
                if (attackChance >= 70)
                {
                    attack = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (armorChance >= 0)
                {
                    armor = 2 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (healthChance >= 25)
                {
                    health = 5 + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (magicChance >= 70)
                {
                    magic = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }

            }
            else if (chance == 4)
            {
                if (iconChance == 1)
                {
                    newItem.icon = wep1;
                }
                else if (iconChance == 2)
                {
                    newItem.icon = wep2;
                }
                else
                {
                    newItem.icon = wep3;
                }
                newItem.equipSlot = EquipmentSlot.Weapons;
                if (attackChance >= 0)
                {
                    attack = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
                if (magicChance >= 70)
                {
                    magic = level + level * qualityChance;
                    qualityChance = Random.Range(0, 3);
                }
            }
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = newItem.icon;
            renderer.size = new Vector2(1,1);
            newItem.cost = (armor + attack + health + magic) * 2;
            newItem.init(armor, attack, health, magic);
            item = newItem;
        }
    }

    public override void Interact()
    {
        Player = FindObjectOfType<Player>();
        if (SceneManager.GetActiveScene().name == "Tavern")
        {
            Debug.Log("In Tavern");
            if (Player.money >= item.cost)
            {
                Debug.Log("SHould be buying");
                base.Interact();

                PickUp();
                Player.money -= item.cost;
                Player.moneyCount.text = "x " + Player.money;
            }
            
        }
        else
        {
            base.Interact();

            PickUp();
        }
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

    private void Update()
    {
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
            player = Player.transform;
            knight = Player;
        }
        else if (!hasInit)
        {
            initialize();
            hasInit = true;
            descriptionText.text = item.description;
        }
        else
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius && !canvas.activeSelf && SceneManager.GetActiveScene().name == "Tavern") {
                canvas.SetActive(!canvas.activeSelf);
            }
            if (distance <= radius && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                //canInteract = true;
            }
            else if (distance > radius && canvas.activeSelf)
            {
                //canInteract = false;
                canvas.SetActive(!canvas.activeSelf);
            }
        }

    }

}
