/// <summary>
/// Interface for any character that has stats
/// </summary>
public interface IStatHolder
{
    CharacterStats GetStats();
    void ApplyEquipmentBonus(EquipmentBonus bonus);
    void RemoveEquipmentBonus(EquipmentBonus bonus);
}
