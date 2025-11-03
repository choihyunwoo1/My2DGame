using UnityEngine;

namespace MyBird
{
    /// <summary>
    /// 카메라의 움직임을 제어하는 클래스 - 플레이어의 오른쪽 이동에 따라간다.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Variables
        public Transform player; //플레이어

        public float followSpeed = 5f; // 카메라가 플레이어를 따라가는 부드러운 속도
        [SerializeField]
        private Vector3 offset;        // 플레이어와 카메라 사이 초기 거리
        #endregion

        #region Unity Event Method
        void Start()
        {
            if (player == null)
            {
                Debug.LogError("Player Transform이 할당되지 않았습니다!");
                return;
            }

            // 초기 X,Y,Z 거리 계산
            offset = transform.position - player.position;
        }

        void LateUpdate() //원래 카메라 업데이트는 LateUpdate()에서 우선시된다.
        {
            if (player == null) return;

            // 목표 위치 계산 (플레이어 X에 따라가고, Y,Z는 기존 그대로)
            Vector3 targetPos = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);

            // 부드럽게 따라가기
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
        #endregion

        #region Custom Method

        #endregion
    }
}