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
    public interface INewsDetailService
    {
        IDataResult<List<NewsDetail>> GetAll();
        IDataResult<NewsDetail> Get(int id);
        IResult Delete(int id);
        IResult Update(NewsDetailUpdateDto newsDetailUpdateDto);
        IResult Add(NewsDetailUpdateDto newsDetailUpdateDto);
    }
}
