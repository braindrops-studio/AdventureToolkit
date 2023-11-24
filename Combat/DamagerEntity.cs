using UnityEngine;

namespace Braindrops.AdventureToolkit.Combat
{
    public class DamagerEntity : MonoBehaviour, Damager
    {
        [Header("Properties")]
        [SerializeField] private float health;
        
        public virtual void TakeDamage(float amount)
        {
            // TODO: Take damage animation
            health -= amount;
            if (health <= 0)
                Die();
        }

        protected virtual void Die()
        {
            // TODO: Die animation
        }
    }
}