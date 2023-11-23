using System;
using System.Linq;
using Braindrops.Unolith.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Braindrops.AdventureToolkit.Colliders
{
    [RequireComponent(typeof(Collider2D))]
    public class CustomTrigger2D : MonoBehaviour
    {
        [SerializeField] private bool isTrigger = true;
        [TagSelector] public string[] tags;
        
        public UnityEvent<Collider2D> onTriggerEnter;
        public UnityEvent<Collider2D> onTriggerStay;
        public UnityEvent<Collider2D> onTriggerExit;

        private Collider2D col;

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            col.isTrigger = isTrigger;
        }

        public void DisableCollider()
        {
            if (col == null)
                return;

            col.enabled = false;
        }

        public void EnableCollider()
        {
            if (col == null)
                return;

            col.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsValidTag(other.tag))
                onTriggerEnter.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (IsValidTag(other.tag))
                onTriggerStay.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsValidTag(other.tag))
                onTriggerExit.Invoke(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (IsValidTag(other.collider.tag))
                onTriggerEnter.Invoke(other.collider);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (IsValidTag(other.collider.tag))
                onTriggerStay.Invoke(other.collider);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (IsValidTag(other.collider.tag))
                onTriggerExit.Invoke(other.collider);
        }

        private bool IsValidTag(string tag)
        {
            return tags.Any(t => t == tag);
        }
    }
}