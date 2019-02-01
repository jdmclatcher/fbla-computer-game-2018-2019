using UnityEngine;

public class DropperItemController : MonoBehaviour {

    [SerializeField] private float moveSpeed;
	
	void Update () {

        // move downwards indefinetly
        transform.Translate(0, Time.deltaTime * -moveSpeed, 0);
	}
}
