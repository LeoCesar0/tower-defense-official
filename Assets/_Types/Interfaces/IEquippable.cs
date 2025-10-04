/// <summary>
/// Interface for any character that can equip items
/// </summary>
public interface IEquippable
{
    void EquipItem(EquipmentData equipment);
    void UnequipItem(EquipmentSlot slot);
    EquipmentData GetEquippedItem(EquipmentSlot slot);
    bool CanEquip(EquipmentData equipment);
}
