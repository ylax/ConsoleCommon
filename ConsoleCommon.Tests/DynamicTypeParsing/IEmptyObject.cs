using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ConsoleCommon.Tests
{
    public interface IEmptyObject
    {
        IAfterProperty OverrideGet<T>(string propertyName, Expression<Func<T>> getBody);
        IAfterProperty OverrideSet<T>(string propertyName, Expression<Action<T>> setBody);
        ICreatingProperty AddProperty(string name, Type type);
        IAfterProperty AddAutoProperty(string name, Type type);
        ICreatingProperty AddProperty<T>(string name);
        IAfterProperty AddAutoProperty<T>(string name);
        IAfterProperty AddVoidMethod(string methodName, Expression<Action> methBody);
        IAfterProperty AddVoidMethod<T>(string methodName, Expression<Action<T>> methBody);
        IAfterProperty AddVoidMethod<T1, T2>(string methodName, Expression<Action<T1, T2>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3>(string methodName, Expression<Action<T1, T2, T3>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4>(string methodName, Expression<Action<T1, T2, T3, T4>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Action<T1, T2, T3, T4, T5>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody);
        IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody);
        IAfterProperty AddMethod<T>(string methodName, Expression<Func<T>> methBody);
        IAfterProperty AddMethod<T1, T2>(string methodName, Expression<Func<T1, T2>> methBody);
        IAfterProperty AddMethod<T1, T2, T3>(string methodName, Expression<Func<T1, T2, T3>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4>(string methodName, Expression<Func<T1, T2, T3, T4>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Func<T1, T2, T3, T4, T5>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody);
        IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>> methBody);

        IAfterProperty OverrideVoidMethod(string methodName, Expression<Action> methBody);
        IAfterProperty OverrideVoidMethod<T>(string methodName, Expression<Action<T>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2>(string methodName, Expression<Action<T1, T2>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3>(string methodName, Expression<Action<T1, T2, T3>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4>(string methodName, Expression<Action<T1, T2, T3, T4>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Action<T1, T2, T3, T4, T5>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody);
        IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody);
        IAfterProperty OverrideMethod<T>(string methodName, Expression<Func<T>> methBody);
        IAfterProperty OverrideMethod<T1, T2>(string methodName, Expression<Func<T1, T2>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3>(string methodName, Expression<Func<T1, T2, T3>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4>(string methodName, Expression<Func<T1, T2, T3, T4>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Func<T1, T2, T3, T4, T5>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody);
        IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>> methBody);
    }
}
