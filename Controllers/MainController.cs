using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using ReceiveMaker.Models;
using ReceiveMaker.Modules.DataIO;
using Newtonsoft.Json;
using ReceiveMaker.Modules.ReceiveCreator;

namespace ReceiveMaker.Controllers
{
    [ApiController]
    public class MainController : Controller
    {
        [Route("gettemplates")]
        [HttpPost]
        public List<string> GetReceiversTemplates()
        {
            return DataIO.GetTemplatesNames();

        }
        [Route("addtemplate")]
        [HttpPost]
        public bool AddTemplate([FromForm] LoadTemplateModel loadTemplate)
        {
            ReceiveTemplateModel receive = new ReceiveTemplateModel();

            if(loadTemplate.Fields != null)
            {
                receive.Fields = JsonConvert.DeserializeObject<List<FieldField>>(loadTemplate.Fields);
            }
            if (loadTemplate.RepeatingFields != null)
            {
                receive.RepeatingFields = JsonConvert.DeserializeObject<List<Item>>(loadTemplate.RepeatingFields);
            }

            receive.Name = loadTemplate.Name.Trim();
            if(loadTemplate.file != null)
            {
                receive.templatePath = ReceiveCreator.saveTemplate(loadTemplate.file, receive.Name);
            }
            try
            {
                DataIO.InsertTemplate(receive);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }


        }
        [Route("deleteTemplate")]
        [HttpPost]
        public bool DeleteTemplate([FromBody] ObjectIdModel id)
        {
            MongoDB.Bson.ObjectId _id = new MongoDB.Bson.ObjectId(id.id);
            return DataIO.DeleteTemplate(_id);

        }
        [Route("getTemplatesInfo")]
        [HttpPost]
        public List<TemplateIdentityModel> GetTemplateList()
        {

            return TemplateIdentityModel.GetIdentityModels(DataIO.GetTemplates());

        }
        [Route("getTemplate")]
        [HttpPost]
        public ReceiveTemplateModel GetTemplate([FromBody] ObjectIdModel id)
        {
            MongoDB.Bson.ObjectId _id = new MongoDB.Bson.ObjectId(id.id);
            return DataIO.GetTemplate(_id);

        }
        [Route("updateTemplate")]
        [HttpPost]
        public bool UpdateTemplate([FromForm] LoadTemplateModel loadTemplate)
        {
            ReceiveTemplateModel receive = DataIO.GetTemplate(new MongoDB.Bson.ObjectId(loadTemplate.id));

            if (loadTemplate.Fields != null)
            {
                receive.Fields = JsonConvert.DeserializeObject<List<FieldField>>(loadTemplate.Fields);
            }
            else
            {
                receive.Fields = new List<FieldField>();
            }

            if (loadTemplate.RepeatingFields != null)
            {
                receive.RepeatingFields = JsonConvert.DeserializeObject<List<Item>>(loadTemplate.RepeatingFields);
            }
            else
            {
                receive.RepeatingFields = new List<Item>();
            }

            if (loadTemplate.file != null)
            {
                receive.templatePath = ReceiveCreator.saveTemplate(loadTemplate.file, receive.Name);
            }
            try
            {
                DataIO.UpdateModel(receive);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        [Route("getReceive")]
        [HttpPost]
        public async Task<FileStreamResult> GetReceive([FromForm] string model1)
        {
            ReceiveTemplateModel model = JsonConvert.DeserializeObject<ReceiveTemplateModel>(model1);   

            MemoryStream memoryStream = ReceiveCreator.MakeReceive(model);

            return File(memoryStream, "application/msword");

            
        }


    }
}
