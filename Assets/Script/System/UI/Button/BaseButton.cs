using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Init();
    }

    public virtual void Init()
    {

    }

    public virtual void OnClick()
    {

    }
}
