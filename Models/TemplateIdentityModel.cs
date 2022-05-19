using System.Collections.Generic;

namespace ReceiveMaker.Models
{
    public class TemplateIdentityModel
    {
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string Name { get; set; }
        public TemplateIdentityModel(ReceiveTemplateModel receiveTemplate)
        {
            this.Name = receiveTemplate.Name;
            this._id = receiveTemplate._id;
        }
        public static List<TemplateIdentityModel> GetIdentityModels(List<ReceiveTemplateModel> templateModels)
        {
            List<TemplateIdentityModel> result = new List<TemplateIdentityModel>();
            foreach(var model in templateModels)
            {
                result.Add(new TemplateIdentityModel(model));
            }
            return result;
        }
    }
}
