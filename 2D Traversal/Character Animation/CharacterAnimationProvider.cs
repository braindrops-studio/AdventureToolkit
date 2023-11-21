using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.CharacterAnimation
{
    public abstract class CharacterAnimationProvider : MonoBehaviour
    {
        public bool IsJumping { get; protected set; }

        public abstract void Jump();

        public abstract void Walk();

        public abstract void Move(float speed);

        public abstract void Idle();

        public abstract void Attack();
    }
}