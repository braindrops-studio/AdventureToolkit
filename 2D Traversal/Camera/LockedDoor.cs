using BrainDrops.Cycas.Library;
using BrainDrops.Unolith.Inventory;
using BrainDrops.Unolith.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace BrainDrops.Unolith.Interactions
{
    public abstract class LockedDoor : DropItemReactioner
    {
        [SerializeField] private string neededItem;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private Collider2D doorCollider;
        [SerializeField] private UnityEvent onOpenDoor;
        
        public override bool HandleDrop(string itemName)
        {
            if (itemName != neededItem) 
                return false;
            if (PlayerInventory.Instance.GetItemAmount(itemName) <= 0)
                return false;
            PlayerInventory.Instance.ChangeItemAmount(itemName, 1);
            OpenDoor();
            return true;
        }

        private void OpenDoor()
        {
            doorAnimator.Play(AnimationNames.DoorOpen);
            StartCoroutine(AsyncHelper.WaitAndDoCouroutine(
                function: () => doorCollider.enabled = false,
                delayInSeconds: AnimationHelper.GetAnimationClipLength(doorAnimator, AnimationNames.DoorOpen)
            ));
            // TODO: Save in the save system
            onOpenDoor.Invoke();
        }
    }
}