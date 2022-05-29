using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private Player player;

    private void Update()
    {
        var curhp = player.CurrentHp.ToString();
        var ammo = player.CurrentNumberOfFirebolls.ToString();
        hpText.text = $"Health: {curhp}\nAmmo: {ammo}";
    }
}
