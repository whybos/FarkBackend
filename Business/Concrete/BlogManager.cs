using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BlogManager : IBlogService
    {
        IBlogDal _blogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }
        public IDataResult<List<Blog>> GetAll()
        {
            var result = _blogDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<Blog>>(result);
            }
            else
            {
                return new ErrorDataResult<List<Blog>>(message: Messages.NotListed);
            }


        }
    }
}
