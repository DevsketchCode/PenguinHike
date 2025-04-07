using UnityEngine;

// Define an interface for the Player
public interface IPlayer
{
    // Property to get and set the player's name
    string PlayerName { get; set; }

    // Enum to represent the different abilities the player can have
    enum AbilityType
    {
        DoubleJump,
        Grab,
        Slide,
        Throw,
        // Add more abilities here as needed
    }

    // Method to check if the player has a specific ability
    bool HasAbility(AbilityType ability);

    // Method to grant an ability to the player
    void GrantAbility(AbilityType ability);

    // Optional: Method to revoke an ability from the player
    void RevokeAbility(AbilityType ability);
}