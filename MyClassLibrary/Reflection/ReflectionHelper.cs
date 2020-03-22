using System;
using System.Reflection;

namespace MyClassLibrary.Reflection
{

	public static class ReflectionHelper
	{

		public static MemberInfo? SearchValueInObject(object value, object obj)
		{
			Type objType = obj.GetType();

			if (objType == typeof(string))
				return null;

			var properties = objType.GetProperties();

			foreach (var property in properties)
			{

				var tempObj = property.GetValue(obj);

				if (tempObj == null)
					continue;

				var temp = tempObj.ToString();

				if (Equals(value, temp) || Equals(value, property.Name))
					return property;

				if (temp.GetType() != typeof(object))
				{
					var result = SearchValueInObject(value, temp);
					if (result != null)
						return result;
				}

				if (!property.DeclaringType.Namespace.StartsWith("System"))
				{
					// search in depth
					var newObj = property.GetValue(obj);
					var result = SearchValueInObject(value, newObj);
					if (result != null)
						return result;
				}
			}

			var fields = objType.GetFields();

			foreach (var field in fields)
			{

				var temp = field.GetValue(obj).ToString();

				if (Equals(value, temp) || Equals(value, field.Name))
					return field;

				if (temp.GetType() != typeof(object))
				{
					var result = SearchValueInObject(value, temp);
					if (result != null)
						return result;
				}

				if (!field.DeclaringType.Namespace.StartsWith("System"))
				{
					// search in depth
					var newObj = field.GetValue(obj);
					var result = SearchValueInObject(value, newObj);
					if (result != null)
						return result;
				}
			}

			var methods = objType.GetMethods();

			foreach (var method in methods)
			{
				if (Equals(value, method.Name))
					return method;
			}

			return null;
		}

	}
}
