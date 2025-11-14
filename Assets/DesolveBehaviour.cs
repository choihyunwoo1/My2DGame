using UnityEngine;

namespace My2DGame
{
    public class DesolveBehaviour : StateMachineBehaviour
    {
        #region Variables
        private Renderer renderer;
        public Material desolveMaterial;

        //딜레이 타이머
        public float delayTimer = 1f;
        float delayCountdown = 0;

        //페이드 타이머
        float countdown = 0f;

        string SplitValue = "_SplitValue";
        #endregion

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { 
            //참조
            renderer = animator.GetComponent<Renderer>();
            //초기화
            countdown = 1f;
            delayCountdown = 0;

            //디졸브 초기화
            renderer.material = desolveMaterial;
            renderer.material.SetFloat("_SplitValue", 1.0f);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //딜레이 체크
            if (delayCountdown < delayTimer)
            {
                delayCountdown += Time.deltaTime;
                return;
            }

            //디졸브 효과 시작 - 1 -> 0 (split value)
            if (countdown >= 0f)
            {
                countdown -= Time.deltaTime;
                if(countdown < 0f) countdown = 0;

                renderer.material.SetFloat("_SplitValue", countdown);
            }
        }

        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
       
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
       
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    }
}