using UnityEngine;
using System.Collections;
using System.Reflection;
using FullSerializer;
using System;
using System.Collections.Generic;

public static class Util  {

	public static string Serialize(Type type, object value) {
		fsData data;
		(new fsSerializer()).TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();
		return fsJsonPrinter.CompressedJson(data);
		// or pretty json;
	}
	
	public static T Deserialize<T>(string serializedState) where T : class{
		fsData data = fsJsonParser.Parse(serializedState);
		Type type = typeof(T);
		object deserialized = null;
		(new fsSerializer()).TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();
		
		return deserialized as T;
	}

    public static object Deserialize(Type type, string serializedState)
    {
        fsData data = fsJsonParser.Parse(serializedState);
        object deserialized = null;
        (new fsSerializer()).TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized as object;
    }
}