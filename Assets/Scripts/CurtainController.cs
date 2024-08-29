using TMPro;
using UnityEngine;
using UnityEngine.UI; // Include the UI namespace

public class CurtainController : MonoBehaviour
{
    public GameObject curtain;
    public TextMeshProUGUI Round; // Reference to the Text component
    public TextMeshProUGUI Winner;

    public void On(int roundNumber,string winnerName)
    {
        curtain.SetActive(true);
        Round.text += roundNumber;
        Winner.text += winnerName;
    }

    public void Off()
    {
        curtain.SetActive(false);
    }
}
