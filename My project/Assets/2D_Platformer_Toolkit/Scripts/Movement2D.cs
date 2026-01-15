using UnityEditor;
using UnityEngine;

namespace Peykarimeh.PlatformerToolkit
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent (typeof(Attack2D))]
    public class Movement2D : MonoBehaviour
    {
        [SerializeField] public float testval = 1f;
        public GUIStyle mainHeaderStyle = new GUIStyle();
        Animator animator;
        [SerializeField] Transform spriteTransform;
        [SerializeField] Rigidbody2D rb2;
        [SerializeField] CapsuleCollider2D capsuleCollider;
        Vector2 input;
        [HideInInspector] public float currentHorizontalSpeed;
        [HideInInspector] public float currentVerticalSpeed;
        [HideInInspector] public PlayerStates currentState;

        [Space]
        //[Header("SPEED VALUES")]
        [Range(1, 10)]
        [SerializeField] float movementSpeed = 5f;
        [Range(0.02f, 1)]
        [SerializeField] float speedUpDuration = 0.1f;
        [Range(0.02f, 1)]
        [SerializeField] float speedDownDuration = 0.06f;
        [Range(0.02f, 1)]
        [SerializeField] float stopDuration = 0.15f;
        [SerializeField] bool canJump2;

        //[Header("DASH")]
        [SerializeField] bool Dash;
        [SerializeField] KeyCode dashButton = KeyCode.LeftShift;
        [SerializeField] bool cancelDashOnWallHit;
        //[Header("-Dash Values")]
        [Range(1f, 10f)]
        [SerializeField] float dashDistance = 3f;
        [Range(0.1f, 10)]
        [SerializeField] float dashDuration = 0.3f;
        [Range(0f, 1)]
        [SerializeField] float dashStopEffect = 0.5f;

        //[Header("-Dash Settings")]
        [SerializeField] bool resetDashOnGround;
        [SerializeField] bool resetDashOnWall;
        [SerializeField] bool airDash;
        [SerializeField] bool dashCancelsGravity;
        [SerializeField] bool verticalDash;
        [SerializeField] bool horizontalDash;
        [SerializeField] Vector2 dashColliderScale;
        [SerializeField] Vector2 dashColliderOffset;
        [Space]
        [SerializeField] float dashCooldown = 0.5f;
        [SerializeField] float dashCoolTimer;

        bool canDash;
        Vector2 defaultColliderOffset;
        Vector2 defaulColliderSize;


        //DASH INFO
        [HideInInspector] public bool isDashing;
        float dashSpeed;
        float dashingTimer;

        [Space]
        //[Header("WALL JUMP")]
        public bool WallJump;
        [SerializeField] Vector2 wallJumpVelocity;
        [Range(0.02f, 1f)]
        [SerializeField] float wallJumpDecelerationFactor = 0.3f;
        [Range(0.01f, 10f)]
        [SerializeField] float wallSlideSpeed = 0.5f;
        [SerializeField] bool isSlidingOnWall;
        [SerializeField] bool variableJumpHeightOnWallJump;

        [SerializeField] float inputDelay = 0.2f;
        [SerializeField] float inputDelayTimer;


        [Space]
        //[Header("----Jumping Values----")]
        [Range(0.5f, 10f)]
        [SerializeField] float jumpHight = 1.5f;
        [Range(0.5f, 50f)]
        [SerializeField] float jumpUpAcceleration = 2.5f;
        [Range(0.5f, 50f)]
        [SerializeField] float jumpDownAcceleration = 4f;
        [Range(0.1f, 50f)]
        [SerializeField] float fallSpeedClamp = 50;
        [Range(1f, 20f)]
        [SerializeField] float gravity = 9.8f;
        [SerializeField] float jumpVelocity;
        [SerializeField] float fallClamp;
        [SerializeField] float jumpUpDuration;
        [SerializeField] bool isNormalJumped;
        [SerializeField] bool isWallJumped;

        [Space]
        //[Header("----Jump Adjustments----")]
        [Range(0f, 1f)]
        [SerializeField] float coyoteTime = 0.15f;
        [Range(0f, 1f)]
        [SerializeField] float jumpBuffer = 0.1f;
        [Range(0f, 1f)]
        [SerializeField] float onAirControl = 1f;

        [Range(0f, 1f)]
        [SerializeField] float variableJumpHeightDuration = 0.75f;
        [Range(0f, 1f)]
        [SerializeField] float jumpReleaseEffect = 0.5f;
        [SerializeField] KeyCode jumpButton = KeyCode.Space;
        bool isHoldingJumpButton;
        float jumpHoldTimer;
        bool isForcingJump;

        //[Header("Ledge Climb")]
        [SerializeField] bool LedgeGrab;
        [SerializeField] bool autoClimbLedge;
        [SerializeField] KeyCode climbButton = KeyCode.W;
        [SerializeField] bool canWallJumpWhileClimbing;
        [SerializeField] float ledgeCheckOffset = 1f;
        [SerializeField] float ledgeCheckDistance = 1f;
        [SerializeField] LayerMask ledgeCheckLayer;
        public bool isLedge;
        Vector2 ledgePosition;
        [SerializeField] Vector2 ledgeClimbPosOffset;
        public bool isClimbingLedge;
        [SerializeField] float ledgeClimbDuration;
        [SerializeField] float ledgeClimbTimer;

        [Space]
        //[Header("----GroundCheck----")]
        [SerializeField] Vector2 groundCheckCenter;
        [SerializeField] float groundCheckRayDistance = 1f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckCircleRadius = 0.5f;

        [Space]
        //[Header("----CeilCheck----")]
        [SerializeField] Vector2 ceilCheckCenter;
        [SerializeField] float ceilCheckRayDistance = 0.1f;
        [SerializeField] LayerMask ceilLayer;
        [SerializeField] float ceilCheckCircleRadius = 0.5f;

        [Space]
        //[Header("----WallCheck----")]
        [SerializeField] float wallCheckRayDistance;
        [SerializeField] LayerMask wallCheckLayer;

        [Space]
        //[Header("JUPM DEBUG")]
        [SerializeField] float jumpToleranceTimer;
        [SerializeField] float fallToleranceTimer;
        public bool isGrounded;
        [SerializeField] bool onCeil;
        public bool canJump;

        [Space]

        //[Header("*****DEBUG*****")]
        public bool leftWallHit;
        public bool rightWallHit;
        public bool hitWall;
        public bool isJumped;//to check if the player is on air because of jumping or falling
        public bool isPressedJumpButton;

        float onAirControlMultiplier;



        private void Reset()
        {
            rb2 = GetComponent<Rigidbody2D>();
            rb2.freezeRotation = true;
            rb2.gravityScale = 0;
            rb2.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            capsuleCollider = GetComponent<CapsuleCollider2D>();

            if (transform.childCount == 0)
            {
                GameObject _sprite = new GameObject();
                _sprite.name = "Sprite";
                _sprite.transform.SetParent(transform);
                _sprite.transform.localPosition = Vector3.zero;
                spriteTransform = _sprite.transform;
                _sprite.AddComponent<SpriteRenderer>();

            }
            GetAutoValueForCeilCheck();
            GetAutoValueForGroundCheck();
        }

        private void Start()
        {
            fallClamp = fallSpeedClamp;
        }

        public enum PlayerStates
        {
            None,
            Grounded,
            Jumping,
            Falling
        }

        void SwitchState()
        {
            PlayerStates newState;
            if (isGrounded)
            {
                newState = PlayerStates.Grounded;
            }
            else
            {

                if (currentVerticalSpeed >= 0)
                {
                    newState = PlayerStates.Jumping;
                }
                else
                {
                    newState = PlayerStates.Falling;
                }
            }
            if (newState != currentState)
            {
                currentState = newState;
            }
        }

        private void Awake()
        {
            rb2 = GetComponent<Rigidbody2D>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
            defaulColliderSize = capsuleCollider.size;
            defaultColliderOffset = capsuleCollider.offset;
            animator = transform.GetChild(0).GetComponent<Animator>();
        }
        private void Update()
        {
            HandlePlatformerMovement();
        }

        private void FixedUpdate()
        {

            UpdatePlatformerSpeed();
            MovePlayer();
            LedgeClimbCountdown();

        }

        void HandlePlatformerMovement()
        {
            GetPlatformerInput();
            DelayInputOnWall();
            CheckSideWall();
            DoDash();
            CheckCeil();
            CheckGround();
            FlipThePlayer();
            CountDownJumpTolerance();
            SwitchState();
            CheckLedge();
            DashCooldownCounter();

        }
        void DoDash()
        {
            if (isDashing)
            {
                dashingTimer -= Time.deltaTime;
                if (dashingTimer <= 0 && (isGrounded && !onCeil || !isGrounded))
                {
                    CancelDash();

                }
            }
        }
        void DashCooldownCounter()
        {
            if (dashCoolTimer > 0)
            {
                dashCoolTimer -= Time.fixedDeltaTime;
            }
        }
        void ClimbLedge()
        {
            //transform.GetChild(0).GetComponent<Animator>().Play("ProtoLedgeClimb");

            isClimbingLedge = true;
            animator.SetBool("ClimbLedge", isClimbingLedge);
            ledgeClimbTimer = ledgeClimbDuration;

        }
        void LedgeClimbCountdown()
        {
            if (isClimbingLedge)
            {
                ledgeClimbTimer -= Time.deltaTime;
                if (ledgeClimbTimer <= 0f)
                {
                    UpdateLedgeClimbPosition();
                }
            }
        }
        public void UpdateLedgeClimbPosition()
        {
            isClimbingLedge = false;
            //transform.GetChild(0).GetComponent<Animator>().Play("ProtoIdle");
            transform.position = spriteTransform.position;
            Vector2 _posOffset = new(spriteTransform.right.x * ledgeClimbPosOffset.x,
                spriteTransform.up.y * ledgeClimbPosOffset.y);
            animator.Play("ProtoIdle");
            //animator.SetBool("ClimbLedge", isClimbingLedge);
            transform.position = (Vector2)transform.position + _posOffset;


        }

        void CheckLedge()
        {
            if (LedgeGrab)
            {
                //CHECH THE LEDGE
                Vector2 _ledgeCheckOrigin = new(transform.position.x, transform.position.y + ledgeCheckOffset);
                RaycastHit2D _HitLedge = Physics2D.Raycast(_ledgeCheckOrigin, spriteTransform.right, ledgeCheckDistance, ledgeCheckLayer);
                bool _canGrabLedge = !_HitLedge && hitWall;


                if (_canGrabLedge)
                {
                    //GET LEDGE POSITION
                    Vector2 _ledgeGroundCheckOrigin = _ledgeCheckOrigin + (Vector2)(spriteTransform.right * ledgeCheckDistance);
                    RaycastHit2D _hit = Physics2D.Raycast(_ledgeGroundCheckOrigin, Vector2.down, ledgeCheckDistance, ledgeCheckLayer);

                    if (_hit)
                    {

                        if (!isLedge)
                        {
                            ledgePosition = (Vector2)transform.position - new Vector2(0f, _hit.distance - 0.1f);
                            isLedge = true;
                            currentVerticalSpeed = 0f;
                            fallClamp = 0f;
                            rb2.MovePosition(ledgePosition);
                            if (autoClimbLedge)
                            {
                                ClimbLedge();

                            }
                        }
                        if (Input.GetKeyDown(climbButton) && !autoClimbLedge)
                        {
                            ClimbLedge();
                        }

                    }

                }

                else if (isLedge && !_canGrabLedge)
                {
                    isLedge = false;
                    isClimbingLedge = false;
                    fallClamp = fallSpeedClamp;
                }

            }



        }

        void CancelDash()
        {
            if (isDashing)
            {
                isDashing = false;
                dashCoolTimer = dashCooldown;
                currentHorizontalSpeed *= dashStopEffect;
                currentVerticalSpeed *= dashStopEffect;

                capsuleCollider.offset = defaultColliderOffset;
                capsuleCollider.size = defaulColliderSize;
            }
        }

        void DashPressed()
        {
            if (canDash && !isDashing && Dash && (horizontalDash && input.x != 0 || verticalDash && input.y != 0) && dashCoolTimer <= 0f)
            {

                if (!isGrounded)
                {
                    canDash = false;

                }
                isDashing = true;
                dashSpeed = dashDistance / dashDuration;
                dashingTimer = dashDuration;

                Vector2 _dir = Vector2.zero;

                if (horizontalDash && !verticalDash)
                {
                    _dir.x = input.x;
                    currentHorizontalSpeed = dashSpeed * _dir.x;
                    currentVerticalSpeed = 0f;


                }
                else if (verticalDash && !horizontalDash)
                {
                    _dir.y = input.y;

                    currentHorizontalSpeed = dashSpeed * _dir.x;
                    currentVerticalSpeed = dashSpeed * _dir.y;
                }
                else if (horizontalDash && verticalDash)
                {
                    _dir = input.normalized;

                    currentHorizontalSpeed = dashSpeed * _dir.x;
                    currentVerticalSpeed = dashSpeed * _dir.y;
                }


                if (isGrounded)
                {
                    capsuleCollider.offset = dashColliderOffset;
                    capsuleCollider.size = dashColliderScale;

                }


            }
        }

        public void GetColliderSize()
        {
            dashColliderScale = capsuleCollider.size;
            dashColliderOffset = capsuleCollider.offset;
            print("Values Saved");
        }

        void DelayInputOnWall()
        {
            if (isSlidingOnWall)
            {
                inputDelayTimer = input.x == 0 ? inputDelay : inputDelayTimer - Time.deltaTime;
                if (inputDelayTimer > 0f) input.x = 0;
            }

        }

        void GetPlatformerInput()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(jumpButton))
            {
                PressJumpButton();

            }
            if (Input.GetKeyUp(jumpButton))
            {
                isHoldingJumpButton = false;
            }
            if (Input.GetKeyDown(dashButton))
            {
                DashPressed();

            }
            Jump();
        }


        private void OnDrawGizmos()
        {
            if (isGrounded)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckCenter - (Vector2)transform.up * groundCheckRayDistance, groundCheckCircleRadius);

            if (onCeil)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere((Vector2)transform.position + ceilCheckCenter + (Vector2)transform.up * ceilCheckRayDistance, ceilCheckCircleRadius);


            if (rightWallHit)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawRay(transform.position, transform.right * wallCheckRayDistance);

            if (leftWallHit)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawRay(transform.position, -transform.right * wallCheckRayDistance);
            Vector2 _ledgePos;
            if (isLedge)
            {
                Gizmos.color = Color.green;
                _ledgePos = new Vector2(transform.position.x, transform.position.y + ledgeCheckOffset);
                Vector2 _ledgeGroundCheckCenter = _ledgePos + (Vector2)(spriteTransform.right * ledgeCheckDistance);

                Gizmos.DrawRay(_ledgeGroundCheckCenter, Vector2.down * ledgeCheckDistance);
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.color = Color.blue;

            _ledgePos = new Vector2(transform.position.x, transform.position.y + ledgeCheckOffset);
            //Gizmos.DrawRay(_ledgePos, spriteTransform.right * ledgeCheckDistance);


        }

        void CheckCeil()
        {
            RaycastHit2D hit2D = Physics2D.CircleCast((Vector2)transform.position + ceilCheckCenter, ceilCheckCircleRadius, transform.up, ceilCheckRayDistance, ceilLayer);

            if (hit2D)
            {
                if (!onCeil)
                {
                    onCeil = true;
                    currentVerticalSpeed = 0;
                }
            }
            else
            {
                onCeil = false;
            }
        }

        public void GetAutoValueForCeilCheck()
        {
            if (capsuleCollider == null)
            {
                capsuleCollider = GetComponent<CapsuleCollider2D>();
            }
#if UNITY_EDITOR
            Undo.RecordObject(this, "Set Value For Ceil Check");
#endif
            ceilCheckCenter = capsuleCollider.offset;
            ceilCheckCircleRadius = capsuleCollider.size.x / 2f;
            ceilCheckRayDistance = capsuleCollider.size.y / 2f - ceilCheckCircleRadius + 0.02f;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        void CheckSideWall()
        {
            rightWallHit = Physics2D.Raycast(transform.position, transform.right, wallCheckRayDistance, wallCheckLayer);
            leftWallHit = Physics2D.Raycast(transform.position, -transform.right, wallCheckRayDistance, wallCheckLayer);

            if (rightWallHit || leftWallHit)
            {
                if (!hitWall)
                {
                    if (resetDashOnWall)
                    {
                        canDash = true;
                    }
                    hitWall = true;
                    currentHorizontalSpeed = 0;

                }

                if (WallJump && !isGrounded)
                {
                    SlideOnWall(true);

                }

                if (cancelDashOnWallHit)
                {
                    CancelDash();
                }
            }
            else
            {
                if (WallJump)
                {
                    SlideOnWall(false);

                    //fallSpeedClamp /= wallSlideSpeedEffect;
                }
                hitWall = false;
            }

        }

        void SlideOnWall(bool _sliding)
        {
            if (!isSlidingOnWall && _sliding)
            {
                isSlidingOnWall = true;
                isNormalJumped = false;
                isWallJumped = false;
                fallClamp = wallSlideSpeed;
            }
            else if (isSlidingOnWall && !_sliding)
            {
                isSlidingOnWall = false;
                fallClamp = fallSpeedClamp;
            }
        }


        void CheckGround()
        {
            RaycastHit2D hit2D = Physics2D.CircleCast((Vector2)transform.position + groundCheckCenter, groundCheckCircleRadius, -transform.up, groundCheckRayDistance, groundLayer);
            if (hit2D)
            {

                if (!isGrounded)
                {

                    isGrounded = true;
                    isWallJumped = false;
                    isNormalJumped = false;
                    fallClamp = fallSpeedClamp;
                    canJump = true;
                    canDash = resetDashOnGround;
                    onAirControlMultiplier = 1;
                    SlideOnWall(false);

                    if (currentVerticalSpeed <= 0)//to check if the player if grounded while falling
                    {
                        currentVerticalSpeed = 0f;
                        isJumped = false;
                    }
                }
                if (resetDashOnGround)
                {
                    canDash = true;
                }

            }
            else
            {
                if (isGrounded)
                {
                    onAirControlMultiplier = onAirControl;
                    isGrounded = false;


                    if (!isJumped)
                    {
                        fallToleranceTimer = coyoteTime;
                        if (!dashCancelsGravity)
                        {
                            CancelDash();
                        }
                    }
                }


            }

            if (!isGrounded)
            {

                fallToleranceTimer -= Time.deltaTime;
                if (fallToleranceTimer <= 0)
                {
                    if (!airDash)
                    {
                        canDash = false;
                    }
                    canJump = false;
                }
            }
        }

        public void GetAutoValueForGroundCheck()
        {
            if (capsuleCollider == null)
            {
                capsuleCollider = GetComponent<CapsuleCollider2D>();
            }
#if UNITY_EDITOR
            Undo.RecordObject(this, "Set Value For Ground Check");
#endif
            groundCheckCenter = capsuleCollider.offset;
            groundCheckCircleRadius = capsuleCollider.size.x / 2f;
            groundCheckRayDistance = capsuleCollider.size.y / 2f - groundCheckCircleRadius + 0.02f;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        void PressJumpButton()
        {
            if (!isDashing)
            {
                isHoldingJumpButton = true;
                isPressedJumpButton = true;
                isForcingJump = true;
                jumpHoldTimer = variableJumpHeightDuration * jumpUpDuration;
                jumpToleranceTimer = jumpBuffer;
            }
        }
        void Jump()
        {
            if (isForcingJump)
            {
                jumpHoldTimer -= Time.deltaTime;
                if (jumpHoldTimer <= 0)
                {
                    isForcingJump = false;
                }
                if (!isHoldingJumpButton && isForcingJump && (variableJumpHeightOnWallJump && isWallJumped || isNormalJumped))
                {
                    currentVerticalSpeed *= jumpReleaseEffect;
                    isForcingJump = false;
                }

            }

            if (canJump && isPressedJumpButton)
            {
                jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
                jumpUpDuration = jumpVelocity / (jumpUpAcceleration * gravity);

                isJumped = true;
                canJump = false;
                isPressedJumpButton = false;
                currentVerticalSpeed = jumpVelocity;
                isNormalJumped = true;
            }
            if (isPressedJumpButton && isSlidingOnWall && (!canWallJumpWhileClimbing && !isClimbingLedge || canWallJumpWhileClimbing))
            {
                jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
                isJumped = true;
                canJump = false;
                isWallJumped = true;
                isPressedJumpButton = false;
                onAirControlMultiplier = wallJumpDecelerationFactor;
                currentVerticalSpeed = jumpVelocity * wallJumpVelocity.y;
                if (leftWallHit)
                {
                    currentHorizontalSpeed = jumpVelocity * wallJumpVelocity.x;
                }
                else if (rightWallHit)
                {
                    currentHorizontalSpeed = -jumpVelocity * wallJumpVelocity.x;
                }
            }
        }

        void CountDownJumpTolerance()
        {
            if (isPressedJumpButton)
            {
                jumpToleranceTimer -= Time.deltaTime;
                if (jumpToleranceTimer <= 0)
                {
                    isPressedJumpButton = false;
                }
            }
        }

        void UpdatePlatformerSpeed()
        {
            if (!isDashing)
            {
                if (input.x != 0)
                {
                    if (input.x * currentHorizontalSpeed >= 0)
                    {
                        if (input.x > 0 && !rightWallHit || input.x < 0 && !leftWallHit)
                        {
                            currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, Time.deltaTime / speedUpDuration * onAirControlMultiplier * movementSpeed);// /(xDist)
                        }
                    }
                    else
                    {
                        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime / speedDownDuration * onAirControlMultiplier * movementSpeed);
                    }
                }
                else
                {
                    currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, Time.deltaTime / stopDuration * onAirControlMultiplier * movementSpeed);
                }
            }


            if (!isGrounded && (!isDashing || !dashCancelsGravity && isDashing))
            {
                if (currentVerticalSpeed >= 0)
                {
                    currentVerticalSpeed -= jumpUpAcceleration * Time.fixedDeltaTime * gravity;
                }
                else if (currentVerticalSpeed < 0)
                {
                    currentVerticalSpeed -= jumpDownAcceleration * Time.fixedDeltaTime * gravity;
                }
                currentVerticalSpeed = Mathf.Clamp(currentVerticalSpeed, -fallClamp, 100f);
            }
            else if (isGrounded)
            {
                if (currentVerticalSpeed < 0)
                {
                    currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, 0, jumpDownAcceleration * jumpVelocity * 3 * Time.fixedDeltaTime);
                }
            }
        }

        void FlipThePlayer()
        {
            Vector3 _playerRot = spriteTransform.localEulerAngles;

            if (!isSlidingOnWall && currentHorizontalSpeed > 0 || WallJump && isSlidingOnWall && rightWallHit)
            {
                _playerRot.y = 0f;
                spriteTransform.localEulerAngles = _playerRot;
            }
            else if (!isSlidingOnWall && currentHorizontalSpeed < 0 || WallJump && isSlidingOnWall && leftWallHit)
            {
                _playerRot.y = 180f;
                spriteTransform.localEulerAngles = _playerRot;
            }
        }

        void MovePlayer()
        {
            rb2.linearVelocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed);
        }
    }
}