using MyClassLibrary.Reflection;
using System.Reflection;
using Xunit;

namespace UnitTestProject.Reflection
{

	public class PropertyTestClass
	{
		public int MyProperty { get; set; }

		public PropertyTestClass(int myProperty)
		{
			MyProperty = myProperty;
		}
	}

	public class FieldTestClass
	{
		public int myField;

		public FieldTestClass(int myField)
		{
			this.myField = myField;
		}
	}

	public class MethodTestClass
	{
		public void MyMethod()
		{

		}
	}

	public class PropertyRecursionTestClass
	{
		public PropertyTestClass MyProperty { get; set; }
		public PropertyRecursionTestClass RecursionTestClass { get; set; }

		public PropertyRecursionTestClass(PropertyTestClass myProperty, PropertyRecursionTestClass recursionTestClass)
		{
			MyProperty = myProperty;
			RecursionTestClass = recursionTestClass;
		}
	}

	public class ReflectionHelperTest
	{

		[Fact]
		public void TestProperty()
		{

			var obj = new PropertyTestClass(4);

			var memberInfo = ReflectionHelper.SearchValueInObject("4", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("MyProperty", memberInfo.Name);
			Assert.Equal("4", ((PropertyInfo)memberInfo).GetValue(obj).ToString());

			memberInfo = ReflectionHelper.SearchValueInObject("MyProperty", obj);
			Assert.NotNull(memberInfo);
			Assert.Equal("MyProperty", memberInfo.Name);
		}

		[Fact]
		public void TestField()
		{
			var obj = new FieldTestClass(4);

			var memberInfo = ReflectionHelper.SearchValueInObject("4", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("myField", memberInfo.Name);
			Assert.Equal("4", ((FieldInfo)memberInfo).GetValue(obj).ToString());

			memberInfo = ReflectionHelper.SearchValueInObject("myField", obj);
			Assert.NotNull(memberInfo);
			Assert.Equal("myField", memberInfo.Name);
		}

		[Fact]
		public void TestMethod()
		{
			var obj = new MethodTestClass();

			var memberInfo = ReflectionHelper.SearchValueInObject("MyMethod", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("MyMethod", memberInfo.Name);
		}

		[Fact]
		public void TestRecursion()
		{
			var obj = new PropertyRecursionTestClass(new PropertyTestClass(3), new PropertyRecursionTestClass(new PropertyTestClass(5), null));

			var memberInfo = ReflectionHelper.SearchValueInObject("MyProperty", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("MyProperty", memberInfo.Name);

			memberInfo = ReflectionHelper.SearchValueInObject("RecursionTestClass", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("RecursionTestClass", memberInfo.Name);
			Assert.Equal(typeof(PropertyRecursionTestClass), memberInfo.DeclaringType);

			memberInfo = ReflectionHelper.SearchValueInObject("5", obj);

			Assert.NotNull(memberInfo);
			Assert.Equal("MyProperty", memberInfo.Name);
			Assert.Equal(typeof(PropertyTestClass), memberInfo.DeclaringType);
		}

	}
}
