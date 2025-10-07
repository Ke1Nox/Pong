using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

// Cambiar movimiento a arriba y abajo y sacar la superposicion de fuerzas y
// que la pelota colisione con los bordes de la pantalla y no con bordes con collider
public class SimplePlayer : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nicknameUI;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;

    private PhotonView photonView;
    private Rigidbody2D rb;

    private float horizontal;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        // Movimiento normal
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    // UI arriba del jugador (no rota ahora)
    void LateUpdate()
    {
        if (nicknameUI != null)
        {
            Vector3 offset = new Vector3(0, 1f, 0);
            nicknameUI.transform.position = transform.position + offset;
            nicknameUI.transform.rotation = Quaternion.identity;
        }
    }

    [PunRPC]
    public void RPC_SetNickname(string nickname)
    {
        if (nicknameUI != null) nicknameUI.text = nickname;
    }

}
