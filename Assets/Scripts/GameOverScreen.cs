using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI message;

    public void GameOver(string winner)
    {
        this.gameObject.SetActive(true);
        LeanTween.scale(this.gameObject, Vector3.one, 1f);
        message.text = $"{winner} Wins the game";
    }
}
