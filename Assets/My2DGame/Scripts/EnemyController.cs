using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// Enemy를 관리하는 클래스
    /// </summary>
    [RequireComponent (typeof(Rigidbody2D), typeof(TouchingDirections))]
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private TouchingDirections touchingDirections;
        private Animator animator;
        private Damageable damageable;

        //적 감지
        public DetectionZone DetectionZone;
        //땅 감지
        public DetectionZone DetectionGround;

        //이동
        //이동 속도
        [SerializeField] private float runSpeed = 4f;   
        //이동 방향
        private Vector2 directionVector = Vector2.right;

        //이동 가능한 방향 정의
        public enum WalkableDirection
        {
            Left,
            Right
        }

        //현재 이동 방향
        private WalkableDirection walkDirection = WalkableDirection.Right;

        //감속 Lerp 계수 
        [SerializeField] private float stopRate = 0.2f;

        //적 감지 - 타겟이 있다
        private bool hasTarget = false;
        #endregion

        #region Property
        public WalkableDirection WalkDirection
        {
            get { return walkDirection; }
            private set
            {
                //방향 전환이 일어난 시점
                if(walkDirection != value)
                {
                    //이미지 플립
                    transform.localScale *= new Vector2(-1, 1);

                    //value값에 따라 이동 방향 설정
                    if (value == WalkableDirection.Left)
                    {
                        directionVector = Vector2.left;
                    }
                    else if(value == WalkableDirection.Right)
                    {
                        directionVector = Vector2.right;
                    }
                }

                walkDirection = value;
            }
        }

        //애니메이터의 파라미터 값(CannotMove) 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.CannotMove);
            }
        }

        //적 감지
        public bool HasTarget
        {
            get { return hasTarget; }
            set
            {
                hasTarget = value;
                animator.SetBool(AnimationString.HasTarget, value);
            }
        }

        //공격 쿨 타임 - 애니메이션 파라미터 값 셋팅
        public float CoolDownTime
        {
            get { return animator.GetFloat(AnimationString.CoolDownTime); }
            set { animator.SetFloat(AnimationString.CoolDownTime, value); }
        }

        //속도 잠금 - 애니메이션의 파라미터 값 읽기
        public bool LockVelocity
        {
            get { return animator.GetBool(AnimationString.LockVelocity); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            touchingDirections = this.GetComponent<TouchingDirections>();
            animator = this.GetComponent<Animator>();
            damageable = this.GetComponent<Damageable>();

            //이벤트 함수 등록
            damageable.hitAction += OnHit;

            //땅 끝 감지
            DetectionGround.noRemainCollider += OncliffDetection;
        }

        private void Update()
        {
            bool playerDetected = false;

            foreach (var col in DetectionZone.detectedColliders)
            {
                if (col != null && col.CompareTag("Player"))
                {
                    playerDetected = true;
                    break;
                }
            }

            // HasTarget은 오직 Player 감지 기준
            HasTarget = playerDetected;

            // 공격 쿨타임 처리
            if (CoolDownTime > 0f)
            {
                CoolDownTime -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            //벽 체크
            if(touchingDirections.IsWall && touchingDirections.IsGround)
            {
                Flip();
            }

            if (LockVelocity == false)
            {
                //이동하기
                if (CannotMove)
                {
                    //감속 rb2D.linearVelocityX -> 0
                    rb2D.linearVelocity = new Vector2(Mathf.Lerp(rb2D.linearVelocityX, 0f, stopRate), rb2D.linearVelocityY);
                }
                else
                {
                    rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocityY);
                }
            }
        }
        #endregion

        #region Custom Method
        //방향 전환
        void Flip()
        {
            if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if(WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("Erro Flip Direction");
            }
        }

        //데미지 이벤트에 등록되는 함수
        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocityY + knockback.y);
        }

        //디텍션 이벤트에 등록되는 함수
        public void OncliffDetection()
        {
            if (touchingDirections.IsGround)
            {
                Flip();
            }
        }
        #endregion
    }
}