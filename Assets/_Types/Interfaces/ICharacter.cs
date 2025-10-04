/// <summary>
/// Main character interface that combines all character functionality
/// </summary>
public interface ICharacter : IDamageable, IAttacker, IMovable, IAbilityUser, IStatHolder, IEquippable, IBuffable, ILevelable, ITargetable
{
    CharacterData GetCharacterData();
    CharacterClass GetCharacterClass();
    CharacterType GetCharacterType();
    int GetLevel();
}
