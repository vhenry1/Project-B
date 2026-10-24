using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Customization : MonoBehaviour
{
	private const string RedKey = "PlayerCustomization_Red";
	private const string GreenKey = "PlayerCustomization_Green";
	private const string BlueKey = "PlayerCustomization_Blue";
	private const string ShapeKey = "PlayerCustomization_Shape";

	private SpriteRenderer previewRenderer;
	private Sprite squareSprite;
	private Sprite circleSprite;

	private void Awake()
	{
		previewRenderer = GetComponent<SpriteRenderer>();
		squareSprite = previewRenderer.sprite;
		circleSprite = CreateCircleSprite(squareSprite);
		ApplySavedCustomization(previewRenderer, squareSprite, circleSprite);
	}

	private void OnDestroy()
	{
		if (circleSprite != null)
		{
			Destroy(circleSprite.texture);
			Destroy(circleSprite);
		}
	}

	public void SelectGreen()
	{
		SaveColor(Color.green);
	}

	public void SelectBlue()
	{
		SaveColor(Color.blue);
	}

	public void SelectRed()
	{
		SaveColor(Color.red);
	}

	public void SelectPink()
	{
		SaveColor(new Color(1f, 0.4f, 0.7f));
	}

	public void SelectSquare()
	{
		PlayerPrefs.SetInt(ShapeKey, 0);
		ApplySelection();
	}

	public void SelectCircle()
	{
		PlayerPrefs.SetInt(ShapeKey, 1);
		ApplySelection();
	}

	public static void ApplySavedCustomization(SpriteRenderer target, Sprite square, Sprite circle)
	{
		target.color = new Color(
			PlayerPrefs.GetFloat(RedKey, 1f),
			PlayerPrefs.GetFloat(GreenKey, 1f),
			PlayerPrefs.GetFloat(BlueKey, 1f),
			1f);
		target.sprite = PlayerPrefs.GetInt(ShapeKey, 0) == 1 ? circle : square;
	}

	public static Sprite CreateCircleSprite(Sprite referenceSprite)
	{
		const int textureSize = 64;
		Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Clamp
		};
		Color32[] pixels = new Color32[textureSize * textureSize];
		float center = (textureSize - 1) / 2f;
		float radius = textureSize / 2f;

		for (int y = 0; y < textureSize; y++)
		{
			for (int x = 0; x < textureSize; x++)
			{
				float distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
				byte alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01(radius - distance + 0.5f) * 255f);
				pixels[y * textureSize + x] = new Color32(255, 255, 255, alpha);
			}
		}

		texture.SetPixels32(pixels);
		texture.Apply();
		float pixelsPerUnit = textureSize / referenceSprite.bounds.size.x;
		return Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), pixelsPerUnit);
	}

	private void SaveColor(Color color)
	{
		PlayerPrefs.SetFloat(RedKey, color.r);
		PlayerPrefs.SetFloat(GreenKey, color.g);
		PlayerPrefs.SetFloat(BlueKey, color.b);
		ApplySelection();
	}

	private void ApplySelection()
	{
		PlayerPrefs.Save();
		ApplySavedCustomization(previewRenderer, squareSprite, circleSprite);
		PlayerController.RefreshCustomization();
	}

    public void BackToMenu()
    {
        // Load the Main Menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
