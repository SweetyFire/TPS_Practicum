using UnityEngine;

public class FirstPersonTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ToggleFirstPerson(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        ToggleFirstPerson(other, false);
    }

    private static void ToggleFirstPerson(Collider other, bool enable)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.Skin.enabled = !enable;
            playerController.CameraController.ToggleFirstPerson(enable);
        }
    }
}
