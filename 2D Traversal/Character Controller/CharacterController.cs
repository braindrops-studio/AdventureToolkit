using Braindrops.AdventureToolkit.Traversal.CharacterAnimation;
using BrainDrops.Unolith.Inputs;
using Braindrops.Unolith.ServiceLocator;
using UnityEngine;

namespace Braindrops.AdventureToolkit.Traversal.Controls
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Controller ")]
        [SerializeField] private float moveSpeed = 5f;
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
        
        private Vector3 characterScale;
        private float characterScaleX;

        private Rigidbody2D rb;

        // private bool isJumping;
        private bool isGrounded;
        private bool isWalled;

        private float defaultMoveSpeed;
        private InputService inputService;

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

            HandleCharacterFlip();

            var feetPosition = groundCheck.position;
            isGrounded = Physics2D.OverlapCircle(feetPosition, checkRadius, groundLayer);
            isWalled = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);
            var hit = Physics2D.Raycast(feetPosition, Vector2.down, checkRadius, groundLayer);
            if (hit && horizontalInput != 0)
            {
                var tangent = Mathf.Sign(horizontalInput) * new Vector2(hit.normal.y, -hit.normal.x);
                rb.velocity = tangent * moveSpeed + (animationHandler.IsJumping ? Vector2.up * rb.velocity.y : Vector2.zero);
            }
            
            if (inputService.IsPressingJump && isGrounded && !isWalled)
            {
                rb.velocity += new Vector2(0f, jumpForce);
                animationHandler.Jump();
            }
            
            if (!animationHandler.IsJumping && isGrounded && horizontalInput == 0)
                StickToGround();
            else
                rb.gravityScale = 1;

            if (animationHandler.IsJumping)
                return;
            if (isWalled && rb.velocity.x > 0)
            {
                animationHandler.Idle();
            }
            else if (isGrounded)
            {
                animationHandler.Move(rb.velocity.magnitude);
            }
            else
            {
                animationHandler.Move(Mathf.Abs(rb.velocity.x));
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
            }
        }

        public void ResetMoveSpeed()
        {
            moveSpeed = defaultMoveSpeed;
        }
    }
}