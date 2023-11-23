using Braindrops.AdventureToolkit.Colliders;
using Braindrops.Unolith.Inputs;
using Braindrops.Unolith.ServiceLocator;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.FourWayPath
{
    public class FourWayPath : MonoBehaviour
    {
        [Header("Colliders")]
        [SerializeField] private CustomTrigger2D topLeftCollider;
        [SerializeField] private CustomTrigger2D topRightCollider;
        [SerializeField] private CustomTrigger2D bottomRightCollider;
        [SerializeField] private CustomTrigger2D bottomLeftCollider;

        [Header("Modifiers")]
        [SerializeField] [Range(0f, .5f)] private float bottomRightTendency = 0f;
        [SerializeField] [Range(0f, .5f)] private float bottomLeftTendency = 0f;

        [Header("Boundary")]
        [SerializeField] private CustomTrigger2D boundary;

        public CustomTrigger2D currentCollider;
        private bool hasExitedCurrentCollider = true;
        private CustomTrigger2D targetCollider;
        private InputService inputService;

        private void Awake()
        {
            inputService = ServiceLocator.Instance.GetService<InputService>();
        }

        private void Start()
        {
            if (boundary != null)
                boundary.onTriggerExit.AddListener((_) => ResetAllColliders());
                
            topLeftCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(topLeftCollider));
            topRightCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(topRightCollider));
            bottomLeftCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(bottomLeftCollider));
            bottomRightCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(bottomRightCollider));
            
            topLeftCollider.onTriggerStay.AddListener((_) => HandleColliderEnter(topLeftCollider));
            topRightCollider.onTriggerStay.AddListener((_) => HandleColliderEnter(topRightCollider));
            bottomLeftCollider.onTriggerStay.AddListener((_) => HandleColliderEnter(bottomLeftCollider));
            bottomRightCollider.onTriggerStay.AddListener((_) => HandleColliderEnter(bottomRightCollider));

            topLeftCollider.onTriggerExit.AddListener((_) => HandleColliderExit(topLeftCollider));
            topRightCollider.onTriggerExit.AddListener((_) => HandleColliderExit(topRightCollider));
            bottomLeftCollider.onTriggerExit.AddListener((_) => HandleColliderExit(bottomLeftCollider));
            bottomRightCollider.onTriggerExit.AddListener((_) => HandleColliderExit(bottomRightCollider));
        }

        private void Update()
        {
            if (currentCollider == topLeftCollider)
            {
                if (topRightCollider == bottomRightCollider)
                    return;

                if (inputService.VerticalInput < 0 + bottomRightTendency)
                {
                    targetCollider = bottomRightCollider;
                    topRightCollider.DisableCollider();
                }
                else
                {
                    topRightCollider.EnableCollider();
                    targetCollider = topRightCollider;
                }
            } else if (currentCollider == topRightCollider)
            {
                if (topLeftCollider == bottomLeftCollider)
                    return;

                if (inputService.VerticalInput < 0 + bottomLeftTendency)
                {
                    targetCollider = bottomLeftCollider;
                    topLeftCollider.DisableCollider();
                }
                else
                {
                    topLeftCollider.EnableCollider();
                    targetCollider = topLeftCollider;
                }
            } else if (currentCollider == bottomLeftCollider)
            {
                if (topRightCollider == bottomRightCollider)
                    return;

                if (inputService.VerticalInput < 0 + bottomRightTendency)
                {
                    targetCollider = bottomRightCollider;
                    topRightCollider.DisableCollider();
                }
                else
                {
                    targetCollider = topRightCollider;
                    topRightCollider.EnableCollider();
                }
            } else if (currentCollider == bottomRightCollider)
            {
                if (topLeftCollider == bottomLeftCollider)
                    return;

                if (inputService.VerticalInput < 0 + bottomLeftTendency)
                {
                    targetCollider = bottomLeftCollider;
                    topLeftCollider.DisableCollider();
                }
                else
                {
                    topLeftCollider.EnableCollider();
                    targetCollider = topLeftCollider;
                }
            }
        }

        private void HandleColliderEnter(CustomTrigger2D customTrigger)
        {
            if (currentCollider == null)
            {
                currentCollider = customTrigger;
                hasExitedCurrentCollider = false;
            }
            else if (customTrigger == targetCollider)
            {
                currentCollider = customTrigger;
                targetCollider = null;
                hasExitedCurrentCollider = false;
            }
        }

        private void HandleColliderExit(CustomTrigger2D customTrigger)
        {
            if (currentCollider == customTrigger)
            {
                hasExitedCurrentCollider = true;
                currentCollider = null;
            }
        }

        private void ResetAllColliders()
        {
            topLeftCollider.EnableCollider();
            topRightCollider.EnableCollider();
            bottomLeftCollider.EnableCollider();
            bottomRightCollider.EnableCollider();
        }
    }
}