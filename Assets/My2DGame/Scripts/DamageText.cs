using UnityEngine;
using TMPro;

namespace My2DGame
{
    /// <summary>
    /// 데미지 효과 : 캐릭터 머리위에 텍스트 띄우기
    /// 위로 이동하기, 이동하면서 페이드 아웃이 끝나면 킬
    /// </summary>
    public class DamageText : MonoBehaviour
    {
        #region Variables
        //참조
        RectTransform rectTransform;
        TextMeshProUGUI damageText;

        [SerializeField] float moveSpeed = 10f;

        //페이드 효과
        Color startColor;

        [SerializeField]
        float fadeTimer = 1f;
        float countdown = 0;

        //페이드 지연
        [SerializeField]
        float delayTimer = 1f;
        float delayCountdown = 0;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rectTransform = GetComponent<RectTransform>();
            damageText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            //초기화
            countdown = 0;
            delayCountdown = 0;
            startColor = damageText.color;
        }
        private void Update()
        {
            //위로 이동
            rectTransform.position += Vector3.up * Time.deltaTime * moveSpeed;

            if (delayCountdown < delayTimer)
            {
                delayCountdown += Time.deltaTime;
                return;
            }

            //페이드 효과
            countdown += Time.deltaTime;

            float alphaValue = startColor.a * (1-(countdown/fadeTimer));
            damageText.color = new Color(startColor.a ,startColor.g, startColor.b, alphaValue);

            //페이드 효과 완료 후 킬
            if (countdown >= fadeTimer)
            { 
                Destroy(gameObject);
            }
        }
        #endregion

        #region Custom Method

        #endregion
    }
}