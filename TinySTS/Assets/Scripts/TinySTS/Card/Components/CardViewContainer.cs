using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TinySTS
{
    public struct CardViewContainer : IComponentData
    {
        public float HSpacing;
        public float ZSpacing;
    }

    public struct CardViewElement : IBufferElementData, IEquatable<CardViewElement>
    {
        public Entity CardViewEntity;

        public bool Equals(CardViewElement other)
        {
            return CardViewEntity.Equals(other.CardViewEntity);
        }
    }

    public struct CardViewLayout : IComponentData
    {
        public float3 Position;
    }

    public struct AddCardViewCmdElement : IBufferElementData
    {
        public Entity CardViewEntity;
    }

    public struct CardViewElementChangedEvent : IComponentData
    {

    }
}
