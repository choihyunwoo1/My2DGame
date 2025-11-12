using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 그라운드, 천정, 벽 체크
    /// </summary>
    public class TouchingDirections : MonoBehaviour
    {
        #region Variables
        //참조        
        private CapsuleCollider2D touchingCol; //접촉하는 충돌체
        private Animator animator;

        //접촉면 범위
        [SerializeField] private float groundDistance = 0.05f;
        [SerializeField] private float cellingDistance = 0.05f;
        [SerializeField] private float wallDistance = 0.1f;

        //접촉 조건
        [SerializeField]
        private ContactFilter2D contactFilter;

        //캐스트 결과
        private RaycastHit2D[] groundHits = new RaycastHit2D[5];
        private RaycastHit2D[] cellingHits = new RaycastHit2D[5];
        private RaycastHit2D[] wallHits = new RaycastHit2D[5];

        //
        [SerializeField] private bool isGround;
        [SerializeField] private bool isCelling;
        [SerializeField] private bool isWall;
        #endregion

        #region Property
        public bool IsGround
        {
            get { return isGround; }
            private set
            {
                isGround = value;
                animator.SetBool(AnimationString.IsGrounded, value);
            }
        }

        public bool IsCelling
        {
            get { return isCelling; }
            private set
            {
                isCelling = value;
                //애니 파라미터 셋팅
            }
        }

        public bool IsWall
        {
            get { return isWall; }
            private set
            {
                isWall = value;
                //애니 파라미터 셋팅
            }
        }

        //벽체크 할 방향
        private Vector2 wallCheckDirection => (this.transform.localScale.x > 0f) ? Vector2.right : Vector2.left;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            touchingCol  = this.GetComponent<CapsuleCollider2D>();
            animator = this.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            IsGround = (touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0);
            IsCelling = (touchingCol.Cast(Vector2.up, contactFilter, cellingHits, cellingDistance) > 0);
            IsWall = (touchingCol.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance ) > 0);

        }
        #endregion
    }
}