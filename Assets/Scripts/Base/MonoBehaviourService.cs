using UnityEngine;

public class MonoBehaviourService<T> : MonoBehaviour where T : class
{
  protected void Awake()
  {
    Service<T>.register( this as T );
  }
}

public static class Service<T>
  where T : class
{
  private static T _instance = null;

  public static T get()
  {
    return _instance;
  }

  public static void register( T instance )
  {
    _instance = instance;
  }
}