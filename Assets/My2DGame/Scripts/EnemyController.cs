using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// Enemy를 관리하는 클래스
    /// </summary>
    [RequireComponent (typeof(Rigidbody2D),typeof(TouchingDirection))]
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirection touchingDirection;

        //속도
        [SerializeField] float runSpeed = 4f;
        //이동 방향
        Vector2 directionVector = Vector2.right;

        //이동 가능한 방향 정의
        public enum WalkableDirection
        { 
            Left,
            Right
        }

        //현재 이동 방향
        WalkableDirection walkDirection = WalkableDirection.Right;

        // 방향 전환 쿨타임
        private bool canFlip = true;
        [SerializeField] private float flipCooldown = 0.2f;
        #endregion

        #region Property
        public WalkableDirection WalkDirection
        { 
            get { return walkDirection; }
            set
            {
                //방향 전환이 일어난 시점
                if (walkDirection != value) //밸류값이 저장된 놈과 틀리면~
                { 
                    //이미지 플립
                    transform.localScale *= new Vector2 (-1, 1);

                    //value값에 따라 이동 방향 설정
                    if (value == WalkableDirection.Left)
                    {
                        directionVector = Vector2.left;
                    }
                    else if (value == WalkableDirection.Right)
                    {
                        directionVector = Vector2.right;
                    }
                }
                walkDirection = value;             
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirection = GetComponent<TouchingDirection>();
        }
        private void FixedUpdate()
        {
            // 벽 체크 (바닥에 닿아있을 때만)
            if (touchingDirection.IsWall && touchingDirection.IsGround && canFlip)
            {
                Flip();
            }

            // 이동
            rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocityY);
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
            else if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("Error Flip Direction");
            }

            // 플립 직후 잠시 대기
            StartCoroutine(FlipCooldown());
        }
        private System.Collections.IEnumerator FlipCooldown()
        {
            canFlip = false;
            yield return new WaitForSeconds(flipCooldown);
            canFlip = true;
        }
        #endregion
    }
}