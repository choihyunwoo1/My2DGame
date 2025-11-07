using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 그라운드, 천정, 벽 체크
    /// 접촉면 체크
    /// </summary>
    public class TouchingDirection : MonoBehaviour
    {
        #region Variables
        //참조
        //접촉하는 충돌체
        CapsuleCollider2D touchingCol; //접촉하는 충동체
        Animator animator;

        //접촉면 범위
        [SerializeField] float groundDistance = 0.05f;
        [SerializeField] float cellingDistance = 0.05f;
        [SerializeField] float wallDistance = 0.1f;

        //접촉 조건
        [SerializeField]
        ContactFilter2D contactFilter;
        
        //캐스트 결과
        RaycastHit2D[] groundHits = new RaycastHit2D[5];
        RaycastHit2D[] cellingHits = new RaycastHit2D[5];
        RaycastHit2D[] wallHits = new RaycastHit2D[5];

        [SerializeField] bool isGround;
        [SerializeField] bool isCelling;
        [SerializeField] bool isWall;
        #endregion

        #region Property
        public bool IsGround
        { 
            get { return isGround; }
            set 
            {
                isGround = value; 
                animator.SetBool(AnimationString.IsGrounded, value);
            }   
        }
        public bool IsCelling
        {
            get { return isCelling; }
            set
            {
                isCelling = value;
                //애니 파라미터 셋팅
            }
        }
        public bool IsWall
        {
            get { return isWall; }
            set
            {
                isWall = value;
                //애니 파라미터 셋팅
            }
        }

        //벽체크 할 방향
        Vector2 wallCheckDirection => (transform.localScale.x > 0f) ? Vector2.right : Vector2.left;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            touchingCol = GetComponent<CapsuleCollider2D>();    
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            IsGround = (touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0);
            IsCelling = (touchingCol.Cast(Vector2.up, contactFilter, cellingHits, cellingDistance) > 0);
            IsWall = (touchingCol.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0);
        }
        #endregion

        #region Custom Method

        #endregion
    }
}