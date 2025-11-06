using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 시차에 의한 배경 움직임 구현
    /// </summary>
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera cam; //카메라 오브젝트
        public Transform followTarget; //folloTarget

        Vector2 startPosition;  //배경 오브젝트의 최초 위치
        float startZ;  //배경 오브젝트의 최초 위치의 Z값 
        #endregion

        #region Property
        //시작 지점으로 부터 카메라의 이동 거리
        public Vector2 CamMoveSinceStart => startPosition - (Vector2)cam.transform.position;
        //플레이어와 배경과의 거리 = Z깊이 값
        public float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
        //
        public float clippingPlane => cam.transform.position.z + (zDistanceFromTarget < 0f ? cam.farClipPlane : cam.farClipPlane);
        //시차 계수
        float parrallaxFactor => Mathf.Abs(zDistanceFromTarget)/clippingPlane;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            startPosition = transform.position;
            startZ = transform.position.z;
        }
        //시차에 의한 배경 이동 위치 구하기
        private void Update()
        {
          Vector2 newPosition = startPosition + CamMoveSinceStart * parrallaxFactor;
          transform.position = new Vector3(newPosition.x, newPosition.y, startZ);
        }
        #endregion

        #region Custom Method

        #endregion
    }
}