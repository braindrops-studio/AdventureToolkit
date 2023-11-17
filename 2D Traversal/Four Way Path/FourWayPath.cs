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

        private CustomTrigger2D currentCollider;

        private InputService inputService;

        private void Awake()
        {
            inputService = ServiceLocator.Instance.GetService<InputService>();
        }

        private void Start()
        {
            topLeftCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(topLeftCollider));
            topRightCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(topRightCollider));
            bottomLeftCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(bottomLeftCollider));
            bottomRightCollider.onTriggerEnter.AddListener((_) => HandleColliderEnter(bottomRightCollider));
        }

        private void Update()
        {
            if (currentCollider == topLeftCollider)
            {
                if ((int)inputService.VerticalInputRaw == -1)
                    topRightCollider.DisableCollider();
                else
                    topRightCollider.EnableCollider();
            } else if (currentCollider == topRightCollider)
            {
                if ((int)inputService.VerticalInputRaw == -1)
                    topLeftCollider.DisableCollider();
                else
                    topLeftCollider.EnableCollider();                    
            } else if (currentCollider == bottomLeftCollider)
            {
                if ((int)inputService.VerticalInputRaw == -1)
                    topRightCollider.DisableCollider();
                else                    
                    topRightCollider.EnableCollider();
            } else if (currentCollider == bottomRightCollider)
            {
                if ((int)inputService.VerticalInputRaw == -1)
                    topLeftCollider.DisableCollider();
                else
                    topLeftCollider.EnableCollider();
            }
        }

        private void HandleColliderEnter(CustomTrigger2D customTrigger)
        {
            currentCollider = customTrigger;
        }
    }
}