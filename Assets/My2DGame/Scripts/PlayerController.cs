using UnityEngine;
using UnityEngine.InputSystem;

namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public float walkSpeed = 5f; //속도

        Rigidbody2D rb2D; //Rigidbody 컴포넌트
        Vector2 inputmove = Vector2.zero; //저장해놓은 Actions의 Value값, Zero는 값이 입력되지 않으면 알아서 멈추기 위한 자동 초기화
        #endregion

        #region Property

        #endregion

        #region Unity Event Method
        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>(); //참조
        }
        //일정한 간격으로 연산하기에 물리연산은 FixedUpdate에서 = time.Deltatime 필요 없음
        void FixedUpdate()
        {
            //이동
            rb2D.linearVelocity = inputmove * walkSpeed;
            //rb2D.linearVelocity = new Vector2(inputmove.x, 
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
                inputmove = context.ReadValue<Vector2>();     
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                rb2D.AddForce(Vector2.up * 400f);
            }
        }
        #endregion
    }
}