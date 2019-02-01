using System.Collections;
using UnityEngine;

// SCRIPT THAT WILL DEACTIVATE THE OBJECT AFTER A SPECIFIED TIME
// USING A COROUTINE

public class DeactivateAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToWait;

    private void Start()
    {
        StartCoroutine(_deactivate());
    }

    IEnumerator _deactivate()
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    }
}
