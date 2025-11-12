using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 시차에 의한 배경 움직임 구현
    /// </summary>
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera cam;              //카메라 오브젝트
        public Transform followTarget;   //플레이어

        private Vector2 startPosition;      //배경 오브젝트의 최초 위치
        private float startZ;               //배경 오브젝트의 최초 위치의 z값
        #endregion

        #region Propety
        //시작 지점으로 부터 카메라의 이동 거리
        public Vector2 CamMoveSinceStart => startPosition - (Vector2)cam.transform.position;  

        //플레이어와 배경과의 거리
        public float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

        //
        public float clippingPlane => cam.transform.position.z + (zDistanceFromTarget < 0f ? cam.farClipPlane : cam.nearClipPlane);

        //시차 계수
        private float parrallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
        #endregion


        #region Unity Event Method
        private void Start()
        {
            //초기화
            startPosition = this.transform.position;
            startZ = this.transform.position.z;
        }

        //시차에 의한 배경 이동 위치 구하기
        private void Update()
        {   
            Vector2 newPostion = startPosition + CamMoveSinceStart * parrallaxFactor;
            this.transform.position = new Vector3(newPostion.x, newPostion.y, startZ);
        }
        #endregion

    }
}