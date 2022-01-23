using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mage Data", menuName = "Character data/Mage")]
public class MageData : CharacterData
{
    public MageDamageType dmgType;
    public MageWeaponType wpnType;
}
