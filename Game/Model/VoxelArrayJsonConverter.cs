using System;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Game.Model
{
	public class VoxelArrayJsonConverter : JsonConverter
	{
		#region Overrides of JsonConverter

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var array = (VoxelArray)value;
			writer.WriteStartObject();
			writer.WritePropertyName("SizeX");
			serializer.Serialize(writer, array.SizeX);
			writer.WritePropertyName("SizeY");
			serializer.Serialize(writer, array.SizeY);
			writer.WritePropertyName("SizeZ");
			serializer.Serialize(writer, array.SizeZ);
			writer.WritePropertyName("Data");
			var bytes = array.ToArray();
			serializer.Serialize(writer, bytes);
			writer.WriteEndObject();
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
		/// <returns>
		/// The object value.
		/// </returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jsonObject = JObject.Load(reader);
			var properties = jsonObject.Properties().ToList();
			var sizex = (int)properties.First(x => x.Name == "SizeX");
			var sizeY = (int)properties.First(x => x.Name == "SizeY").Value;
			var sizeZ = (int)properties.First(x => x.Name == "SizeZ").Value;
			var Data = properties.First(x => x.Name == "Data");
			var voxelArray = new VoxelArray(sizex, sizeY, sizeZ);
			var v = (byte[])Data.Value;
			int index = 0;
			for (int x = 0; x < sizex; x++)
				for (int y = 0; y < sizeY; y++)
					for (int z = 0; z < sizeZ; z++)
					{
						voxelArray.FillBox(v[index],x,y,z,x,y,z);
						++index;
					}
			return voxelArray;
		}

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns>
		/// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvert(Type objectType)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}