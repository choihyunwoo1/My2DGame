using UnityEngine;

namespace MyBird
{
    /// <summary>
    /// 게임 전체의 흐름을 관리하는 클래스
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        static bool isStart;
        #endregion

        #region Property
        public static bool IsStart
        {
            get { return isStart; }
            set {isStart = value;}
        }
        #endregion

        #region Unity Event Method
        void Start()
        {
            //초기화
            isStart = false;
        }
        void Update()
        {
      
        }
        #endregion

        #region Custom Method
 
        #endregion

    }
}