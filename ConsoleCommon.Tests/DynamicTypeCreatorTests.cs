using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Reflection.Emit;
using ConsoleCommon;
using System.Diagnostics;
using System.Linq.Expressions;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;
using ConsoleCommon.Parsing.TypeParsers;
using ConsoleCommon.Parsing;

namespace ConsoleCommon.Tests
{
    public class Product
    {
        public virtual int MyAge { get { return 30; } }
        public string Name { get; set; }
        public Product ExtraProduct => new Product { Name = "Base Product" };
        public virtual Product VirtualProduct => new Product { Name = "Virtual Product" };
        public virtual Product GetVirtualProductMethod()
        {
            return new Product { Name = "Virtual Product From Method" };
        }
        public virtual Product GetVirtualProductMethodInput(Product input)
        {
            return new Product { Name = "Virtual Product From Method" };
        }
    }
    [TestFixture]
    public class DynamicTypeCreatorTests
    {
        [Test]
        public void TestTypeCreator()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product), @"C:\PROD")
                .AddPassThroughCtors()
                .AddAutoProperty("ProductName", typeof(string))
                .AddVoidMethod<string>("WriteStuff", (s) => Debug.WriteLine(s))
                .FinishBuildingAndSaveType("NewProduct.dll")
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            _newProduct.GetType().GetMethod("WriteStuff").Invoke(_newProduct, new object[] { "2nd ol" });
            //set ProductName value
            _newProduct.GetType().GetProperty("ProductName").SetValue(_newProduct, "Cool Item");

            //get ProductName value
            string _prodName = _newProduct.GetType().GetProperty("ProductName").GetValue(_newProduct).ToString();
            Assert.IsTrue(_prodName == "Cool Item");
        }
        [Test]
        public void TestAddVoidMethod_2()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("ProductName", typeof(string))
                .AddVoidMethod<string, int>("WriteStuff", (s, i) => Debug.WriteLine($"{s} {i * -1}"))
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            _newProduct.GetType().GetMethod("WriteStuff").Invoke(_newProduct, new object[] { "2nd ol", 5 });
        }
        [Test]
        public void TestAddMethod_1()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("ProductName", typeof(string))
                .AddMethod<string>("WriteStuff", () => $"{20 * -1}")
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            string _out =
                _newProduct.GetType().GetMethod("WriteStuff")
                .Invoke(_newProduct, new object[0] { })
                .ToString();

            Assert.IsTrue(_out == (-20).ToString());
        }
        [Test]
        public void TestAddMethod_2()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("ProductName", typeof(string))
                .AddMethod<int, string>("WriteStuff", (i) => $"{i * -1}")
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            string _out =
                _newProduct.GetType().GetMethod("WriteStuff")
                .Invoke(_newProduct, new object[] { 5 })
                .ToString();

            Assert.IsTrue(_out == (-5).ToString());
        }
        [Test]
        public void TestUseBaseProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("dude", typeof(string))
                //.AddMethod<int>("get_MyAge", () => 18)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            PropertyInfo _ageProp = _newProduct.GetType().GetProperty("MyAge");
            Assert.IsNotNull(_ageProp, "MyAge prop not found");
            int _out = (int)_ageProp.GetValue(_newProduct);
            Assert.IsTrue(_out == 30);
        }
        [Test]
        public void TestOverrideBaseProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("popo", typeof(string))
                .OverrideGet<int>("MyAge", () => 18)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            IEnumerable<PropertyInfo> _ageProps =
                _newProduct.GetType().GetProperties().Where(m => m.Name == "MyAge");
            Assert.IsFalse(_ageProps.Count() != 1, "more or less than one age prop appeared");
            int _age = (int)_ageProps.First().GetValue(_newProduct);
            Assert.IsTrue(_age == 18);

            _age = (int)_newProduct.GetType().GetMethod("get_MyAge").Invoke(_newProduct, null);
            Assert.IsTrue(_age == 18);
        }
        [Test]
        public void TestCombinedAutoProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddAutoProperty("popo", typeof(string))
                .AddAutoProperty("MyAge", typeof(int))
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            setPropVal(_newProduct, "popo", "Big Cop");
            Assert.IsTrue(getPropVal(_newProduct, "popo").ToString() == "Big Cop");
            Assert.IsTrue((int)getPropVal(_newProduct, "MyAge") == 0);
            setPropVal(_newProduct, "MyAge", 18);
            Assert.IsTrue((int)getPropVal(_newProduct, "MyAge") == 18);
        }
        [Test]
        public void TestCombinedGetProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create("NewProduct", typeof(Product))
                .AddPassThroughCtors()
                .AddProperty("popo", typeof(string))
                .AddGet<string>(() => "Big Cop")
                .AddProperty("MyAge", typeof(int))
                .AddGet<int>(() => 18)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            Assert.IsTrue(getPropVal(_newProduct, "popo").ToString() == "Big Cop");
            Assert.IsTrue((int)getPropVal(_newProduct, "MyAge") == 18);
            bool _smallCopError = false;
            try
            {
                setPropVal(_newProduct, "popo", "Small Cop");
            }
            catch { _smallCopError = true; }
            Assert.IsTrue(_smallCopError);

            bool _ageError = false;
            try
            {
                setPropVal(_newProduct, "MyAge", 28);
            }
            catch { _ageError = true; }
            Assert.IsTrue(_ageError);
        }
        [Test]
        public void TestCombinedGetProperty_Generics()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddProperty<string>("popo")
                .AddGet<string>(() => "Big Cop")
                .AddProperty<int>("MyAge")
                .AddGet<int>(() => 18)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            Assert.IsTrue(getPropVal(_newProduct, "popo").ToString() == "Big Cop");
            Assert.IsTrue((int)getPropVal(_newProduct, "MyAge") == 18);
        }
        [Test]
        public void TestAddSimpleMethod()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddMethod<int, int, int>("newMeth", (i, j) => i + j)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "newMeth");
            Assert.IsNotNull(_meth, "new method not found");
            int _add = (int)_meth.Invoke(_newProduct, new object[] { 5, 6 });
            Assert.IsTrue(_add == 11);
        }
        [Test]
        public void TestAddInComplexMethod()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddMethod<Product, int>("calcOldAge", (i) => 70 - i.MyAge)
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "calcOldAge");
            Assert.IsNotNull(_meth, "new method not found");
            int _old = (int)_meth.Invoke(_newProduct, new object[] { new Product() });
            Assert.IsTrue(_old == 40);
        }
        [Test]
        public void TestAddOutComplexMethod()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddMethod<string, Product>("newProd", (i) => new Product { Name = i })
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "newProd");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.Invoke(_newProduct, new object[] { "Yisrael" });
            Assert.IsTrue(_yis.Name == "Yisrael");
        }
        [Test]
        public void TestAddOutInComplexMethod()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddMethod<Product, Product>("newProd", (i) => new Product { Name = i.Name + " Johannan!" })
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "newProd");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = new Product() { Name = "Yisrael" };
            Product _newProd = (Product)_meth.Invoke(_newProduct, new object[] { _yis });
            Assert.IsTrue(_newProd.Name == "Yisrael Johannan!");
        }
        [Test]
        public void TestAddComplexProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<object>("NewProduct")
                .AddPassThroughCtors()
                .AddProperty<Product>("newProd")
                .AddGet<Product>(() => new Product { Name = "Yisrael Johannan!" })
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            PropertyInfo _meth = _newProduct.GetType().GetProperties().FirstOrDefault(m => m.Name == "newProd");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.GetValue(_newProduct);
            Assert.IsTrue(_yis.Name == "Yisrael Johannan!");
        }
        [Test]
        public void TestHideComplexProperty()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .AddProperty<Product>("ExtraProduct")
                .AddGet<Product>(() => new Product { Name = "Overriden property!" })
                .FinishBuildingType()
                .GetConstructor(new Type[0] { })
                .Invoke(new object[0] { });

            PropertyInfo _meth = _newProduct.GetType().GetProperties().FirstOrDefault(m => m.Name == "ExtraProduct");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.GetValue(_newProduct);
            Assert.IsTrue(_yis.Name == "Overriden property!");
        }
        [Test]
        public void TestOverrideVirtualComplexProperty()
        {
            string _prodName = "Overridden virtual property!";
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .OverrideGet<Product>("VirtualProduct", () => new Product { Name = "Overridden virtual property!" })
                .FinishBuildingType()
                .GetConstructor(Type.EmptyTypes)
                .Invoke(new object[0] { });

            PropertyInfo _meth = _newProduct.GetType().GetProperties().FirstOrDefault(m => m.Name == "VirtualProduct");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.GetValue(_newProduct);
            Assert.IsTrue(_yis.Name == _prodName);
        }
        [Test]
        public void TestOverrideVirtualComplexProperty_ParamsObject()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<ParamsObject>("NewParams")
                .AddPassThroughCtors()
                .OverrideGet<ITypeParserContainer>("TypeParser",
                () => new TypeParserContainer(false, new KeyValueParser()))
                .FinishBuildingType()
                .GetConstructor(new Type[] { typeof(string[]) })
                .Invoke(new object[] { new string[0] { } });

            PropertyInfo _meth = _newProduct.GetType().GetProperties().FirstOrDefault(m => m.Name == "TypeParser");
            Assert.IsNotNull(_meth, "new method not found");
            ITypeParserContainer _yis = (ITypeParserContainer)_meth.GetValue(_newProduct);
            bool _stringNotAccepted = false;
            try
            {
                _yis.GetParser(typeof(string));
            }
            catch
            {
                _stringNotAccepted = true;
            }
            Assert.IsTrue(_stringNotAccepted, "String parser found");
            bool _keyValueParserFound = true;
            try
            {
                _yis.GetParser(typeof(KeyValuePair<,>));
            }
            catch
            {
                _keyValueParserFound = false;
            }
            Assert.IsTrue(_keyValueParserFound, "Keyvalue parser not found");
        }
        [Test]
        public void TestMethodOverride()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .OverrideMethod<Product>("GetVirtualProductMethod", () => new Product { Name = "Overridden Method!" })
                .FinishBuildingType()
                .GetConstructor(Type.EmptyTypes)
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "GetVirtualProductMethod");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.Invoke(_newProduct, new object[0] { });
            Assert.IsTrue(_yis.Name == "Overridden Method!");
        }
        [Test]
        public void TestMethodOverrideInput()
        {
            //create and save new type
            object _newProduct = DynamicTypeCreator
                .Create<Product>("NewProduct")
                .AddPassThroughCtors()
                .OverrideMethod<Product,Product>("GetVirtualProductMethodInput", (p) => new Product { Name = $"Overridden Method! In Text: {p.Name}" })
                .FinishBuildingType()
                .GetConstructor(Type.EmptyTypes)
                .Invoke(new object[0] { });

            MethodInfo _meth = _newProduct.GetType().GetMethods().FirstOrDefault(m => m.Name == "GetVirtualProductMethodInput");
            Assert.IsNotNull(_meth, "new method not found");
            Product _yis = (Product)_meth.Invoke(_newProduct, new object[] { new Product { Name = "In Product!" } });
            Assert.IsTrue(_yis.Name == "Overridden Method! In Text: In Product!");
        }
        private object getPropVal(object obj, string propName)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(propName);
            return _prop.GetValue(obj);
        }
        private void setPropVal(object obj, string propName, object propVal)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(propName);
            _prop.SetValue(obj, propVal);
        }
    }
}
