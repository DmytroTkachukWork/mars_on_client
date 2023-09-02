using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BlurScreenshot : MonoBehaviourService<BlurScreenshot>
{
  #region Serialize Fields
  [SerializeField] private Material mat_grayscale_blur = null;
  [SerializeField] private Material mat_colored_blur   = null;
  #endregion

  #region Private Fields
  private const float SCREEN_MULTIPLIER = 0.5f;

  private bool          use_grayscale             = false;
  private RenderTexture screenshot_render_texture = null;
  #endregion


  #region Unity Methods
  private void OnRenderImage( RenderTexture source, RenderTexture destination )
  {
    UnityEngine.Graphics.Blit( source, screenshot_render_texture, use_grayscale ? mat_grayscale_blur : mat_colored_blur );
    UnityEngine.Graphics.Blit( source, destination, Vector2.one, Vector2.zero ); //rendering into the destination texture should be the last rendering operation
  }
  #endregion

  #region Public Methods
  public IEnumerator takeScreenshot( RawImage raw_image, bool use_grayscale = true )
  {
    this.use_grayscale = use_grayscale;

    screenshot_render_texture = raw_image.texture as RenderTexture;
    if ( screenshot_render_texture == null )
    {
      screenshot_render_texture = createRenderTexture();
      raw_image.texture = screenshot_render_texture;
    }

    this.enabled = true;

    yield return null;

    this.enabled = false;
  }
  #endregion

  #region Private Methods
  private RenderTexture createRenderTexture()
  {
    return new RenderTexture( (int)(Screen.width * SCREEN_MULTIPLIER), (int)(Screen.height * SCREEN_MULTIPLIER), 0 );
  }
  #endregion
}
