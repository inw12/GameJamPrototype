using UnityEngine;
public class FootstepTrigger : MonoBehaviour
{
    public void PlayFootstepSFX()
    {
        AudioManager.Instance.PlaySFX("Footstep");
    }
}
