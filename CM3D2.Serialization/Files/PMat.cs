using System;

namespace CM3D2.Serialization.Files;

public class PMat : ICM3D2Serializable, ISummarizable
{
	public readonly string signature = "CM3D2_PMATERIAL";
	//Unsure, unused
	public int version;

	public int hash;
	public string materialName;
	public float renderQueue;
	//Unused
	public string shader;

	public void WriteWith(ICM3D2Writer writer)
	{
		writer.Write(signature);

		writer.Write(version);
		writer.Write(hash);
		writer.Write(materialName);
		writer.Write(renderQueue);
		writer.Write(shader);
	}

	public void ReadWith(ICM3D2Reader reader)
	{
		reader.Read(out string temp_signature);
		if (temp_signature != signature)
			throw new FormatException($"Expected {nameof(signature)} \"{signature}\" but instead found \"{temp_signature}\"");

		reader.Read(out version);
		reader.Read(out hash);
		reader.Read(out materialName);
		reader.Read(out renderQueue);
		reader.Read(out shader);
	}

	public string Summarize()
	{
		return $"{{ {signature} v{version} \"{hash}\" {materialName} {renderQueue} {shader}}}";
	}
}