using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class Piece : MonoBehaviour
{
    public enum Team { White, Black }

    private Camera arCamera;
    private Rigidbody rb;
    private bool beingDragged;
    private int x, y;
    public Team team;
    public bool isKing = false;
    private GameManager gm;

    private void Start()
    {
        arCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();

            // Verificação de componentes obrigatórios
            if (GetComponent<Collider>() == null)
                Debug.LogError($"Piece '{gameObject.name}' precisa de um Collider para ser selecionada!");
            if (rb == null)
                Debug.LogError($"Piece '{gameObject.name}' precisa de um Rigidbody!");
            else
                rb.isKinematic = true; // padrão: não afetado pela física
    }

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int X => x;
    public int Y => y;

    void Update()
    {
        //Debug.Log($"[Piece] Update chamado para {gameObject.name}");
        if (beingDragged)
            Drag();

        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
        #endif
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        //Debug.Log($"[Piece] HandleTouchInput chamado para {gameObject.name}");
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        // TOQUE INICIOU
        if (touch.press.wasPressedThisFrame)
            TrySelect(touch.position.ReadValue());

        // TOQUE LIBERADO
        if (touch.press.wasReleasedThisFrame)
            Release();

    }

    // ===============================
    // SUPORTE AO MOUSE NO EDITOR
    // ===============================
    void HandleMouseInput()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            TrySelect(UnityEngine.Input.mousePosition);
        }
        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            Release();
        }
    }


    // ===============================
    // SELEÇÃO
    // ===============================
    void TrySelect(Vector2 touchPos)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log($"Selecionou a peça: {gameObject.name}");
                    beingDragged = true;
                    if (rb != null) rb.isKinematic = true;
                }
            }
            else
            {
                Debug.Log("Raycast não atingiu nada ao tentar selecionar a peça.");
            }
    }


    // ===============================
    // ARRASTAR
    // ===============================
    void Drag()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        if (UnityEngine.Input.GetMouseButton(0))
        {
            Vector3 mousePos = UnityEngine.Input.mousePosition;
            Ray ray = arCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                float hitDist = Vector3.Distance(arCamera.transform.position, hit.point);
                float targetDist = hitDist * 0.5f;
                Vector3 newPos = arCamera.transform.position + ray.direction.normalized;
                transform.position = newPos + Vector3.up * 0.01f;
            }
            return;
        }
        #endif
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.isPressed)
            {
                Vector3 touchPos = touch.position.ReadValue();
                Ray ray = arCamera.ScreenPointToRay(touchPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    float hitDist = Vector3.Distance(arCamera.transform.position, hit.point);
                    float targetDist = hitDist * 0.5f;
                    Vector3 newPos = arCamera.transform.position + ray.direction.normalized;
                    transform.position = newPos + Vector3.up * 0.01f;
                }
            }
        }
    }


    // ===============================
    // SOLTAR A PEÇA
    // ===============================
    void Release()
    {
        if (!beingDragged) return;

        beingDragged = false;

        if (rb != null) rb.isKinematic = false;

        // determine nearest cell indices
        Board board = FindObjectOfType<Board>();
        int targetX = x, targetY = y;
        float shortestDist = float.MaxValue;

        for (int i = 0; i < board.gridSize; i++)
        {
            for (int j = 0; j < board.gridSize; j++)
            {
                Vector3 cellPos = board.GetCellCenter(i, j);
                float d = Vector3.Distance(transform.position, cellPos);

                if (d < shortestDist)
                {
                    shortestDist = d;
                    targetX = i;
                    targetY = j;
                }
            }
        }

        // ask GameManager to perform/validate move
        if (gm != null)
        {
            bool moved = gm.TryMovePiece(this, targetX, targetY);
            if (!moved)
            {
                // invalid, snap back handled by GameManager but ensure position
                transform.position = board.GetCellCenter(x, y);
            }
        }
        else
        {
            // fallback: snap to nearest
            SnapToBoard();
        }
    }


    // ===============================
    // ENCAIXAR NA CASA MAIS PRÓXIMA
    // ===============================
    void SnapToBoard()
    {
        Board board = FindObjectOfType<Board>();

        float shortestDist = float.MaxValue;
        int bestX = x, bestY = y;

        for (int i = 0; i < board.gridSize; i++)
        {
            for (int j = 0; j < board.gridSize; j++)
            {
                Vector3 cellPos = board.GetCellCenter(i, j);
                float d = Vector3.Distance(transform.position, cellPos);

                if (d < shortestDist)
                {
                    shortestDist = d;
                    bestX = i;
                    bestY = j;
                }
            }
        }

        x = bestX;
        y = bestY;

        transform.position = board.GetCellCenter(x, y);
    }


    // ===============================
    // CAPTURA SIMPLES
    // ===============================
    // Capture and move handling is managed by GameManager
}
