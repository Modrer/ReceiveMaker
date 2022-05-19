using System.Collections.Generic;

namespace ReceiveMaker.Models
{
    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
 
        public Field(string name, string type, string key)
        {
            this.Name = name;
            this.Type = type;
            this.Key = key;
        }
    }
    public class FieldField : Field
    {
        public string Value { get; set; }
        public FieldField(string name, string type, string key) : base(name, type, key)
        {
        }
    }
    public class ItemField  : Field
    {
        public List<string> Values { get; set; }
        public ItemField(string name, string type, string key) : base(name, type, key)
        {

        }
    }
    public class Item
    {
        private List<ItemField> _fields;
        public string Name { get; set; }
        public string Key { get; set; }
        public List<ItemField> Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new List<ItemField>();
                }
                return _fields;
            }
            set
            { _fields = value; }
        }
       
    }

    public class ReceiveTemplateModel
    {
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string Name { get; set; }
        private List<Item> _repeatingFields;
        public List<Item> RepeatingFields
        {
            get
            {
                if (_repeatingFields == null)
                {
                    _repeatingFields = new List<Item>();
                }
                return _repeatingFields;
            }
            set { _repeatingFields = value; }
        }
        private List<FieldField> _fields;
        public List<FieldField> Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new List<FieldField>();
                }
                return _fields;
            }
            set
            { _fields = value; }
        }
        public string templatePath { get; set; }

    }
}
