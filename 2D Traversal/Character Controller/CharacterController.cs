using System;
using Braindrops.AdventureToolkit.Traversal.CharacterAnimation;
using Braindrops.Unolith.Inputs;
using Braindrops.Unolith.ServiceLocator;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.Controls
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Controller ")]
        [SerializeField] private Vector2 moveSpeed = new Vector2(3, 1);
        [SerializeField] private float jumpForce = 5f;
        
        [Header("Animation")]
        [SerializeField] private CharacterAnimationProvider animationHandler;
        
        [Header("Collision Checks")]
        [SerializeField] private float checkRadius;
        
        [Space(15)]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;
        
        [Space(15)]
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform wallCheck;

        [Space(15)]
        [SerializeField] private float stickToGroundSpeed;

        private Vector3 characterScale;
        private float characterScaleX;

        private Rigidbody2D rb;

        private bool isGrounded;
        private bool isWalled;
        private bool isStuck;
        private bool isInAir;

        private Vector2 defaultMoveSpeed;
        private InputService inputService;

        public void SetIsInAir(bool value) => isInAir = value;

        public Transform GroundCheck => groundCheck;

        private void Awake()
        {
            inputService = ServiceLocator.Instance.GetService<InputService>();
            rb = GetComponent<Rigidbody2D>();
            
            defaultMoveSpeed = moveSpeed;
            characterScale = transform.localScale;
            characterScaleX = characterScale.x;            
        }

        private void Update()
        {
            var horizontalInput = inputService.HorizontalInput;
            var verticalInput = inputService.VerticalInput;
            
            isWalled = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);

            if (isInAir)
            {
                rb.gravityScale = 1;
            }
            else
            {
                var hitTop = Physics2D.Raycast(rb.position, Vector2.up, 10, groundLayer);
                var hitBottom = Physics2D.Raycast(rb.position, Vector2.down, 10, groundLayer);
                var distanceTop = Vector2.Distance(rb.position + Vector2.up * .05f, hitTop.point);
                var distanceBottom = Vector2.Distance(rb.position - Vector2.up * .05f, hitBottom.point);
                if (verticalInput > 0)
                    verticalInput *= distanceTop / (distanceBottom + distanceTop);
                else
                    verticalInput *= distanceBottom / (distanceBottom + distanceTop);
                rb.velocity = new Vector2(horizontalInput * moveSpeed.x, verticalInput * moveSpeed.y);
            }

            HandleCharacterFlip();

            if (animationHandler.IsJumping)
                return;
            if (isWalled && Mathf.Abs(rb.velocity.x) > 0)
            {
                animationHandler.Idle();
            }
            else if (isGrounded)
            {
                animationHandler.Move(rb.velocity.magnitude);
            }
            else
            {
                animationHandler.Move(Mathf.Abs(rb.velocity.magnitude));
            }

            return;

            void HandleCharacterFlip()
            {
                characterScale.x = horizontalInput switch
                {
                    < 0 => -characterScaleX,
                    > 0 => characterScaleX,
                    _ => characterScale.x
                };

                transform.localScale = characterScale;
            }

            void StickToGround()
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
                // if (isStuck)
                    // return;
                if (hit)
                {
                    Debug.DrawLine(transform.position, hit.point);
                    if (Vector3.Distance(transform.position, new Vector3(hit.point.x, hit.point.y, transform.position.z)) <= 0.1f)
                        isStuck = true;
                    if (!isStuck)
                    {
                       transform.position = Vector3.Lerp(transform.position, new Vector3(hit.point.x, hit.point.y, transform.position.z), stickToGroundSpeed * 5 * Time.deltaTime);
                    }
                }
            }
        }

        public void ResetMoveSpeed()
        {
            moveSpeed = defaultMoveSpeed;
            animationHandler.ResetAnimationSpeed();
        }

        public void MultiplyMoveSpeed(float amount, bool changeAnimationSpeed = true)
        {
            moveSpeed *= amount;
            if (changeAnimationSpeed)
                animationHandler.ChangeAnimationSpeed(amount);
        }
    }
}