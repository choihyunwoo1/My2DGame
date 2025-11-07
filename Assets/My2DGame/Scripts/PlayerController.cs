using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public float walkSpeed = 3f; // 걷기 속도
        public float runSpeed = 5f;  // 달리기 속도
        public float jumpForce = 400f; //점프 힘

        //참조
        private Animator animator;
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection; 

        private Vector2 inputmove = Vector2.zero;


        // 상태값
        private bool isMove = false;
        private bool isFacingRight = true;
        private bool isRun = false; // Shift키로 조작
        private bool isJump = false;
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
                if (IsMove)
                {
                    if (IsRun)
                    {
                        return runSpeed;
                    }
                    else
                    { 
                        return walkSpeed;
                    }
                }
                else //이동불가
                {
                    return 0f;
                }
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
                animator.SetBool(AnimationString.IsJump, value);
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
        {
            rb2D.linearVelocity = new Vector2(inputmove.x * CurrentMoveSpeed, rb2D.linearVelocity.y);

            // 땅에 닿았으면 점프 상태 해제
            if (touchingDirection.IsGround && IsJump)
            {
                IsJump = false;
            }
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
            // 점프 조건: performed && 땅 위일 때만
            if (context.performed && touchingDirection.IsGround)
            {
                rb2D.AddForce(Vector2.up * jumpForce);
                IsJump = true;
            }
        }
        #endregion
    }
}