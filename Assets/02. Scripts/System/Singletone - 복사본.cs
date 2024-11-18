using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
                else
                {
                    MakeRootAndDontDestroy(instance.gameObject);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            MakeRootAndDontDestroy(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // 중복 인스턴스 파괴
        }
    }

    private static void MakeRootAndDontDestroy(GameObject obj)
    {
        if (obj.transform.parent != null)
        {
            obj.transform.parent = null;  // 최상위 계층으로 이동
        }
        DontDestroyOnLoad(obj);  // 최상위 GameObject에 대해 호출
    }
}
