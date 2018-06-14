using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class DynamicHandlerCompiler<T>
    {
        readonly Type _type;
        public DynamicHandlerCompiler()
        {
            this._type = typeof(T);
        }
        public DynamicHandlerCompiler(Type targetType)
        {
            this._type = targetType;
        }
        public DynamicHandlerCompiler(object target)
        {
            this._type = (target ?? new object()).GetType();
        }
        public Action<T, K, Type> CreaterSetPropertyHandler<K>(string propertyName, Type paramType = null)
        {
            var _type = typeof(T);
            var temp = typeof(Type);
            string methodName = $"set_{propertyName}";
            var callMethod = _type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
            var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(K), temp });

            var para = callMethod.GetParameters()[0];
            var targetTyppe = _type.BaseType == typeof(T) ? typeof(T) : _type;

            DynamicMethod methodBuilder = new DynamicMethod("EmitCallable", null, new Type[] { typeof(T), typeof(K), temp }, _type.Module);

            var il = methodBuilder.GetILGenerator();
            var NotNullLable = il.DefineLabel();

            var local = il.DeclareLocal(typeof(bool));
            var local2 = il.DeclareLocal(para.ParameterType);
            var local3 = il.DeclareLocal(typeof(Type));
            var local4 = il.DeclareLocal(typeof(Exception));
            var local5 = il.DeclareLocal(typeof(bool));

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
            il.Emit(OpCodes.Stloc, local);
            il.Emit(OpCodes.Ldloc, local);
            il.Emit(OpCodes.Brfalse_S, NotNullLable);
            var TryLable = il.BeginExceptionBlock();
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("get_IsEnum"));
            il.Emit(OpCodes.Stloc, local5);
            il.Emit(OpCodes.Ldloc, local5);
            var IF1 = il.DefineLabel();
            il.Emit(OpCodes.Brfalse_S, IF1);
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Ldarga_S, 1);
            il.Emit(OpCodes.Call, typeof(K).GetMethod("ToString", new Type[0]));
            il.Emit(OpCodes.Call, typeof(System.Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string) }));
            il.Emit(OpCodes.Unbox_Any, para.ParameterType);
            il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数
            il.Emit(OpCodes.Nop);

            var ELSE1 = il.DefineLabel();
            il.Emit(OpCodes.Br_S, ELSE1);
            il.MarkLabel(IF1);
            il.Emit(OpCodes.Ldtoken, typeof(K));
            il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("IsAssignableFrom"));
            il.Emit(OpCodes.Stloc, local5);
            il.Emit(OpCodes.Ldloc, local5);
            var IF2 = il.DefineLabel();

            il.Emit(OpCodes.Brfalse_S, IF2);
            il.Emit(OpCodes.Nop);


            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, callMethod, null);


            il.Emit(OpCodes.Br_S, ELSE1);
            il.MarkLabel(IF2);
            il.Emit(OpCodes.Nop);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Call, changeTypeMethod); //未支持装箱操作 值类型转换需增加装箱操作，现在有System.InvalidCastException异常

            if (para.ParameterType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, para.ParameterType);//  string = (string)object;
            }
            else
            {
                il.Emit(OpCodes.Castclass, para.ParameterType);//   Class = object as Class
            }
            il.Emit(OpCodes.Stloc, local2);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc, local2);
            il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数

            il.Emit(OpCodes.Nop);
            il.MarkLabel(ELSE1);
            il.Emit(OpCodes.Nop);

            il.BeginCatchBlock(typeof(System.Exception));
            il.Emit(OpCodes.Stloc, local4);
            il.Emit(OpCodes.Ldloc_3, local4);
            il.EmitCall(OpCodes.Callvirt, typeof(Exception).GetMethod("ToString"), null);//调用函数
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.EndExceptionBlock();
            il.Emit(OpCodes.Ret);

            il.MarkLabel(NotNullLable);
            il.Emit(OpCodes.Ret);


            var ca = typeof(Action<T, K, Type>);
            var a = methodBuilder.CreateDelegate(ca);
            return a as Action<T, K, Type>;
        }
        public Action<T, K, Type> CreaterMapperSetPropertyHandler<K>(string propertyName)
        {
            var _type = typeof(T);
            var temp = typeof(Type);
            string methodName = $"set_{propertyName}";
            var callMethod = _type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
            var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(K), temp });

            var para = callMethod.GetParameters()[0];
            var targetTyppe = _type.BaseType == typeof(T) ? typeof(T) : _type;
            DynamicMethod methodBuilder = new DynamicMethod("EmitCallable", null, new Type[] { typeof(T), typeof(K), temp }, _type.Module);

            var il = methodBuilder.GetILGenerator();
            var NotNullLable = il.DefineLabel();

            var local = il.DeclareLocal(typeof(bool));
            var local2 = il.DeclareLocal(para.ParameterType);

            var local4 = il.DeclareLocal(typeof(Exception));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Cgt_Un);
            il.Emit(OpCodes.Stloc, local);
            il.Emit(OpCodes.Ldloc, local);
            il.Emit(OpCodes.Brfalse_S, NotNullLable);
            var TryLable = il.BeginExceptionBlock();
            if (para.ParameterType.IsEnum)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Ldarga_S, 1);
                il.Emit(OpCodes.Call, typeof(K).GetMethod("ToString", new Type[0]));
                il.Emit(OpCodes.Call, typeof(System.Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string) }));
                il.Emit(OpCodes.Unbox_Any, para.ParameterType);
                il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数
            }
            else if (typeof(K).IsAssignableFrom(para.ParameterType))
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.EmitCall(OpCodes.Callvirt, callMethod, null);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Call, changeTypeMethod); //未支持装箱操作 值类型转换需增加装箱操作，现在有System.InvalidCastException异常

                if (para.ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, para.ParameterType);//  string = (string)object;
                }
                else
                {
                    il.Emit(OpCodes.Castclass, para.ParameterType);//   Class = object as Class
                }
                il.Emit(OpCodes.Stloc, local2);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldloc, local2);
                il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数
            }

            il.BeginCatchBlock(typeof(System.Exception));
            il.Emit(OpCodes.Stloc, local4);
            il.Emit(OpCodes.Ldloc, local4);
            il.EmitCall(OpCodes.Callvirt, typeof(Exception).GetMethod("ToString"), null);//调用函数
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.EndExceptionBlock();
            il.Emit(OpCodes.Ret);

            il.MarkLabel(NotNullLable);
            il.Emit(OpCodes.Ret);
            var ca = typeof(Action<T, K, Type>);
            var a = methodBuilder.CreateDelegate(ca);
            return a as Action<T, K, Type>;
        }

        public Func<T, K> CreaterGetPropertyHandler<K>(string propertyName, bool isEnum = false)
        {
            string methodName = $"get_{propertyName}";
            var callMethod = _type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public);
            var paramType = callMethod.ReturnType;
            var targetTyppe = this._type.BaseType == typeof(T) ? typeof(T) : this._type;
            DynamicMethod method = new DynamicMethod("EmitCallable2", typeof(K), new Type[] { typeof(T) }, this._type.Module);
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, callMethod, null);//调用函数
            if (paramType.IsValueType)
            {
                if (paramType.IsEnum && isEnum)
                    il.Emit(OpCodes.Box, typeof(Int32));
                else
                    il.Emit(OpCodes.Box, paramType);
            }
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<T, K>)) as Func<T, K>;
        }

        public Func<T> CreaterInstance(Type type)
        {
            DynamicMethod method = new DynamicMethod("GetInstance", this._type, null, this._type.Module);
            var il = method.GetILGenerator();
            var local = il.DeclareLocal(this._type);
            il.Emit(OpCodes.Newobj, type.GetConstructor(new Type[] { }));
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        public Func<object[], T> CreaterInstance(Type type, Type[] parameterTypes)
        {
            DynamicMethod method = new DynamicMethod("GetInstance", this._type, new Type[] { typeof(object[]) }, this._type.Module);
            var c = type.GetConstructor(parameterTypes);
            var il = method.GetILGenerator();
            var local = il.DeclareLocal(this._type);
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                if (parameterTypes[i].IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, parameterTypes[i]);//  string = (string)object;
                }
                else
                {
                    il.Emit(OpCodes.Castclass, parameterTypes[i]);//   Class = object as Class
                }
            }
            il.Emit(OpCodes.Newobj, c);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Func<object[], T>)) as Func<object[], T>;
        }

        public Type CreaterAnonEntity(string clssName, IEnumerable<CreaterDynamicClassProperty> Propertys)
        {
            var assemblyName = new AssemblyName("AnonEntity");
            // create assembly builder
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            // create module builder
            // var moduleBuilder = assemblyBuilder.DefineDynamicModule("AnonEntityModule", "Framework.dll");
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            // create type builder for a class
            var typeBuilder = moduleBuilder.DefineType(clssName, TypeAttributes.Public);

            var fields = (from t in Propertys select typeBuilder.DefineField(t.Name.ToLower(), t.Type, FieldAttributes.Private)).ToArray(); ;

            ConstructorInfo objCtor = typeof(object).GetConstructor(new Type[0]);

            var constructorArgs = from t in Propertys select t.Type;

            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, constructorArgs.ToArray());
            ILGenerator ilOfCtor = constructorBuilder.GetILGenerator();
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Call, objCtor);
            for (var i = 1; i < fields.Count() + 1; i++)
            {
                ilOfCtor.Emit(OpCodes.Ldarg_0);
                ilOfCtor.Emit(OpCodes.Ldarg_S, i);
                ilOfCtor.Emit(OpCodes.Stfld, fields[i - 1]);
            }
            ilOfCtor.Emit(OpCodes.Ret);
            for (var i = 0; i < fields.Count(); i++)
            {
                var methodGet = typeBuilder.DefineMethod($"get_{Propertys.ElementAt(i).Name}", MethodAttributes.Public, Propertys.ElementAt(i).Type, null);
                var ilOfGet = methodGet.GetILGenerator();
                ilOfGet.Emit(OpCodes.Ldarg_0); // this
                ilOfGet.Emit(OpCodes.Ldfld, fields[i]);
                ilOfGet.Emit(OpCodes.Ret);
                var propertyId = typeBuilder.DefineProperty(Propertys.ElementAt(i).Name, PropertyAttributes.None, Propertys.ElementAt(i).Type, null);
                propertyId.SetGetMethod(methodGet);
            }
            var classType = typeBuilder.CreateType();
            return classType;

        }

    }
    public class CreaterDynamicClassProperty
    {
        public string Name { get; set; }

        public Type Type { get; internal set; }

        public dynamic Value { get; internal set; }

        public CreaterDynamicClassProperty(string name, dynamic value)
        {
            this.Name = name;
            this.Type = value.GetType();
            this.Value = value;
        }
        public static object[] GetValues(IEnumerable<CreaterDynamicClassProperty> propertys)
        {
            return (from t in propertys select t.Value as object).ToArray();
        }
    }
    public enum DynamicHandlerType : uint
    {
        Set = 0,
        Get = 1,
    }
}
