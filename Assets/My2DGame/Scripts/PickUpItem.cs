using UnityEngine;

namespace My2DGame
{
    public class PickUpItem : MonoBehaviour
    {
        // Variables
        [SerializeField] private float healthRestore = 20f;

        //아이템 회전 연출
        Vector3 rotationSpeed = new Vector3(0f, 180f, 0f);

        // Unity M
        private void Update()
        {
            //회전
            transform.eulerAngles += Time.deltaTime * rotationSpeed;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool isPickup = PickUp(collision);
            if (isPickup)
            {
                Destroy(gameObject);
            }
        }

        //Custom M
        //픽업시 아이템 효과 구현 - 성공시 true, 실패시 false
        protected virtual bool PickUp(Collider2D collision)
        {
            bool isUse= false;

            Damageable damageable = collision.GetComponentInParent<Damageable>();

            if (damageable != null)
            {
                isUse = damageable.Heal(healthRestore);
            }

            return isUse;
        }

        //아이템을 회전 시킨다.
        
    }
}