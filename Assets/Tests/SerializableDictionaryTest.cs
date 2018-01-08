using System;
using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class SerializableDictionaryTest
{
	[Test]
	public void SerializableDictionary_Serialize() {
		var dict = new SerializableDictionary ();
		dict.Add ("test", 123);

		var stream = new MemoryStream ();
		var bf = new BinaryFormatter ();

		// Serialize
		bf.Serialize (stream, dict);

		var bytes = stream.ToArray ();
		stream.Close ();

		var newStream = new MemoryStream (bytes);

		// Deserialize
		var newDict = bf.Deserialize (newStream) as SerializableDictionary;

		Assert.AreEqual (dict, newDict);
	}
}

