using UnityEngine;
[CreateAssetMenu(fileName = "NuevaSkill", menuName = "SkillTree/Tree")]
public class SkillData : ScriptableObject
{
    public string skillid;
    public string skillname;
    [TextArea] public string description;
    public int expCost;
    public SkillData requiredskill;

}
