using UnityEngine;

public class DestroyPickUp : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(DestroyIt), Random.Range(3f, 6f));
    }

    private void DestroyIt()
    {
        Destroy(gameObject);
    }

}
