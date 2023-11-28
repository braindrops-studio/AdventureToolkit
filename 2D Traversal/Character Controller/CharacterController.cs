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
        public bool isInAir;

        private Vector2 defaultMoveSpeed;
        private InputService inputService;

        public void SetIsInAir(bool value) => isInAir = value;

        public Transform GroundCheck => groundCheck;

        private bool isFallingScenario;

        private void Awake()
        {
            inputService = ServiceLocator.Instance.GetService<InputService>();
            rb = GetComponent<Rigidbody2D>();
            
            defaultMoveSpeed = moveSpeed;
            characterScale = transform.localScale;
            characterScaleX = characterScale.x;            
        }

        public void GoToFallingScenario()
        {
            isFallingScenario = true;
            SetIsInAir(true);
            rb.gravityScale = 1;
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            animationHandler.Jump();
        }

        public void EndFallingScenario()
        {
            isFallingScenario = false;
            isInAir = false;
            rb.gravityScale = 0;
            animationHandler.Idle();
        }

        private void Update()
        {
            var horizontalInput = inputService.HorizontalInput;
            var verticalInput = inputService.VerticalInput;
            HandleCharacterFlip();
            
            if (isFallingScenario)
            {
                if (!animationHandler.IsJumping)
                {
                    rb.gravityScale = .1f;
                    animationHandler.FreeFall();
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    rb.velocity += moveSpeed.x * .2f * new Vector2(horizontalInput, verticalInput);
                    var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
                    if (Vector3.Distance(hit.point, transform.position) < .1f)
                        EndFallingScenario();
                }
                return;
            }
            
            isWalled = Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);
            
            rb.gravityScale = isInAir ? 1 : 0;
            if (!isInAir)
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
            else
            {
                rb.velocity = new Vector2(horizontalInput * moveSpeed.x, verticalInput * moveSpeed.y);
                var hit = Physics2D.Raycast(groundCheck.position, Vector2.down, checkRadius, groundLayer);
                if (hit && horizontalInput != 0)
                {
                    var tangent = Mathf.Sign(horizontalInput) * new Vector2(hit.normal.y, -hit.normal.x);
                    rb.velocity = tangent * moveSpeed.x;
                }

                if (horizontalInput == 0)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0;
                }
            }


            if (animationHandler.IsJumping)
                return;
            if (isWalled && Mathf.Abs(rb.velocity.x) > 0 && Mathf.Abs(rb.velocity.y) == 0)
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