using UnityEngine;

public sealed class GameLogic : MonoBehaviour
{
    public GameObject dungeonCanvas;
    public GameObject mapCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            EnterBattle(1);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            Game.CurrentCombatSystem.CurrentCombat.EndTurn();
        }


        if (Game.CurrentCombatSystem.Enemies.Count == 0)
        {
            mapCanvas.SetActive(true);
            dungeonCanvas.SetActive(false);
            Game.PlayerDeck.Initialize();
        }
    }


    public void EnterBattle(int mapLevel)
    {
        var currentCombatSystem = Game.CurrentCombatSystem = new CombatSystem();
        currentCombatSystem.StartCombat(true);
    }

    /*
    public void Initialize()
    {
        CurrentHand = Game.PlayerDeck.DrawHand();
        CombatHistory = new Queue<Combat>();
        TurnNumber = 1;

        CurrentCombat = new Combat
        {
            OnTurnCompleted = NextTurn,
            CardsInCombat = new List<Card>(),
            TurnNumber = TurnNumber
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //currentCombat.EndTurn();
            DamageReceived?.Invoke(3, Game.Player);
        }
    }*/
}