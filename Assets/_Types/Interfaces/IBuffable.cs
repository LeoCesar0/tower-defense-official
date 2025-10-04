/// <summary>
/// Interface for any character that can have buffs/debuffs
/// </summary>
public interface IBuffable
{
    void ApplyBuff(BuffData buff, int stackCount = 1);
    void RemoveBuff(BuffData buff);
    void RemoveAllBuffs();
    bool HasBuff(BuffData buff);
    int GetBuffStackCount(BuffData buff);
}
