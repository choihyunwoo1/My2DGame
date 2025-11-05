using UnityEngine;

namespace MyBird
{
    /// <summary>
    /// 기둥 프리팹 오브젝트를 스폰하는 클래스
    /// 1초에 하나씩 기둥을 소환
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        #region Variables
        public GameObject pipePrefab;

        //스폰 타이머
        float spawnTimer = 1.5f;
        float countdown = 0f;

        //스폰 높이 랜덤 범위 지정값
        float minSpawnY = -1.5f; //최저
        float maxSpawnY = 3.5f; //최대
        #endregion

        #region Unity Event Method
        private void Start()
        {
            spawnTimer = 1.5f;
        }
        private void Update()
        {
           
            if (GameManager.IsDeath == true)
                return;

            if (GameManager.IsStart == false)
                return;

            //1초에 기둥 하나씩 스폰
            countdown += Time.deltaTime;
            if (countdown >= spawnTimer)
            {
                //타이머 기능
                SpawnPipe();

                //타이머 초기화
                countdown = 0f;
                spawnTimer = 1.5f - GameManager.spawnValue;
            }
        }
        #endregion

        #region Custom Method
        void SpawnPipe()
        {
            float spawnY = transform.position.y + Random.Range(minSpawnY, maxSpawnY);
            Vector3 spawnPosition = new Vector3(transform.position.x, spawnY,transform.position.z);
            Instantiate(pipePrefab,spawnPosition, Quaternion.identity);
        }
        #endregion
    }
}