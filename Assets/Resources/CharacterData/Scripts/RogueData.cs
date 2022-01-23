using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rogue Data", menuName = "Character data/Rogue")]
public class RogueData : CharacterData
{
    public RogueClassType ClassType;
    public RogueWeaponType WeaponType;
}
