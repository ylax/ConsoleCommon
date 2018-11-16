using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace ConsoleCommon.Tests
{
    public partial class DynamicTypeCreatorBase : IGetOrSetAdded
    {
        public ICreatingProperty AddProperty<T>(string name)
        {
            return AddProperty(name, typeof(T));
        }
        public ICreatingProperty AddProperty(string name, Type type)
        {
            PropertyBuilder _pBuilder = _tBuilder.DefineProperty(name, PropertyAttributes.SpecialName, CallingConventions.HasThis, type, Type.EmptyTypes);
            _propBuilds.Add(new PropertyBuilding { propertyBuilder = _pBuilder });
            return this;
        }
        public IAfterProperty AddAutoProperty<T>(string name)
        {
            return AddAutoProperty(name, typeof(T));
        }
        /// <summary>
        /// Adds a new property to type with specified name and type.
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <param name="type">Type of property</param>
        /// <returns></returns>
        public IAfterProperty AddAutoProperty(string name, Type type)
        {
            //base property
            AddProperty(name, type);
            AddAutoGet();
            AddAutoSet();
            return this;
        }
        public IGetOrSetAdded AddGet<T>(Expression<Func<T>> methBody)
        {
             //get last prop
             PropertyBuilding _pBuild = _propBuilds.Last();
            string _name = _pBuild.propertyBuilder.Name;
            Type _type = _pBuild.propertyBuilder.PropertyType;
            
            //static method, get body
            MethodAttributes _statAttrs = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Static;
            MethodBuilder _statMethBuilder = _tBuilder.DefineMethod($"stat_get_{_name}", _statAttrs);
            methBody.CompileToMethod(_statMethBuilder);

            //get method
            MethodAttributes _attrs = GetGetSetAttrs(_name);
            MethodBuilder _getBuilder = _tBuilder.DefineMethod($"get_{_name}", _attrs, CallingConventions.HasThis, methBody.ReturnType, Type.EmptyTypes);
            ILGenerator _mGen = _getBuilder.GetILGenerator();
            _mGen.Emit(OpCodes.Call, _statMethBuilder);
            _mGen.Emit(OpCodes.Ret);

            _pBuild.getBuilder = _getBuilder;
            return this;
        }
        public IGetOrSetAdded AddAutoGet()
        {
            PropertyBuilding _pBuild = _propBuilds.Last();
            string _name = _pBuild.propertyBuilder.Name;
            Type _type = _pBuild.propertyBuilder.PropertyType;

            //backing field
            FieldBuilder _fBuilder = getFieldBuilder(_pBuild);
            
            //get method
            MethodAttributes _propAttrs = GetGetSetAttrs(_name);
            MethodBuilder _getBuilder = _tBuilder.DefineMethod($"get_{_name}", _propAttrs, CallingConventions.HasThis, _type, Type.EmptyTypes);
            ILGenerator _getGen = _getBuilder.GetILGenerator();
            _getGen.Emit(OpCodes.Ldarg_0);
            _getGen.Emit(OpCodes.Ldfld, _fBuilder);
            _getGen.Emit(OpCodes.Ret);

            _pBuild.getBuilder = _getBuilder;
            return this;
        }
        public IGetOrSetAdded AddAutoSet()
        {
            PropertyBuilding _pBuild = _propBuilds.Last();
            string _name = _pBuild.propertyBuilder.Name;
            Type _type = _pBuild.propertyBuilder.PropertyType;

            //backing field
            FieldBuilder _fBuilder = getFieldBuilder(_pBuild);

            //set method
            MethodAttributes _propAttrs = GetGetSetAttrs(_name);
            MethodBuilder _setBuilder = _tBuilder.DefineMethod($"set_{_name}", _propAttrs, null, new Type[] { _type });
            ILGenerator _setGen = _setBuilder.GetILGenerator();
            _setGen.Emit(OpCodes.Ldarg_0);
            _setGen.Emit(OpCodes.Ldarg_1);
            _setGen.Emit(OpCodes.Stfld, _fBuilder);
            _setGen.Emit(OpCodes.Ret);

            _pBuild.setBuilder = _setBuilder;
            return this;
        }
        
        #region Helpers
        private MethodAttributes GetGetSetAttrs(string propName)
        {
            BindingFlags _bFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            MethodAttributes _attrs;
            //override
            if (_parentType.GetProperties(_bFlags).FirstOrDefault(p => p.Name == propName) != null)
            {
                _attrs = MethodAttributes.Public | MethodAttributes.HideBySig
                | MethodAttributes.SpecialName | MethodAttributes.Virtual
                | MethodAttributes.NewSlot | MethodAttributes.Final;
            }
            else
            {
                _attrs = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            }
            return _attrs;
        }
        private FieldBuilder getFieldBuilder(PropertyBuilding pBuilding)
        {
            //backing field
            string _name = pBuilding.propertyBuilder.Name;
            Type _type = pBuilding.propertyBuilder.PropertyType;

            FieldBuilder _fBuilder = pBuilding.backFieldBuilder;
            if (_fBuilder == null)
            {
                _fBuilder = _tBuilder.DefineField($"m_{_name}", _type, FieldAttributes.Private);
                pBuilding.backFieldBuilder = _fBuilder;
            }
            return _fBuilder;
        }
        #endregion
    }
}
