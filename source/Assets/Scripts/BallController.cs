using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private bool m_enableVisualFeedback;
    [SerializeField]
    private GameObject m_pointPrefab;
    [Range(1f, 5f)]
    [SerializeField]
    private float m_movementSpeed = 2.5f;
    [Range(0.1f, 1f)]
    [SerializeField]
    private float m_actionDuration = 0.5f;

    private Memento m_memento = new Memento();
    private List<GameObject> m_feedbacks = new List<GameObject>();
    private int m_number = 0;

    void Start()
    {
        if (!m_pointPrefab)
        {
            if (m_enableVisualFeedback)
                Debug.LogError("Atenção, o prefab do feedback visual está nulo.");
            else
                Debug.LogWarning("Atenção, caso queira habilitar o feedback visual, adicione o prefab antes!");
        }

        InstatiateVisualFeedback();
    }

    /// <summary>
    /// Gera um valor para a rotação e executa o próximo movimento.
    /// </summary>
    public void Move(Action callback)
    {
        m_memento.PushState(GetCurrentMemento());

        var degree = Random.Range(-360f, 360f);

        ExecuteAction(transform.eulerAngles + new Vector3(0, degree, 0), transform.position + transform.forward * m_movementSpeed, () =>
        {
            InstatiateVisualFeedback();

            callback();
        });
    }

    /// <summary>
    /// Retorna para a posição anterior.
    /// </summary>
    public void Back(Action callback)
    {
        if (m_memento.IsEmpty)
        {
            callback();
            return;
        }

        MementoState lastState = m_memento.GetLastState();

        ExecuteAction(lastState.Rotation.eulerAngles, lastState.Position, () =>
        {
            RemoveLastFeedback();

            callback();
        });
    }

    /// <summary>
    /// Reconstitui todos os movimentos.
    /// </summary>
    public void Reconstitute(Action callback)
    {
        if (m_memento.IsEmpty)
        {
            callback();
            return;
        }

        m_memento.PushState(GetCurrentMemento());

        List<MementoState> allStates = m_memento.GetStatesToReplay();

        ReconstituteNextState(allStates, callback);
    }

    /// <summary>
    /// Reconstitui o próximo estado.
    /// </summary>
    private void ReconstituteNextState(List<MementoState> allStates, Action callback)
    {
        ExecuteAction(allStates[0].Rotation.eulerAngles, allStates[0].Position, () =>
        {
            allStates.RemoveAt(0);

            if (allStates.Count > 0)
            {
                ReconstituteNextState(allStates, callback);
            }
            else
            {
                m_memento.GetLastState();

                callback();
            }
        });
    }

    /// <summary>
    /// Retorna para a primeira posição e reinicia os movimentos.
    /// </summary>
    public void Reset(Action callback)
    {
        if (m_memento.IsEmpty)
        {
            callback();
            return;
        }

        MementoState firstState = m_memento.ResetStates();

        ExecuteAction(firstState.Rotation.eulerAngles, firstState.Position, () =>
        {
            ClearFeedbacks();

            callback();
        });
    }

    /// <summary>
    /// Cria um novo estado de acordo com a rotação e posição atual.
    /// </summary>
    private MementoState GetCurrentMemento()
    {
        return new MementoState(transform.rotation, transform.position);
    }

    /// <summary>
    /// Executa todas as ações comuns do jogo (rotação e translação).
    /// </summary>
    private void ExecuteAction(Vector3 rotationValue, Vector3 positionValue, Action callback)
    {
        transform.DORotate(rotationValue, m_actionDuration).OnComplete(() =>
        {
            transform.DOMove(positionValue, m_actionDuration).OnComplete(() =>
            {
                callback();
            });
        });
    }

    /// <summary>
    /// Instancia feedback visual, caso esteja habilitado.
    /// </summary>
    private void InstatiateVisualFeedback()
    {
        m_number++;

        if (!m_enableVisualFeedback) return;

        GameObject point = Instantiate(m_pointPrefab) as GameObject;
        point.transform.position = new Vector3(transform.position.x, -0.9f, transform.position.z);
        point.GetComponentInChildren<TextMesh>().text = (m_number).ToString();

        m_feedbacks.Add(point);
    }

    /// <summary>
    /// Remove último feedback, caso esteja habilitado.
    /// </summary>
    private void RemoveLastFeedback()
    {
        m_number--;

        if (!m_enableVisualFeedback) return;

        Destroy(m_feedbacks[m_number]);
        m_feedbacks.RemoveAt(m_number);
    }

    /// <summary>
    /// Remove último feedback, caso esteja habilitado.
    /// </summary>
    private void ClearFeedbacks()
    {
        m_number = 0;

        if (!m_enableVisualFeedback) return;

        for (int i = 0; i < m_feedbacks.Count; i++)
        {
            Destroy(m_feedbacks[i]);
        }
        m_feedbacks.Clear();
    }
}