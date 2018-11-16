using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Linq.Expressions;

namespace ConsoleCommon.Tests
{
    public static class DynamicTypeCreator
    {
        public static IBaseObject Create<TParent>(string className)
        {
            return Create(className, typeof(TParent));
        }
        public static IBaseObject Create(string className, Type parentType)
        {
            return new DynamicTypeCreatorBase().Create(className, parentType);
        }
        public static IBaseObject Create<TParent>(string className, string dir)
        {
            return Create(className, typeof(TParent), dir);
        }
        public static IBaseObject Create(string className, Type parentType, string dir)
        {
            return new DynamicTypeCreatorBase().Create(className, parentType, dir);
        }
    }
    public partial class DynamicTypeCreatorBase : IBaseObject, IEmptyObject, IAfterProperty, IAfterAttribute
    {
        TypeBuilder _tBuilder;
        List<PropertyBuilding> _propBuilds = new List<PropertyBuilding>();
        AssemblyBuilder _aBuilder;
        Type _parentType;

        /// <summary>
        /// Begins creating type using the specified name.
        /// </summary>
        /// <param name="className">Class name for new type</param>
        /// <param name="parentType">Name of base class. Use object if none</param>
        /// <returns></returns>
        public IBaseObject Create(string className, Type parentType)
        {
            return Create(className, parentType, "");
        }
        /// <summary>
        /// Begins creating type using the specified name and saved in the specified directory.
        /// Use this overload to save the resulting .dll in a specified directory.
        /// </summary>
        /// <param name="className">Class name for new type</param>
        /// <param name="parentType">Name of base class. Use object if none</param>
        /// <param name="dir">Directory path to save .dll in</param>
        /// <returns></returns>
        public IBaseObject Create (string className, Type parentType, string dir)
        {
            _parentType = parentType;
            //Define type
            AssemblyName _name = new AssemblyName(className);
            if (string.IsNullOrWhiteSpace(dir))
            {
                _aBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_name, AssemblyBuilderAccess.RunAndSave);
            }
            else _aBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_name, AssemblyBuilderAccess.RunAndSave, dir);
            ModuleBuilder _mBuilder = _aBuilder.DefineDynamicModule(_name.Name, _name.Name + ".dll");
            _tBuilder = _mBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class, parentType);
            return this;
        }
        /// <summary>
        /// Adds constructors to new type that match all constructors on base type.
        /// Parameters are passed to base type.
        /// </summary>
        /// <returns></returns>
        public IEmptyObject AddPassThroughCtors()
        {
            foreach(ConstructorInfo _ctor in _parentType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                ParameterInfo[] _params = _ctor.GetParameters();
                Type[] _paramTypes = _params.Select(p => p.ParameterType).ToArray();
                Type[][] _reqModifiers = _params.Select(p => p.GetRequiredCustomModifiers()).ToArray();
                Type[][] _optModifiers = _params.Select(p => p.GetOptionalCustomModifiers()).ToArray();
                ConstructorBuilder _ctorBuild = _tBuilder.DefineConstructor(MethodAttributes.Public, _ctor.CallingConvention, _paramTypes, _reqModifiers, _optModifiers);
                for (int i = 0; i < _params.Length; i++)
                {
                    ParameterInfo _param = _params[i];
                    ParameterBuilder _prmBuild = _ctorBuild.DefineParameter(i + 1, _param.Attributes, _param.Name);
                    if (((int)_param.Attributes & (int)ParameterAttributes.HasDefault) != 0) _prmBuild.SetConstant(_param.RawDefaultValue);

                    foreach(CustomAttributeBuilder _attr in GetCustomAttrBuilders(_param.CustomAttributes))
                    {
                        _prmBuild.SetCustomAttribute(_attr);
                    }
                }

                //ConstructorBuilder _cBuilder = _tBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Any, argTypes);
                ILGenerator _ctorGen = _ctorBuild.GetILGenerator();
                _ctorGen.Emit(OpCodes.Nop);
                //arg0=new obj, arg1-arg3=passed params. Push onto stack for call to base class
                _ctorGen.Emit(OpCodes.Ldarg_0);
                for (int i = 1; i <= _params.Length; i++) _ctorGen.Emit(OpCodes.Ldarg, i);
                _ctorGen.Emit(OpCodes.Call, _ctor);
                _ctorGen.Emit(OpCodes.Ret);
            }
            return this;
        }

        public IAfterAttribute AddPropertyAttribute<TAttr>(Type[] ctorArgTypes, params object[] ctorArgs)
        {
            return AddPropertyAttribute(typeof(TAttr), ctorArgTypes, ctorArgs);
        }
        /// <summary>
        /// Adds an attribute to a property just added.
        /// </summary>
        /// <param name="attrType">Type of attribute</param>
        /// <param name="ctorArgTypes">Types of attribute's cstor parameters</param>
        /// <param name="ctorArgs">Values to pass in to attribute's cstor. Must match in type and order of cstorArgTypes parameter</param>
        /// <returns></returns>
        public IAfterAttribute AddPropertyAttribute(Type attrType, Type[] ctorArgTypes, params object[] ctorArgs)
        {
            if (ctorArgTypes.Length != ctorArgs.Length) throw new Exception("Type count must match arg count for attribute specification");
            ConstructorInfo _attrCtor = attrType.GetConstructor(ctorArgTypes);
            for (int i = 0; i < ctorArgTypes.Length; i++)
            {
                CustomAttributeBuilder _attrBuild = new CustomAttributeBuilder(_attrCtor, ctorArgs);
                _propBuilds.Last().propertyBuilder.SetCustomAttribute(_attrBuild);
            }
            return this;
        }
        /// <summary>
        /// Completes building type, compiles it, and returns the resulting type
        /// </summary>
        /// <returns></returns>
        public Type FinishBuildingType()
        {
            foreach(var _pBuilder in _propBuilds)
            {
                if (_pBuilder.getBuilder != null) _pBuilder.propertyBuilder.SetGetMethod(_pBuilder.getBuilder);
                if (_pBuilder.setBuilder != null) _pBuilder.propertyBuilder.SetSetMethod(_pBuilder.setBuilder);
            }
            
            Type _paramsType = _tBuilder.CreateType();
            return _paramsType;
        }
        /// <summary>
        /// Completes building type, compiles it, saves it, and returns the resultying type.
        /// Assembly is saved in the calling assembly's directory or in the dir specified in the Create method.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly</param>
        /// <returns></returns>
        public Type FinishBuildingAndSaveType(string assemblyFileName)
        {
            Type _newType = FinishBuildingType();
            Save(assemblyFileName);
            return _newType;
        }
        #region Helpers
        private IAfterProperty addMethod(string methodName, LambdaExpression methBody, MethodAttributes methAttrs = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Static)
        {
            MethodBuilder _mb = _tBuilder.DefineMethod(methodName, methAttrs);
            methBody.CompileToMethod(_mb);

            return this;
        }
        private IAfterProperty overrideMethod(string methodName, LambdaExpression methBody, MethodAttributes methAttrs = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Static)
        {
            Type[] _paramTypes = methBody.Parameters.Select(p => p.Type).ToArray();
            MethodInfo _methToOverride = getMethodToOverride(methodName, _paramTypes);
            bool _isOverride = _methToOverride!=null;
            if (!_isOverride) throw new Exception($"Method '{methodName}' not found");

            //stat method
            MethodAttributes _statAttrs = MethodAttributes.Private | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Static;
            MethodBuilder _statMBuild = _tBuilder.DefineMethod($"stat_{methodName}", _statAttrs);
            methBody.CompileToMethod(_statMBuild);

            //non stat method
            _statAttrs = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.Virtual;
            MethodBuilder _mBuild = _tBuilder.DefineMethod(
                methodName,
                _statAttrs,
                CallingConventions.HasThis,
                _statMBuild.ReturnType,
                _paramTypes
                );
            ILGenerator _mGen = _mBuild.GetILGenerator();
            //_mGen.Emit(OpCodes.Ldarg_0);
            for (int i = 1; i <= _paramTypes.Length; i++) _mGen.Emit(OpCodes.Ldarg, i);
            _mGen.Emit(OpCodes.Call, _statMBuild);
            _mGen.Emit(OpCodes.Ret);

            _tBuilder.DefineMethodOverride(_mBuild, _methToOverride);

            return this;
        }
        private CustomAttributeBuilder[] GetCustomAttrBuilders(IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.Select(attribute => {
                object[] attributeArgs = attribute.ConstructorArguments.Select(a => a.Value).ToArray();
                PropertyInfo[] namedPropertyInfos = attribute.NamedArguments.Select(a => a.MemberInfo).OfType<PropertyInfo>().ToArray();
                object[] namedPropertyValues = attribute.NamedArguments.Where(a => a.MemberInfo is PropertyInfo).Select(a => a.TypedValue.Value).ToArray();
                FieldInfo[] namedFieldInfos = attribute.NamedArguments.Select(a => a.MemberInfo).OfType<FieldInfo>().ToArray();
                object[] namedFieldValues = attribute.NamedArguments.Where(a => a.MemberInfo is FieldInfo).Select(a => a.TypedValue.Value).ToArray();
                return new CustomAttributeBuilder(attribute.Constructor, attributeArgs, namedPropertyInfos, namedPropertyValues, namedFieldInfos, namedFieldValues);
            }).ToArray();
        }
        /// <summary>
        /// Requires admin privileges. 
        /// To save in a specified dir, use the Create overload that requires a 'dir' parameter.
        /// </summary>
        /// <param name="assemblyFileName"></param>
        private void Save(string assemblyFileName)
        {
            string _assemblyName = assemblyFileName;
            if (!Path.HasExtension(assemblyFileName) || Path.GetExtension(assemblyFileName).ToLower() != ".dll")
                _assemblyName += ".dll";
            _aBuilder.Save(_assemblyName);
        }
        private MethodInfo getMethodToOverride(string methName, Type[] parameterTypes)
        {
            BindingFlags _bFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            MethodInfo _matchMeth = 
                _parentType.GetMethods(_bFlags)
                .Where(m =>
                m.Name == methName &&
                m.GetParameters()
                    .Select(p => p.ParameterType)
                    .SequenceEqual(parameterTypes))
                .FirstOrDefault();
            return _matchMeth;
        }

        public IAfterProperty OverrideGet<T>(string propertyName, Expression<Func<T>> getBody)
        {
            return OverrideMethod<T>($"get_{propertyName}", getBody);
        }

        public IAfterProperty OverrideSet<T>(string propertyName, Expression<Action<T>> setBody)
        {
            return OverrideVoidMethod<T>($"set_{propertyName}", setBody);
        }
        #endregion
    }
}
