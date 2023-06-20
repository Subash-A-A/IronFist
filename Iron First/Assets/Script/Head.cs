using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] Transform head;

    private void Update()
    {
        transform.rotation = head.rotation;
    }
}
