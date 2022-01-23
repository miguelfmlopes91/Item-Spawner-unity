using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

[CreateAssetMenu(fileName = "New Warrior Data", menuName = "Character data/Warrior")]
public class WarriorData : CharacterData
{
    public WarriorStrategyType StrategyType;
    public WarriorWeaponType WeaponType;
}
