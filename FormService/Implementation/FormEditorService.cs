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
    }
}
