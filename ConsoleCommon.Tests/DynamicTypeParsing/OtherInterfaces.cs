using System;
using System.Linq.Expressions;

namespace ConsoleCommon.Tests
{
    public interface IBaseObject
    {
        IEmptyObject AddPassThroughCtors();
    }

    public interface IAfterProperty : IEmptyObject, IFinishBuild
    {
        IAfterAttribute AddPropertyAttribute(Type attrType, Type[] ctorArgTypes, params object[] ctorArgs);
        IAfterAttribute AddPropertyAttribute<TAttr>(Type[] ctorArgTypes, params object[] ctorArgs);
    }
    public interface ICreatingProperty
    {
        IGetOrSetAdded AddAutoGet();
        IGetOrSetAdded AddAutoSet();
        IGetOrSetAdded AddGet<T>(Expression<Func<T>> methBody);
    }
    public interface IGetOrSetAdded : IAfterProperty, ICreatingProperty
    {

    }
    public interface IAfterAttribute : IEmptyObject, IFinishBuild
    {

    }
    public interface IFinishBuild
    {
        Type FinishBuildingType();
        Type FinishBuildingAndSaveType(string assemblyFileName);
    }
}
