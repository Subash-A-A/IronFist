using UnityEngine;

public class Sign : MonoBehaviour
{
    private void Start()
    {
        HideSign();
    }
    public void HideSign()
    {
        gameObject.SetActive(false);
    }
}
