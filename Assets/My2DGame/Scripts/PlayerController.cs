using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("이동 관련")]
        [SerializeField] public float walkSpeed = 3f; // 걷기 속도
        [SerializeField] public float runSpeed = 5f;  // 달리기 속도
        [SerializeField] public float airControlPercent = 0.5f; // 공중 제어 비율

        [Header("점프 관련")]
        [SerializeField] public float jumpForce = 400f; // 점프 힘
        [SerializeField] private int maxJumpCount = 2; // 최대 점프 가능 횟수

        [Header("벽 슬라이드/점프 관련")]
        [SerializeField] private float wallSlideSpeed = -2f; // 벽에서 미끄러지는 속도
        [SerializeField] private Vector2 wallJumpForce = new Vector2(5f, 7f); // 벽점프 반발력

        // 참조
        private Animator animator;
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection;

        private Vector2 inputmove = Vector2.zero;

        // 상태값
        private bool isMove = false;
        private bool isFacingRight = true;
        private bool isRun = false;
        private bool isJump = false;
        private int currentJumpCount = 0;
        private bool isAttacking = false;
        private bool isWallSliding = false;
        private bool isWallJumping = false;
        #endregion

        #region Property
        public bool IsMove
        { 
            get {return isMove;}
            private set
            {
                isMove = value;
                animator.SetBool(AnimationString.IsMove, value);
            }
        }
        public bool IsRun
        {
            get { return isRun;}
            private set
            {
                isRun = value;
                animator.SetBool(AnimationString.IsRun, value);
            }
        }
        //현재 이동속도
        public float CurrentMoveSpeed
        {
            get
            {
                float baseSpeed = IsMove ? (IsRun ? runSpeed : walkSpeed) : 0f;

                // 공중에서 이동 제어 감소
                if (!touchingDirection.IsGround)
                    baseSpeed *= airControlPercent;

                return baseSpeed;
            }
        }
        public bool IsFacingRight
        {
            get { return isFacingRight;}
            set
            {
                //반전 구현
                if (isFacingRight != value)
                {
                    this.transform.localScale *= new Vector2(-1, 1);
                }
                isFacingRight = value; 
            }
        }
        public bool IsJump
        {
            get => isJump;
            private set
            {
                isJump = value;
                animator.SetBool(AnimationString.JumpTrigger, value);
            }
        }
        #endregion

        #region Unity Event Method
        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>(); //참조
            animator = GetComponent<Animator>();
            touchingDirection = GetComponent<TouchingDirection>();
        }
        //일정한 간격으로 연산하기에 물리연산은 FixedUpdate에서 = time.Deltatime 필요 없음
        void FixedUpdate()
        { // 벽 슬라이드 판정
            isWallSliding = touchingDirection.IsWall
                            && !touchingDirection.IsGround
                            && rb2D.linearVelocity.y < 0f
                            && Mathf.Sign(inputmove.x) == Mathf.Sign(transform.localScale.x);

            // X이동 (벽에 붙어 있을 땐 제외)
            if (!isWallSliding && !isWallJumping)
            {
                rb2D.linearVelocity = new Vector2(inputmove.x * CurrentMoveSpeed, rb2D.linearVelocity.y);
            }
            //공격중 이동 제어
            if (!isWallSliding && !isWallJumping && !isAttacking)
            {
                rb2D.linearVelocity = new Vector2(inputmove.x * CurrentMoveSpeed, rb2D.linearVelocity.y);
            }
            // 벽 슬라이드 처리
            if (isWallSliding)
            {
                rb2D.linearVelocity = new Vector2(0f, Mathf.Max(rb2D.linearVelocity.y, wallSlideSpeed));
            }
            // 착지 시 점프 초기화
            if (touchingDirection.IsGround)
            {
                IsJump = false;
                currentJumpCount = 0;
                isWallJumping = false; // 벽점프 상태 해제
            }
            // 애니메이션 Y속도
            animator.SetFloat(AnimationString.Yvelocity, rb2D.linearVelocityY);
        }
        #endregion

        #region Custom Method
        //방향 전환
        void SetFacingDirection(Vector2 moveInput)
        {
            if (moveInput.x > 0f && !IsFacingRight) // 오른쪽 이동 시 오른쪽 바라보게
            {
                IsFacingRight = true;
            }
            else if (moveInput.x < 0f && IsFacingRight) // 왼쪽 이동 시 왼쪽 바라보게
            {
                IsFacingRight = false;
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (isAttacking) return; // 공격 중이면 이동 무시

            inputmove = context.ReadValue<Vector2>();
            IsMove = (inputmove != Vector2.zero);
            SetFacingDirection(inputmove);
        }
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started) //버튼을 눌렀을때
                IsRun = true;
            else if (context.canceled) //버튼을 땔때
                IsRun = false;
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            // 벽 슬라이드 중 벽 점프
            if (isWallSliding)
            {
                isWallJumping = true;
                animator.SetTrigger(AnimationString.JumpTrigger);

                // 방향 반전 후 반대쪽으로 점프
                IsFacingRight = !IsFacingRight;
                Vector2 jumpDir = new Vector2(IsFacingRight ? 1f : -1f, 1f);
                rb2D.linearVelocity = Vector2.zero;
                rb2D.AddForce(jumpDir.normalized * wallJumpForce, ForceMode2D.Impulse);

                Invoke(nameof(ResetWallJump), 0.2f); // 잠시 후 이동 복귀
            }
            // 일반 점프 (2단 포함)
            else if (currentJumpCount < maxJumpCount)
            {
                animator.SetTrigger(AnimationString.JumpTrigger);
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, 0f);
                rb2D.AddForce(Vector2.up * jumpForce);
                IsJump = true;
                currentJumpCount++;
            }

            void ResetWallJump()
            {
                isWallJumping = false;
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (touchingDirection.IsGround)
            {
                animator.SetTrigger("AttackTrigger");
                isAttacking = true; // 공격 시작
            }

            OnAttackEnd();
        }
        // Animation Event를 이용해서 공격 종료 시점에 호출
        public void OnAttackEnd()
        {
            isAttacking = false; // 공격 종료
        }
        #endregion
    }
}