using System;
using ProtoBuf;

namespace ProtoBufDataTemplate
{
    [ProtoContract]
    public class Item
    {
        //Protobuf also needs to associate data with keys so that it can identify data on serialization and deserialization. We do this with the ProtoMember attribute, and in that attribute be assign an integer value
        [ProtoMember(1)]
        public string name;
        [ProtoMember(2)]
        public int price;
        [ProtoMember(3)]
        public ItemType itemType;

        //then we can just make the class as normal
        public Item()
        {
            this.name = "Item";
            this.price = 10;
            this.itemType = ItemType.Hat;
        }

        public Item(string name, int price, ItemType currentItemType)
        {
            this.name = name;
            this.price = price;
            this.itemType = currentItemType;
        }
    }

    [ProtoContract]
    public enum ItemType
    {
        Hat = 0,
        Cape = 1,
        Weapon = 2,
        Shoe = 3,
        Arm_Armour = 4,
        Leg_Armour = 5,
        Shirt = 6,
    }
}
