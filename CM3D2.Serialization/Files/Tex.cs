using CM3D2.Serialization.Collections;
using CM3D2.Serialization.Types;
using System;

namespace CM3D2.Serialization.Files;

public class Tex : ICM3D2Serializable
{
	public readonly string signature = "CM3D2_TEX";

	public int version;

	public string unknown = string.Empty;

	[FileVersionConstraint(1011)]
	public int uvRectCount;

	[FileVersionConstraint(1011)]
	[LengthDefinedBy("uvRectCount")]
	public LengthDefinedArray<Float4> uvRects = new();

	[FileVersionConstraint(1010)]
	public int width;

	[FileVersionConstraint(1010)]
	public int height;

	[FileVersionConstraint(1010)]
	public int format;

	public int imageDataSize;

	[LengthDefinedBy("imageDataSize")]
	public LengthDefinedArray<byte> imageData = new();

	public void WriteWith(ICM3D2Writer writer)
	{
		writer.Write(signature);
		writer.Write(version);
		writer.Write(unknown);

		if (uvRectCount >= 0)
		{
			version = 1011;
			writer.Write(uvRectCount);
			uvRects.ValidateLength(uvRectCount, "uvRects", "uvRectCount");
			writer.Write(uvRects);
		}
		else
		{
			version = 1010;
		}

		writer.Write(width);
		writer.Write(height);
		writer.Write(format);
		writer.Write(imageDataSize);
		imageData.ValidateLength(imageDataSize, "imageData", "imageDataSize");
		writer.Write(imageData);
	}

	public void ReadWith(ICM3D2Reader reader)
	{
		reader.Read(out var text);
		if (text != signature)
		{
			throw new FormatException(string.Concat("Expected signature \"", signature, "\" but instead found \"", text, "\""));
		}

		reader.Read(out version);
		reader.Read(out unknown);

		if (version >= 1011)
		{
			reader.Read(out uvRectCount);
			uvRects.SetLength(uvRectCount);
			reader.Read(ref uvRects);
		}

		if (version >= 1010)
		{
			reader.Read(out width);
			reader.Read(out height);
			reader.Read(out format);
		}

		reader.Read(out imageDataSize);
		imageData.SetLength(imageDataSize);
		reader.Read(ref imageData);

		if (version == 1000)
		{
			if (imageData.Length >= 24)
			{
				width = (imageData[16] << 24) | (imageData[17] << 16) | (imageData[18] << 8) | imageData[19];
				height = (imageData[20] << 24) | (imageData[21] << 16) | (imageData[22] << 8) | imageData[23];
				format = 5;
			}
			else
			{
				throw new FormatException("imageData does not contain enough data to extract width and height for version 1000.");
			}
		}
	}
}