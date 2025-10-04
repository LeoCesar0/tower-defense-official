/// <summary>
/// Interface for any character that can use abilities
/// </summary>
public interface IAbilityUser
{
    void UseAbility(AbilityData ability);
    void UseAbility(int abilityIndex);
    bool CanUseAbility(AbilityData ability);
    float GetAbilityCooldown(AbilityData ability);
    AbilityData[] GetAvailableAbilities();
}
