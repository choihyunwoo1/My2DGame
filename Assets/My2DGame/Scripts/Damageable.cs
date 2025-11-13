using UnityEngine;
using UnityEngine.Events;

namespace My2DGame
{
    /// <summary>
    /// Health를 관리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;

        [SerializeField]
        private float currentHealth;

        [SerializeField]
        private float maxHealth = 100f;

        //죽음 체크
        private bool isDeath = false;

        //무적모드
        private bool isInvincible = false;
        //무적 모드 타이머
        [SerializeField]
        private float invincibleTimer = 3f;
        private float countdown = 0f;

        //데미지 입을때 호출되는 이벤트 함수
        public UnityAction<float, Vector2> hitAction;
        //힐 할때 호출되는 이벤트 함수
        public UnityAction<float> healAction;
        #endregion

        #region Property
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = value;

                if(currentHealth <= 0f)
                {
                    IsDeath = true;
                }
            }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
            private set
            {
                maxHealth = value;
            }
        }

        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                animator.SetBool(AnimationString.IsDeath, value);
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
        }

        private void Update()
        {
            //무적 타이머 - 무적 모드일때
            if (isInvincible)
            {
                countdown += Time.deltaTime;
                if (countdown >= invincibleTimer)
                {
                    //타이머 구현 - 무적모드 해제
                    isInvincible = false;

                    //타이머 초기화
                    countdown = 0f;
                }
            }
            
        }
        #endregion

        #region Custom Method
        //데미지 주기
        public void TakeDamage(float damage, Vector2 knockback)
        {
            //죽음 체크, 무적 체크
            if (isDeath || isInvincible)
                return;

            CurrentHealth -= damage;
            Debug.Log($"CurrentHealth:{CurrentHealth}");

            isInvincible = true;

            //애니메이션
            animator.SetTrigger(AnimationString.HitTrigger);

            //데미지 효과(knockback)
            //hitAction 이벤트에 등록된 함수 호출
            hitAction?.Invoke(damage, knockback); //넉백

            //데미지 텍스트 연출 효과
            CharacterEvents.characterDamaged?.Invoke(transform, damage);
        }

        //힐하기 , 힐 성공시 true, 실패하면 false
        public bool Heal(float healAmount)
        {
            //죽음 체크, 무적 체크
            if (isDeath)
                return false;

            //체력 만땅 체크
            if (currentHealth >= maxHealth)
                return false;

            //리얼 힐 값 구하기
            float maxHeal = MaxHealth - CurrentHealth; //최대로 힐 할수 있는 값
            float realHeal =  (maxHeal < healAmount) ? maxHeal : healAmount; 

                currentHealth += realHeal;
            Debug.Log($"플레이어 회복! 현재 체력: {currentHealth}");
            
            //healAction의 null체크 :healAction에 등록된 함수가 있는지 진위여부 체크
            /*if(healAction != null)
            {
                healAction.Invoke(realHeal);
            }*/

            healAction?.Invoke(realHeal);

            CharacterEvents.characterHeal.Invoke(transform, realHeal);

            return true;
        }
        #endregion
    }
}