using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Button m_movementButton;
    [SerializeField]
    private Button m_backButton;
    [SerializeField]
    private Button m_reconstituteButton;
    [SerializeField]
    private Button m_resetButton;
    [SerializeField]
    private Text m_messageText;

    private BallController m_ballController;

    void Start()
    {
        if (!m_movementButton || !m_backButton || !m_reconstituteButton || !m_resetButton)
        {
            Debug.LogError("Atenção, pelo menos um dos botões está nulo.");
        }

        PrepareButtons();

        if (!m_messageText)
        {
            Debug.LogError("Atenção, o texto de mensagens está nulo.");
        }

        m_messageText.text = null;

        m_ballController = FindObjectOfType<BallController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void PrepareButtons()
    {
        // Movimentar.
        m_movementButton.onClick.AddListener(MovementAction);
        // Voltar.
        m_backButton.onClick.AddListener(BackAction);
        // Reconstituir.
        m_reconstituteButton.onClick.AddListener(ReconstituteAction);
        // Reiniciar.
        m_resetButton.onClick.AddListener(ResetAction);
    }

    /// <summary>
    /// Ação do botão de movimentar.
    /// </summary>
    public void MovementAction()
    {
        m_messageText.text = "Movimentando para uma nova direção...";
        SetButtonsInteraction(false);

        m_ballController.Move(() =>
        {
            m_messageText.text = null;
            SetButtonsInteraction(true);
        });
    }

    /// <summary>
    /// Ação do botão de voltar.
    /// </summary>
    public void BackAction()
    {
        m_messageText.text = "Voltando para a posição anterior...";
        SetButtonsInteraction(false);

        m_ballController.Back(() =>
        {
            m_messageText.text = null;
            SetButtonsInteraction(true);
        });
    }

    /// <summary>
    /// Ação do botão de reconstituir.
    /// </summary>
    public void ReconstituteAction()
    {
        m_messageText.text = "Reconstituindo os movimentos já feitos...";
        SetButtonsInteraction(false);

        m_ballController.Reconstitute(() =>
        {
            m_messageText.text = null;
            SetButtonsInteraction(true);
        });
    }

    /// <summary>
    /// Ação do botão de reiniciar.
    /// </summary>
    public void ResetAction()
    {
        m_messageText.text = "Reiniciando o histórico dos movimentos...";
        SetButtonsInteraction(false);

        m_ballController.Reset(() =>
        {
            m_messageText.text = null;
            SetButtonsInteraction(true);
        });
    }

    /// <summary>
    /// Define se os botões estarão ativos.
    /// </summary>
    public void SetButtonsInteraction(bool canInteract)
    {
        m_movementButton.interactable = canInteract;
        m_backButton.interactable = canInteract;
        m_reconstituteButton.interactable = canInteract;
        m_resetButton.interactable = canInteract;
    }
}
