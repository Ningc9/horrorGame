using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject flameEffect; // 火焰粒子
    public bool isLit = false;     // 是否已点燃

    public void Ignite()
    {
        if (isLit) return;

        isLit = true;

        if (flameEffect != null)
        {
            flameEffect.SetActive(true);
        }

        Debug.Log("火炬点燃！");
    }
}
