using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Estrutura para ser armazenada no memento.
/// Apesar de ter apenas uma variável inicialmente, já tem a estrutura adequada para armazenar mais informações.
/// </summary>
public class MementoState
{
    public Quaternion Rotation { get; private set; }
    public Vector3 Position { get; private set; }

    public MementoState(Quaternion rotation, Vector3 position)
    {
        Rotation = rotation;
        Position = position;
    }
}

/// <summary>
/// Armazena o histórico das ações.
/// </summary>
public class Memento
{
    private Stack<MementoState> m_states = new Stack<MementoState>();
    private int m_index;

    public bool IsEmpty { get { return m_states.Count == 0; } }
    
    /// <summary>
    /// Adciona um novo estado.
    /// </summary>
    public void PushState(MementoState newState)
    {
        m_states.Push(newState);
    }

    /// <summary>
    /// Recupera o último estado e o remove.
    /// </summary>
    public MementoState GetLastState()
    {
        return m_states.Pop();
    }

    /// <summary>
    /// Recupera todos os estados.
    /// </summary>
    public List<MementoState> GetStatesToReplay()
    {
        return m_states.Reverse().ToList();
    }

    /// <summary>
    /// Remove todos os estados.
    /// </summary>
    public MementoState ResetStates()
    {
        var firstState = m_states.Last();

        m_states.Clear();

        return firstState;
    }
}