using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.Shared;
using FormService.Infrastructure;
using Newtonsoft.Json;


namespace FormService.Implementation
{
    public class FormEditorService : IFormEditorService
    {
        private IFormRepository _formRepository;

        public FormEditorService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public int CreateForm(string schema, int formTypeId, int userId)
        {
            var form = new Form();
            form.CreatedBy = userId;
            form.Created = DateTime.Now;
            form.StatusId = (int)Variables.FormStatus.Draft;
            form.FormSchema = schema;
            form.FormTypeId = formTypeId;
            _formRepository.Add(form);
            _formRepository.Save();

            return form.Id;
        }

        public IEnumerable<Form> ListFormsOfType(int formTypeId)
        {
            return _formRepository.FindBy(x => x.FormTypeId == formTypeId);
        }

        public IEnumerable<ListItemStruct> ListFormsOfTypeSimple(int formTypeId)
        {
            return _formRepository.FindBy(x => x.FormTypeId == formTypeId).Select(y => new ListItemStruct{Id = y.Id, Name = y.Name});
        }

        public Form GetForm(int id)
        {
            return _formRepository.FindBy(x => x.Id == id).FirstOrDefault();
        }

        public ResultInfo SaveFormFields(int formId, string data)
        {
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(data);
            var temp = jObject["fields"];
            return null;
        }

        public ResultInfo SaveData(ContractFillViewInfo model)
        {
            var jObject = Newtonsoft.Json.Linq.JObject.Parse(model.Data);
            return null;
        }
    }
}
