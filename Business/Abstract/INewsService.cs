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
    public interface INewsService
    {
        IDataResult<List<News>> GetAll();
        IResult Delete(int id);
        IResult Update(NewsUpdateDto newsUpdateDto);
        IResult Add(NewsUpdateDto newsUpdateDto);
    }
}
