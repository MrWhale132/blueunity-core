#nullable enable
using Codice.CM.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using Theblueway.Core.Runtime.Extensions;
using UnityEngine;

namespace Theblueway.Core.Runtime.TypeInterfaceScripts
{
    public abstract class TypeInterface<T> : TypeInterface { public T? instance; }

    public abstract class TypeInterface
    {

        public const bool Static = true;

        public static bool _attributeLookupIsBuilt => _interfaceTypeAttributeByHandledTypeLookup != null;
        public static Dictionary<long, TypeInterfaceInfo>? _interfaceTypeAttributeByHandledTypeLookup;
        public static Dictionary<long, TypeInterface> _interfaceInstanceByIdLookup = new();

        //=============================

        public Dictionary<int, TypeMember> _typeMembersByIndex = new();


        public abstract IEnumerable<TypeMember> GetMembers();

        public IEnumerable<TypeMember> GetTypeMembers()
        {
            return _typeMembersByIndex.Values;
        }

        public TypeMember? GetTypeMemberByIndex(int index)
        {
            if (_typeMembersByIndex.TryGetValue(index, out TypeMember typeMember))
            {
                return typeMember;
            }
            else
            {
                Debug.LogError($"No TypeMember found for index {index} in TypeInterface {this.GetType().CleanAssemblyQualifiedName()}");
                return null;
            }
        }

        //=====================

        public static IEnumerable<TypeInterfaceInfo> GetTypeInterfaceInfos()
        {
            EnsureLookUp();
            return _interfaceTypeAttributeByHandledTypeLookup!.Values;
        }

        public static TypeInterfaceInfo? GetTypeInterfaceInfo(long interfaceId)
        {
            EnsureLookUp();
            if (_interfaceTypeAttributeByHandledTypeLookup!.TryGetValue(interfaceId, out TypeInterfaceInfo info))
            {
                return info;
            }
            else
            {
                if (interfaceId != 0)
                    Debug.LogError($"No TypeInterfaceInfo found for Id {interfaceId}");
                return null;
            }
        }

        public static IEnumerable<TypeMember>? GetMembersOf(long interfaceId)
        {
            var instance = GetInstance(interfaceId);
            if (instance == null) return null;
            return instance.GetTypeMembers();
        }


        public static TypeMember? GetTypeMemberByIndex(long interfaceId, int index)
        {
            var instance = GetInstance(interfaceId);

            if (instance == null) return null;

            if (instance._typeMembersByIndex.TryGetValue(index, out TypeMember typeMember))
            {
                return typeMember;
            }
            else
            {
                Debug.LogError($"No TypeMember found for index {index} in TypeInterface {instance.GetType().CleanAssemblyQualifiedName()}");
                return null;
            }
        }




        public static TypeInterface GetInstance(long interfaceId)
        {
            EnsureLookUp();

            if (!_interfaceInstanceByIdLookup.ContainsKey(interfaceId))
            {
                var instance = CreateInstance(interfaceId);
                Setup(instance);
            }

            return _interfaceInstanceByIdLookup[interfaceId];
        }

        public static void Setup(TypeInterface instance)
        {
            var members = instance.GetMembers();

            foreach (var member in members)
            {
                if (instance._typeMembersByIndex.ContainsKey(member.Index))
                {
                    Debug.LogError($"Duplicate TypeMember index {member.Index} found in TypeInterface {instance._typeMembersByIndex}, {instance.GetType().CleanAssemblyQualifiedName()}. ");
                    continue;
                }

                instance._typeMembersByIndex.Add(member.Index, member);
            }
        }

        public static TypeInterface CreateInstance(long interfaceId)
        {
            Type interfaceType = _interfaceTypeAttributeByHandledTypeLookup![interfaceId].TypeInterfaceType;

            var instance = ObjectFactory.CreateInstance<TypeInterface>(interfaceType);

            _interfaceInstanceByIdLookup.Add(interfaceId, instance);

            return _interfaceInstanceByIdLookup[interfaceId];
        }



        public static void EnsureLookUp()
        {
            if (_attributeLookupIsBuilt) return;
            BuildLookup();
        }

        public static void BuildLookup()
        {
            if (_attributeLookupIsBuilt) return;
            _interfaceTypeAttributeByHandledTypeLookup = new();

            var types = AppDomain.CurrentDomain.GetUserTypes();

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<TypeInterfaceAttribute>();

                if (attr == null) continue;

                if (!type.IsAssignableTo(typeof(TypeInterface)))
                {
                    Debug.LogError($"Type {type.CleanAssemblyQualifiedName()} has {nameof(TypeInterfaceAttribute)} but does not implement {nameof(TypeInterface)}");
                    continue;
                }

                if (_interfaceTypeAttributeByHandledTypeLookup.ContainsKey(attr.Id))
                {
                    Debug.LogError($"Duplicate {nameof(TypeInterfaceAttribute)} found for Id {attr.Id} on type {type.CleanAssemblyQualifiedName()}. ");
                    continue;
                }

                var info = new TypeInterfaceInfo
                {
                    Id = attr.Id,
                    HandledType = attr.HandledType,
                    TypeInterfaceType = type
                };

                _interfaceTypeAttributeByHandledTypeLookup.Add(attr.Id, info);
            }
        }
    }



    public class TypeInterfaceInfo
    {
        public long Id { get; internal set; }
        public Type HandledType { get; internal set; } = null!;
        public Type TypeInterfaceType { get; internal set; } = null!;
    }
}
