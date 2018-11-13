using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public Stat damage;
    public Stat armor;

    public int maxHealth;
    public int currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage) {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 5, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        //Die in some way
        Debug.Log(transform.name + " died.");
    }


}
