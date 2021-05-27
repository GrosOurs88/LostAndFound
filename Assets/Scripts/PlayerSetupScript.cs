using UnityEngine;
using Mirror;

public class PlayerSetupScript : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    Camera cameraWaitForConnection;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++) //Remove components if this player is not the one played by this PC
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            cameraWaitForConnection = Camera.main;
            if(cameraWaitForConnection != null)
            {
                cameraWaitForConnection.gameObject.SetActive(false);
            }   
        }
    }

    private void OnDisable()
    {
        if (cameraWaitForConnection != null)
        {
            cameraWaitForConnection.gameObject.SetActive(true);
        }
    }
}
