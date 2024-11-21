using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPattern", menuName = "Pattern/Enemy")]
public class EnemyPattern : ScriptableObject
{
    [SerializeField] private List<ArmorPattern> _listPattern;

    public List<ArmorPattern> ListPattern { get => _listPattern ; private set => _listPattern = value; }
}

[Serializable]
public class EnemyArmor
{
    public bool armorUp;
    public bool armorBottom;
    public bool armorLeft;
    public bool armorRight;
}

[Serializable]
public class ArmorPattern
{
    public List<EnemyArmor> listEnemyArmor;
}
