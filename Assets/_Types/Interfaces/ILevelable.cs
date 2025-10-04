/// <summary>
/// Interface for any character that can level up
/// </summary>
public interface ILevelable
{
    void GainExperience(int amount);
    int GetExperience();
    int GetExperienceToNextLevel();
    bool CanLevelUp();
    void OnLevelUp();
}
