using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IPlayer
{
    public string PlayerName { get; set; } // Implement the PlayerName property
    public GameObject player;
    public System.Collections.Generic.List<IPlayer.AbilityType> acquiredAbilities = new System.Collections.Generic.List<IPlayer.AbilityType>();

    public Player()
    {
        // Constructor logic here
        PlayerName = "Bingsu (빙수)"; // Example default name
    }

    public Player(string playerName, System.Collections.Generic.List<IPlayer.AbilityType> abilities)
    {
        PlayerName = playerName;
        acquiredAbilities = abilities;
    }

    void Start()
    {
        // Example of granting an initial ability
        GrantAbility(IPlayer.AbilityType.Slide);
        GrantAbility(IPlayer.AbilityType.DoubleJump);
    }

    public bool HasAbility(IPlayer.AbilityType ability)
    {
        return acquiredAbilities.Contains(ability);
    }

    public void GrantAbility(IPlayer.AbilityType ability)
    {
        if (!HasAbility(ability))
        {
            acquiredAbilities.Add(ability);
            Debug.Log($"{PlayerName} learned the ability: {ability}");
            // Implement logic to enable the ability (e.g., set a boolean flag, enable a component).
        }
        else
        {
            Debug.Log($"{PlayerName} already has the ability: {ability}");
        }
    }

    public void RevokeAbility(IPlayer.AbilityType ability)
    {
        if (HasAbility(ability))
        {
            acquiredAbilities.Remove(ability);
            Debug.Log($"{PlayerName} lost the ability: {ability}");
            // Implement logic to disable the ability.
        }
        else
        {
            Debug.Log($"{PlayerName} doesn't have the ability: {ability}");
        }
    }
}