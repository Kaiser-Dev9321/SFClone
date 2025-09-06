using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TCBuild
{
    public int TC_comboCheck;
    public AttackData currentTC_attackData;
}

[CreateAssetMenu(fileName = "TCData_", menuName = "TC Data/New TC Data")]
public class TargetComboData : ScriptableObject
{
    public string tc_ID;
    public TCBuild[] tcBuild;
}
