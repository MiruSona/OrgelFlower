using UnityEngine;
using System.Collections;


// 싱글톤 : 씬에서 하나의 객체로만 사용될경우 이클래쓰를 상속받아서 사용
public abstract class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                try
                {
                    _instance = (T)FindObjectOfType<T>();
                }
                catch (System.Exception e)
                {
                    print("Error : " + e.StackTrace);
                    return null;
                }
                if (_instance == null)
                {
                    //Debug.LogError("싱글톤 에러 인스턴스가 존재하지 않음");
                }
            }
            return _instance;
        }
    }

    public static bool haveInstance()
    {
        if (_instance == null)
        {
            try
            {
                _instance = (T)FindObjectOfType<T>();
            }
            catch (System.Exception e)
            {
                print("Error : " + e.StackTrace);
                return false;
            }
        }
        return (_instance != null);
        
    }

}
