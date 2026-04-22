using System;
using UnityEngine;
using TMPro;

public class HeadsUpDisplay : Singleton<HeadsUpDisplay>
{
    [SerializeField] private TextMeshProUGUI _drawPileNumber;
    [SerializeField] private TextMeshProUGUI _discardPileNumber;
    [SerializeField] private TextMeshProUGUI _energyNumber;


    public void UpdateDeckValues(int drawPileCount, int discardPileCount)
    {
        _drawPileNumber.text = drawPileCount.ToString();
        _discardPileNumber.text = discardPileCount.ToString();
    }

    public void UpdateEnergyValues(int currentEnergy, int maxEnergy)
    {
        _energyNumber.text = $"{currentEnergy}/{maxEnergy}";
    }
    public void EndTurn()
    {
        TurnManager.Instance.EndTurn();
    }

    public void EndCombat()
    {
        CombatManager.Instance.EndCombat(false);
    }
}
