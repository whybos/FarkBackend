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
    public interface IBlogService
    {
        IDataResult<List<Blog>> GetAll();
        IResult Delete(int id);
        IResult Update(BlogUpdateDto blogUpdateDto);
        IResult Add(BlogUpdateDto blogUpdateDto);
    }
}
