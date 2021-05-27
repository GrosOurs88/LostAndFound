using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public enum PlayerSpeed {Normal, Run, Count}
    [HideInInspector]
    public PlayerSpeed speedMode;

    [SerializeField]
    private float[] speedTable = new float[(int)PlayerSpeed.Count];
    private float speed;

    private PlayerMovementScript playerMovementScript;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();

        UpdateCurrentSpeed(PlayerSpeed.Normal);
    }

    public void UpdateCurrentSpeed(PlayerSpeed _playerSpeed)
    {
        speedMode = _playerSpeed;
        speed = speedTable[(int)speedMode];
    }

    void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * movX;
        Vector3 moveVertical = transform.forward * movZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        playerMovementScript.Move(velocity);
    }

    public bool CheckSpeedMode(PlayerSpeed _playerSpeed)
    {
        return speedMode == _playerSpeed;
    }
}
