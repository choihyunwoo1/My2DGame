using UnityEngine;
using System.Collections;

namespace My2DGame
{
    /// <summary>
    /// 플레이어 동작에 따라 잔상 이펙트 효과 구현
    /// </summary>
    public class TrailEffect : MonoBehaviour
    {

        #region  Variables
        //참조
        SpriteRenderer spriteRenderer;

        //잔상 머티리얼
        public Material ghostMaterial;

        //잔상 효과
        bool isTrailActive = false; //효과 활성/비활성
        [SerializeField]float trailActiveTime = 2f; //효과 지속 시간
        [SerializeField] float trailRefreshRate = 0.1f; //잔상들의 발생 간격 시간
        [SerializeField] float trailDestroyDelay = 1f; //1초후에 킬 - 페이드 아웃 효과

        //잔상 페이드 아웃 효과
        string shaderValueRef = "_Alpha";
        [SerializeField] float shaderValueRate = 0.1f; //알파값 감소 비율
        [SerializeField] float shaderValueRefreshRate = 0.1f; //알파값이 감소 되는 시간 간격
        #endregion 

        #region Unity Event Method
        private void Awake()
        {
            //참조 
            spriteRenderer = GetComponent<SpriteRenderer>();    
        }
        #endregion

        #region Custom Method
        //잔상 효과 플레이
        public void StartTrailEffect()
        {
            //현재 효과가 진행 중이라면 리턴
            if (isTrailActive)
                return; 

            StartCoroutine(ActiveTrail(trailActiveTime));
        }

        //매개변수 효과 지속시간  
        IEnumerator ActiveTrail(float activeTime)
        { 
            isTrailActive = true;

            while (activeTime > 0f) //반복문
            {
                activeTime -= trailRefreshRate;

                //잔상 게임 오브젝트 만들기 - 현재 플레이어의 위치에 플레이어의 스프라이트로 만든다
                //하이라키창에 빈 오브젝트 
                GameObject ghostObject = new GameObject(); 
                //트랜스폼 셋팅 - 플레이어의 트랜스폼과 동일
                ghostObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
                ghostObject.transform.localScale = transform.localScale;
                //Sprite Renderer 셋팅
                SpriteRenderer renderer = ghostObject.AddComponent<SpriteRenderer>(); //새로 추가
                renderer.sprite = spriteRenderer.sprite;
                renderer.sortingLayerName = spriteRenderer.sortingLayerName;
                renderer.sortingOrder = spriteRenderer.sortingOrder - 1;
                renderer.material = ghostMaterial;

                //페이드 아웃 효과
                 // (Material, valueRef, goalFloat, stepAmount, refreshRate)
                StartCoroutine(AnimateMaterialFloat(renderer.material, shaderValueRef, 0f, shaderValueRate, shaderValueRefreshRate));

                //고스트 오브젝트 딜레이 후 킬
                Destroy(ghostObject, trailDestroyDelay);

                //딜레이
                yield return new WaitForSeconds(trailRefreshRate);

            }

            //효과 해제
            isTrailActive = false;
        }

        //Material 속성(Alpha) 값 감소
        IEnumerator AnimateMaterialFloat(Material material, string ValueRef, float goal, float rate, float refreshRate)
        { 
            float value = material.GetFloat(ValueRef);

            while(value > goal)
            {
                value -= rate;
                material.SetFloat(ValueRef, value);

                yield return new WaitForSeconds(refreshRate);   
            }
        }

        #endregion 

    }
}