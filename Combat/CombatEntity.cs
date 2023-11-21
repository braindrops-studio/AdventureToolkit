using Braindrops.AdventureToolkit.Traversal.CharacterAnimation;
using Braindrops.Unolith.Inputs;
using Braindrops.Unolith.ServiceLocator;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Combat
{
    public class CombatEntity : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float attackCoolDown = 2f;
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float attackPower = 100f;
        [SerializeField] private Transform attackStartPoint;
        
        [Header("Animation")]
        [SerializeField] private CharacterAnimationProvider animationHandler;

        private InputService inputService;
        private float lastAttackTime;
        
        private void Awake()
        {
            inputService = ServiceLocator.Instance.GetService<InputService>();
        }

        private void Update()
        {
            if (CanAttack() && inputService.IsPressingAttack)
                Attack();

            return;

            bool CanAttack()
            {
                return Time.time - lastAttackTime > attackCoolDown;
            }
        }

        public void Attack()
        {
            animationHandler.Attack();
            lastAttackTime = Time.time;

            var hits = Physics2D.RaycastAll(attackStartPoint.position, Vector2.right, attackRange);

            foreach (var hit in hits)
            {
                var damager = hit.collider.gameObject.GetComponent<Damager>();
                damager?.TakeDamage(attackPower);
            }
        }
    }
}