using UnityEngine;

// SCRIPT THAT DESTROYS THE OBJECT AFTER A SPECIFIED LENGTH OF TIME

public class DestroyAfterTime : MonoBehaviour {

    [SerializeField] private float timeToWait;

    private void Start()
    {
        Destroy(gameObject, timeToWait);
    }
}
