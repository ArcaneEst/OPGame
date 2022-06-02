using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private Player player;

    private void Update()
    {
        hpText.text = player.CurrentNumberOfFireballs.ToString();
    }
}
