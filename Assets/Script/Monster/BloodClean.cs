using UnityEngine;

public class BloodParticleCleanup : MonoBehaviour
{
    // Function to clean up a blood particle
    public void Update()
    {
        Destroy(gameObject, 1f);
    }
}