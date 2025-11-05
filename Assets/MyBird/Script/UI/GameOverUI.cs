using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace MyBird
{
    /// <summary>
    /// GameOver UI를 관리하는 클래스
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        #region Variables
        //메뉴 씬
        private string loadToScene = "Title";

        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI bestScoreText;

        public GameObject newText;
        #endregion

        #region Unity Event Method
        //게임오버 UI 값 설정
        private void OnEnable()
        {
            scoreText.text = GameManager.Score.ToString();

            //베스트 스코어 가져오기
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);
            //베스트 스코어와 현재 스코어 비교
            if (GameManager.Score > bestScore) //현재 스코어 값이 더 클 경우 대체됨
            { 
                bestScore = GameManager.Score;
                PlayerPrefs.SetInt("BestScore", bestScore);

                //UI
                newText.SetActive(true);
            }
            bestScoreText.text = bestScore.ToString();
        }

        #endregion

        #region Custom Method
        //다시하기
        public void Retry()
        {
            // GameManager 상태 초기화 (씬 로드 전에 실행해야 함)
            // 만약 GameManager가 DontDestroyOnLoad라면 이 초기화가 필수입니다.
            GameManager.IsStart = false;
            GameManager.IsDeath = false;
            GameManager.Score = 0;

            // 현재 씬을 다시 로드
            string nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }
        //메뉴
        public void Menu()
        {
            Debug.Log("MainMenu로");
            //SceneManager.LoadScene
        }
        #endregion
    }
}