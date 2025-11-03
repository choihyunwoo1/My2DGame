using UnityEngine;

namespace MyBird
{
    /// <summary>
    /// 그라운드 배경 이동(롤링) 구현
    /// 땅을 왼쪽으로 무한 반복시키는 클래스
    /// </summary>
    public class GroundMove : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        float moveSpeed = 5f; // 이동 속도
        #endregion

        #region Unity Event Method
        void Update()
        {
            RollingMove();
        }
        #endregion

        #region Custom Method
        void RollingMove()
        {
            // GroundParent 전체를 왼쪽으로 이동
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);

            // GroundParent가 한 땅 이미지 길이만큼 이동하면 위치 초기화
            if (transform.localPosition.x <= -8.4f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 8.4f, transform.localPosition.y, transform.localPosition.z);
            }
        }
        #endregion

    }
}