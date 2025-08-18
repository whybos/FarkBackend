using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFormsActiveService
    {
        IDataResult<List<FormsActive>> GetAll();
        IResult Delete(int id);
        IResult Update(FormsActiveUpdateDto formsActiveUpdateDto);
        IResult Add(FormsActiveUpdateDto formsActiveUpdateDto);
    }
}
