using UnityEngine;
using TMPro;

namespace MyBird
{
    public class DrawScore : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        private void Update()
        {
            scoreText.text = GameManager.Score.ToString();
        }
    }
}