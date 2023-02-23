using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Singleton
    private static AIManager instance;
    public static AIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("AIManager").AddComponent<AIManager>();
                Debug.LogWarning("AI-Manager Instance was missing");
            }

            return instance;
        }
    }
    #endregion

    #region Attributes
    [SerializeField]
    private List<Entity> entities;

    [SerializeField]
    private List<StateMachine> stateMachines;

    [SerializeField]
    private TextMeshProUGUI defenderHealthText;
    [SerializeField]
    private TextMeshProUGUI defenderPopulationText;
    [SerializeField]
    private TextMeshProUGUI invaderPopulationText;

    [SerializeField]
    private Destination defenderHealth;

    private int invaderNumber = 0;
    private int defenderNumber = 0;

    private bool itemUnregistered = false;
    #endregion

    #region Properties
    public List<StateMachine> StateMachines { get => stateMachines; set => stateMachines = value; }
    public List<Entity> Entities { get => entities; set => entities = value; }
    public int InvaderNumber { get => invaderNumber; set => invaderNumber = value; }
    public int DefenderNumber { get => defenderNumber; set => defenderNumber = value; }
    #endregion

    #region Methods
    void Start()
    {
        instance = this;
        stateMachines = new List<StateMachine>();

        foreach (Entity entity in entities)
        {

            if (entity is Invader)
            {
                ++InvaderNumber;
                entity.name = $"Invader Number {InvaderNumber}";
            }
            else if (entity is Defender)
            {
                ++DefenderNumber;
                entity.name = $"Defender Number {DefenderNumber}";
            }
            else
                return;

            CreateStateMachine(entity);
        }
    }

    void Update()
    {
        for (int i = 0; i < StateMachines.Count; i++)
        {
            StateMachines[i]?.Evaluate();
        }

        if (itemUnregistered)
        {

            for (int i = StateMachines.Count - 1; i >= 0; i--)
            {
                if (StateMachines[i] == null)
                {
                    StateMachines.RemoveAt(i);
                    Entities.RemoveAt(i);
                }
            }
            itemUnregistered = false;
        }

        defenderHealthText.text = $"Defender Health: {defenderHealth.CurrentHealth}";
        defenderPopulationText.text = $"Defender Population: {defenderNumber}";
        invaderPopulationText.text = $"Invader Population: {invaderNumber}";

    }
    public void CreateStateMachine(Entity entity)
    {
        StateMachine stateMachine;

        if (entity is Invader)
            stateMachine = new InvaderStateMachine((Invader)entity);
        else if (entity is Defender)
            stateMachine = new DefenderStateMachine((Defender)entity);
        else
            return;

        stateMachines.Add(stateMachine);
    }
    public void RegisterEntity(Entity entity)
    {
        if (Entities.Contains(entity)) return;

        Entities.Add(entity);
        CreateStateMachine(entity);
    }

    public void UnregisterEntity(Entity entity)
    {
        if (!Entities.Contains(entity)) return;

        if (entity is Invader)
        {
            --InvaderNumber;

        }
        else if (entity is Defender)
        {
            --DefenderNumber;
        }

        int index = Entities.IndexOf(entity);

        Entities[index] = null;
        StateMachines[index] = null;
        itemUnregistered = true;
    }
    #endregion
}
