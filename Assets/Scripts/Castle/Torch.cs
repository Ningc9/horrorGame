using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject flameEffect; // ��������
    public bool isLit = false;     // �Ƿ��ѵ�ȼ

    public void Ignite()
    {
        if (isLit) return;

        isLit = true;

        if (flameEffect != null)
        {
            flameEffect.SetActive(true);
        }

        Debug.Log("����ȼ��");
    }
}
