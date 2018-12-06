using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using ConsoleCommon.Parsing;
using ConsoleCommon.Parsing.TypeParsers;
using ConsoleCommon.Parsing.TypeParsers.Interfaces;

namespace ConsoleCommon.Tests
{
    public interface IEmptyParams
    {
        IAfterPropertyParams AddSwitch(string name, Type type, string switchName = "", bool required = false, int defaultOrdinal = -1, params string[] switchValues);
        IAfterPropertyParams AddSwitch<T>(string name, string switchName = "", bool required = false, int defaultOrdinal = -1, params string[] switchValues);
        IAfterPropertyParams OverrideTypeParsers(Expression<Func<ITypeParserContainer>> newTypeContainerExpr);
    }
    public interface IAfterPropertyParams : IEmptyParams, IFinishParams
    {
    }
    
    public interface IFinishParams
    {
        ParamsObject FinishBuilding(params string[] args);
        ParamsObject FinishBuilding(StringBuilder commandText);
    }
    public static class DynamicParamsCreator
    {
        public static IEmptyParams Create(string className = null)
        {
            string _className = string.IsNullOrWhiteSpace(className) ? "DynamicParams" : className;
            return new DynamicParamsCreatorBase().Create(_className);
        }
    }
    
    public class DynamicParamsCreatorBase : IEmptyParams, IAfterPropertyParams
    {
        DynamicTypeCreatorBase _creatorBase = new DynamicTypeCreatorBase();
        public IEmptyParams Create(string className)
        {
            _creatorBase
                    .Create(className, typeof(ParamsObject))
                    .AddPassThroughCtors();
                return this;
        }
        public IAfterPropertyParams AddSwitch<T>(string name, string switchName = "", bool required = false, int defaultOrdinal = -1, params string[] switchValues)
        {
            return AddSwitch(name, typeof(T), switchName, required, defaultOrdinal, switchValues);
        }
        public IAfterPropertyParams AddSwitch(string name, Type type, string switchName = "", bool required = false, int defaultOrdinal = -1, params string[] switchValues)
        {
            string _switchName = string.IsNullOrWhiteSpace(switchName) ? name : switchName;
            //string switchName = "", bool required = false, int defaultOrdinal = -1, string helpText = "", params string[] switchValues
            Type[] _argTypes = new Type[] { typeof(string), typeof(bool), typeof(int), typeof(string), typeof(string[]) };
            object[] _argVals = new object[] { _switchName, required, defaultOrdinal, "", switchValues };

            _creatorBase
                .AddAutoProperty(name, type)
                .AddPropertyAttribute(typeof(SwitchAttribute), _argTypes, _argVals);
            return this;
        }
        public IAfterPropertyParams OverrideTypeParsers(Expression<Func<ITypeParserContainer>> newTypeContainerExpr)
        {
            _creatorBase
                .OverrideGet<ITypeParserContainer>("TypeParser", newTypeContainerExpr);
            return this;
        }
        public ParamsObject FinishBuilding(params string[] args)
        {
            return (ParamsObject)
                _creatorBase
                .FinishBuildingType()
                .GetConstructor(new Type[] { typeof(string[]) })
                .Invoke(new object[] { args });
        }
        public ParamsObject FinishBuilding(StringBuilder commandText)
        {
            return (ParamsObject)
                _creatorBase
                .FinishBuildingType()
                .GetConstructor(new Type[] { typeof(string) })
                .Invoke(new object[] { commandText.ToString() });
        }
    }
}
