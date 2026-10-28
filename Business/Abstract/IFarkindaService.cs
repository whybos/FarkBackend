using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IFarkindaService
    {
        IDataResult<List<Farkinda>> GetAll();
        IDataResult<Farkinda> Get(int id);
        IResult Add(FarkindaUpdateDto farkindaUpdateDto);
        IResult Delete(int id);
        IResult Update(FarkindaUpdateDto farkindaUpdateDto);
    }
}
