namespace ReceiveMaker.Models
{
    public class ObjectIdModel : ReceiveTemplateModel
    {
        public string id { get; set; }


        public ReceiveTemplateModel GetTemplateModel()
        {
            ReceiveTemplateModel model = new ReceiveTemplateModel();

            model.Fields = this.Fields;
            model.Name = Name;
            model.RepeatingFields = RepeatingFields;
            model.templatePath = templatePath;
            model._id = new MongoDB.Bson.ObjectId(id);

            return model;
        }

    }
}
