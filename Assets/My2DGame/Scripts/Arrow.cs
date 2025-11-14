using UnityEngine;

namespace My2DGame
{
    /// <summary>
    /// 플레이어의 활 공격(화살)을 관리하는 클래스
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Arrow : MonoBehaviour
    {
        [Header("Arrow Settings")]
        [SerializeField] private float damage = 10f;   // 화살 공격력
        [SerializeField] private float lifeTime = 5f;  // 자동 제거 시간

        private Rigidbody2D rb;
        private Collider2D col;
        private bool hasHit = false; // 한 번만 충돌 처리

        [SerializeField] private GameObject hitEffectPrefab; // 적중 이펙트 프리팹
        [SerializeField] private float hitEffectLifeTime = 0.5f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        private void Start()
        {
            // Collider를 Trigger로 사용하면 관통 가능
            col.isTrigger = true;

            // 일정 시간이 지나면 제거
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasHit) return; // 중복 처리 방지
            hasHit = true;

            // 적 또는 Damageable 객체 감지
            Damageable target = collision.GetComponent<Damageable>();

            if (target != null)
            {
                // 피격 방향 계산 (넉백용)
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
                Vector2 knockback = hitDirection * 2f; // 넉백 세기 (원하면 수정 가능)

                // 데미지 적용
                target.TakeDamage(damage, knockback);

                // 적중 이펙트 생성
                if (hitEffectPrefab != null)
                {
                    GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(effect, hitEffectLifeTime);
                }
            }

            // 충돌 즉시 제거 (또는 충돌 애니메이션 추가 가능)
            Destroy(gameObject);
        }
    }
}
