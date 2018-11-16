using System;
using System.Linq.Expressions;

namespace ConsoleCommon.Tests
{
    public partial class DynamicTypeCreatorBase
    {
        public IAfterProperty AddVoidMethod(string methodName, Expression<Action> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T>(string methodName, Expression<Action<T>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2>(string methodName, Expression<Action<T1, T2>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3>(string methodName, Expression<Action<T1, T2, T3>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4>(string methodName, Expression<Action<T1, T2, T3, T4>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Action<T1, T2, T3, T4, T5>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T>(string methodName, Expression<Func<T>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2>(string methodName, Expression<Func<T1, T2>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3>(string methodName, Expression<Func<T1, T2, T3>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4>(string methodName, Expression<Func<T1, T2, T3, T4>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Func<T1, T2, T3, T4, T5>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody)
        {
            return addMethod(methodName, methBody);
        }
        public IAfterProperty AddMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>> methBody)
        {
            return addMethod(methodName, methBody);
        }


        public IAfterProperty OverrideVoidMethod(string methodName, Expression<Action> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T>(string methodName, Expression<Action<T>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2>(string methodName, Expression<Action<T1, T2>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3>(string methodName, Expression<Action<T1, T2, T3>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4>(string methodName, Expression<Action<T1, T2, T3, T4>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Action<T1, T2, T3, T4, T5>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideVoidMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T>(string methodName, Expression<Func<T>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2>(string methodName, Expression<Func<T1, T2>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3>(string methodName, Expression<Func<T1, T2, T3>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4>(string methodName, Expression<Func<T1, T2, T3, T4>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5>(string methodName, Expression<Func<T1, T2, T3, T4, T5>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
        public IAfterProperty OverrideMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(string methodName, Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>> methBody)
        {
            return overrideMethod(methodName, methBody);
        }
    }
}
