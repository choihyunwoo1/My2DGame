using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 애니메이션 상태 동안 페이드 아웃 후 오브젝트 킬
    /// </summary>
    public class FadeOutBehaviour : StateMachineBehaviour
    {
        #region Variables
        //참조
        SpriteRenderer spriteRenderer;
        GameObject removeObject;

        //딜레이 타이머
        public float delayTimer = 1f;
        float delayCountdown = 0;

        //페이드 타이머
        public float fadeTimer = 1f;
        float countdown = 0f;

        Color startColor;
        #endregion


        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { 
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            removeObject = animator.gameObject;

            //초기화
            countdown = 0f;
            delayCountdown = 0;
            startColor =spriteRenderer.color;
        }
        //타이머 기능은 업데이트에선
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //딜레이 체크
            if (delayCountdown < delayTimer)
            { 
                delayCountdown += Time.deltaTime;
                return;
            }

            //페이드 타이머 - Alpha값이 1 -> 0
            countdown += Time.deltaTime;

            // 0 -> 1
            float alphavalue = startColor.a * (1 - (countdown / fadeTimer));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alphavalue);

            //타이머 종료시
            if (countdown > fadeTimer)
            {
                //Kill
                Destroy(removeObject);
            }
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { }
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { }
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        { }
    }
}