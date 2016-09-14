using System;
using ProtoBufDataTemplate;
using ProtoBuf.Meta;

namespace ProtoBufDataSerializerTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = TypeModel.Create();

            model.Add(typeof(ItemType), true);
            model.Add(typeof(Item), true);


            model.AllowParseableTypes = true;
            model.AutoAddMissingTypes = true;

            model.Compile("ProtoBufDataSerializerTemplate", "ProtoBufDataSerializerTemplate.dll");
        }
    }
}
