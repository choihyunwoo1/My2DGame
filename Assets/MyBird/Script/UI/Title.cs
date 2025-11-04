using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyBird
{
    /// <summary>
    /// 타이틀 씬을 관리하는 클래스
    /// </summary>
    public class Title : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private string loadToScene = "PlayScene";
        #endregion

        #region Custom Method
        //플레이 버튼
        public void Play()
        { 
            SceneManager.LoadScene(loadToScene);
        }
        #endregion
    }
}