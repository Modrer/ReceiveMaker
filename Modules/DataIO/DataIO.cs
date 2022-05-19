using System;
using System.Collections.Generic;
using System.Linq;
using ReceiveMaker.Models;
using MongoDB.Driver;
using System.IO;

namespace ReceiveMaker.Modules.DataIO
{
    public static class DataIO
    {
        static MongoClient dbClient = new MongoClient();
        static IMongoDatabase database = dbClient.GetDatabase("kyrsova");
        static IMongoCollection<ReceiveTemplateModel> TemplateCollection = database.GetCollection<ReceiveTemplateModel>("templates");
        public static List<string> GetTemplatesNames()
        {
            List<string> names = new List<string>();

            var templates = TemplateCollection.Find(x => true).ToList();

            foreach (var template in templates)
            {
                names.Add(template.Name);
            }
            return names;
        }
        public static void UpdateModel(ReceiveTemplateModel model)
        {

            TemplateCollection.ReplaceOne(x => x._id == model._id,model);

        }
        public static ReceiveTemplateModel GetTemplate(string name)
        {
            return TemplateCollection.Find(x => x.Name == name).FirstOrDefault();
        }
        public static ReceiveTemplateModel GetTemplate(MongoDB.Bson.ObjectId id)
        {
            return TemplateCollection.Find(x => x._id == id).FirstOrDefault();
        }
        public static List<ReceiveTemplateModel> GetTemplates()
        {
            var t = TemplateCollection.Find(x => true);
            return t.ToList();
        }
        public static bool InsertTemplate(ReceiveTemplateModel templateModel)
        {
            try
            {
                TemplateCollection.InsertOne(templateModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }
        public static bool DeleteTemplate(string name)
        {
            try
            {

                var template = TemplateCollection.FindOneAndDelete(x => x.Name == name);
                File.Delete(template.templatePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }
        public static bool DeleteTemplate(MongoDB.Bson.ObjectId id)
        {
            try
            {
                var template = TemplateCollection.FindOneAndDelete(x => x._id == id);
                File.Delete(template.templatePath);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }
    }
}
