using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    /// <summary>
    /// Flappy Bird 스타일 점프를 처리하는 Player 클래스
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Variables
        [SerializeField] 
        private float jumpForce = 0.4f; // 점프 세기 조절
        bool keyJump = false;

        [SerializeField]
        float readyForce = 5f; //밑에서 받쳐주는 힘

        private Rigidbody2D rb2D;

        [Header("회전 설정")]
        [SerializeField] private float maxUpRotation = 30f;    // 위로 회전 최대 각도
        [SerializeField] private float maxDownRotation = -90f; // 아래로 회전 최소 각도
        [SerializeField] private float rotationSpeed = 5f;     // 회전 부드럽게 전환 속도

        float moveSpeed = 5f; //이동 속도

        //버드 대기 UI
        public GameObject readyUI;

        public GameObject gameoverUI;
        #endregion

        #region Unity Event Method
        void Start()
        {
            rb2D = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기
        }

        void Update() //방향설정, 언어로 이루어진 메서드를 처리하기에 가장 용이함
        {
            // 스페이스바 또는 마우스 왼클릭을 눌렀을 때
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Debug.Log(" Jump ");
                keyJump = true;
            }

            RotateBird(); // 매 프레임마다 회전 처리

            MoveBird(); // Bird 이동
        }

        private void FixedUpdate() //직접적인 수치의 이동에 Update보다 안정적임
        {
            if (keyJump)
            {
                InPutBird();

                JumpBird();
                keyJump = false;
            }

            //시작 여부 체크
            if (GameManager.IsStart == false)
            {
                ReadyBird();
                return;
            }
        }

        //충돌체크 - 매개변수로 부딪힌 충돌체를 입력 받는다
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //충돌한 충돌체 체크
            if (collision.gameObject.tag == "Pipe")
            {
                GameOver();
            }
            else if (collision.gameObject.tag == "Ground")
            {
                GameOver();
            }
        }

        //점수 획득 체크
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Point")
            {
                Debug.Log("Point 획득");
                GameManager.Score++;
            }
        }
        #endregion

        #region Custom Method
        void GameOver()
        { 
            GameManager.IsDeath =true;
            gameoverUI.SetActive(true);
        }

        void InPutBird()
        {
            if(GameManager.IsDeath == true) 
                return;

            //스페이스키 OR 마우스 원클릭으로 입력받기
            keyJump |= Input.GetKeyDown(KeyCode.Space);
            keyJump |= Input.GetMouseButtonDown(0);

            //플레이어 이동 시작
            if (GameManager.IsStart == false && keyJump == true)
            { 
                GameManager.IsStart = true;
            }

            //UI
            readyUI.SetActive(false);
        }

        void ReadyBird()
        {
            if (rb2D.linearVelocityY < 0f)
            {
                rb2D.linearVelocity = Vector2.up * readyForce;
            }
        }

        void JumpBird()
        {
            if (GameManager.IsDeath == true)
                return;

            // 기존 속도를 0으로 초기화해서 일관된 점프 느낌 만들기
            rb2D.linearVelocity = Vector2.zero;

            // 위 방향으로 힘을 가함
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        void RotateBird()
        {
            if (GameManager.IsDeath == true)
                return;

            // 새의 현재 y속도에 따라 회전 각도를 계산
            float targetAngle;

            if (rb2D.linearVelocity.y > 0)
            {
                // 올라가는 중일 때 위로 회전 (+30도까지)
                targetAngle = maxUpRotation;
            }
            else
            {
                // 떨어질 때 아래로 회전 (-90도까지)
                // 속도가 빠를수록 더 기울이기 위해 보간
                targetAngle = Mathf.Lerp(0, maxDownRotation, -rb2D.linearVelocity.y / 10f);
            }

            // 현재 회전 → 목표 회전으로 부드럽게 보간
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
         
        // Bird 이동
        void MoveBird()
        {
            if(GameManager.IsDeath == true)
                return;

            //오른쪽으로 이동
            transform.Translate(Vector3.right* moveSpeed* Time.deltaTime, Space.World); 
        }


        #endregion 
    }
}