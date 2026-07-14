using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject Target {get; private set;}
    public bool IsTarget {get; private set;}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = other.gameObject;
            IsTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = null;
            IsTarget = false;
        }
    }
}
