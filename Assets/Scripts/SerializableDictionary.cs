using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


[Serializable]
public class SerializableDictionary : Dictionary<string, int>, ISerializable {
	public SerializableDictionary() {
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context) {
		info.AddValue ("_count", this.Count, typeof(int));

		int i = 0;
		foreach (var entry in this) {
			info.AddValue ("key_" + i, entry.Key);
			info.AddValue ("value_" + entry.Key, entry.Value, typeof(int));
			i++;
		}
	}

	public SerializableDictionary(SerializationInfo info, StreamingContext context) {
		var count = info.GetInt32("_count");

		for (var i = 0; i < count; i++) {
			var key = info.GetString ("key_" + i);
			var value = info.GetInt32 ("value_" + key);

			this.Add (key, value);
		}
	}
}

