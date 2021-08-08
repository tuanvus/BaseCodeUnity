using System;

using UnityEngine;

public class Cropper : MonoBehaviour
{
	[SerializeField] private Color _backgroundColor;

	[SerializeField] private Texture2D _originalTexture;
	[SerializeField] private Texture2D _croppedTexture;

	private void Start()
	{
		_croppedTexture = Crop(_originalTexture);
	}

	private Texture2D Crop(Texture2D texture)
	{
		try
		{
			int x1 = GetTrimStartX(texture);
			int y1 = GetTrimStartY(texture);
			int x2 = GetTrimEndX(texture);
			int y2 = GetTrimEndY(texture);

			Texture2D croppedTexture = new Texture2D(x2 - x1, y2 - y1, texture.format, false, false)
			{
				name = $"{texture.name} Cropped",

				alphaIsTransparency = texture.alphaIsTransparency,
			};

			croppedTexture.SetPixels(0, 0, croppedTexture.width, croppedTexture.height, texture.GetPixels(x1, y1, croppedTexture.width, croppedTexture.height));

			croppedTexture.Apply();

			return croppedTexture;
		}
		catch (Exception exception)
		{
			Debug.LogError(exception.ToString());

			return null;
		}
	}

	private int GetTrimStartX(Texture2D texture)
	{
		for (int x = 0; x < texture.width; x++)
		{
			for (int y = 0; y < texture.height; y++)
			{
				if (texture.GetPixel(x, y) != _backgroundColor)
				{
					return x;
				}
			}
		}

		return -1;
	}

	private int GetTrimStartY(Texture2D texture)
	{
		for (int y = 0; y < texture.height; y++)
		{
			for (int x = 0; x < texture.width; x++)
			{
				if (texture.GetPixel(x, y) != _backgroundColor)
				{
					return y;
				}
			}
		}

		return -1;
	}

	private int GetTrimEndX(Texture2D texture)
	{
		for (int x = texture.width - 1; x > -1; x--)
		{
			for (int y = texture.height - 1; y > -1; y--)
			{
				if (texture.GetPixel(x, y) != _backgroundColor)
				{
					return x;
				}
			}
		}

		return -1;
	}

	private int GetTrimEndY(Texture2D texture)
	{
		for (int y = texture.height - 1; y > -1; y--)
		{
			for (int x = texture.width - 1; x > -1; x--)
			{
				if (texture.GetPixel(x, y) != _backgroundColor)
				{
					return y;
				}
			}
		}

		return -1;
	}
}