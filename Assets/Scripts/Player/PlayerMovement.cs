using UnityEngine;
using Unity.Netcode;

// Controla el movimiento y salto del jugador en un contexto de red.
// El propietario local lee el input y envía los comandos al servidor.
public class PlayerMovement : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed; // Velocidad de desplazamiento del jugador.
    [SerializeField] float jumpForce; // Fuerza aplicada al salto.

    Rigidbody rb; // Componente para aplicar fuerzas.
    CapsuleCollider col; // Collider usado para comprobar si está en el suelo.

    public override void OnNetworkSpawn()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor para controlar mejor la cámara/movimiento.

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (!IsOwner) return; // Solo el jugador local procesa input.

        Vector2 moveInput = Vector2.zero;
        moveInput.x = Input.GetAxis("Horizontal") * speed; // Movimientos laterales.
        moveInput.y = Input.GetAxis("Vertical") * speed; // Movimientos hacia adelante/atrás.
        moveInput *= Time.deltaTime; // Ajusta la velocidad al tiempo entre frames.

        TranslateRpc(moveInput); // Envía el movimiento al servidor.

        if (Input.GetButtonDown("Jump"))
        {
            JumpRpc(); // Pide al servidor realizar el salto.
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Libera el cursor si el jugador presiona ESC.
        }
    }

    [Rpc(SendTo.Server)]
    void TranslateRpc(Vector2 moveInput)
    {
        transform.Translate(moveInput.x, 0, moveInput.y); // Mueve el jugador localmente en el servidor.
    }

    [Rpc(SendTo.Server)]
    void JumpRpc()
    {
        if (IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Aplica el salto solo si está en el suelo.
    }

    bool IsGrounded()
    {
        // Comprueba si el jugador está tocando el suelo con un raycast hacia abajo.
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}
