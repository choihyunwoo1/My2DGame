using UnityEngine;
using UnityEngine.Events;

namespace My2DGame
{
    /// <summary>
    /// 캐릭터에서 공통적으로 호출되는 이벤트 정적(Static) 함수 정의 
    /// </summary>
    public class CharacterEvents
    {
        //캐릭터가 데미지를 입을때 등록된 함수 호출하는 이벤트 함수
        public static UnityAction<Transform, float> characterDamaged;
        






    }
}