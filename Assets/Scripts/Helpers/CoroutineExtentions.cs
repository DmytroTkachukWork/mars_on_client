using System.Collections;

public static class CoroutineExtentions
{
  private static Tweener tweener = null;
  private static void init()
  {
    tweener = Service<Tweener>.get();
  }

  public static IEnumerator startCoroutineAndStopPrev( this IEnumerator curent_cor, IEnumerator new_cor )
  {
    init();
    if ( curent_cor != null )
      tweener.StopCoroutine( curent_cor );

    curent_cor = new_cor;

    if ( new_cor != null )
      tweener.StartCoroutine( new_cor );

    return new_cor;
  }

  public static void stop( this IEnumerator curent_cor )
  {
    init();
    if ( curent_cor != null )
      tweener.StopCoroutine( curent_cor );
  }

  public static void start( this IEnumerator curent_cor )
  {
    init();
    if ( curent_cor != null )
      tweener.StartCoroutine( curent_cor );
  }
}