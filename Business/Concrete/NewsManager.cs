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
    public class NewsManager :INewsService
    {
        INewsDal _newsDal;

        public NewsManager(INewsDal newsDal)
        {
            _newsDal = newsDal;
        }
        public IDataResult<List<News>> GetAll()
        {
            var result = _newsDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<News>>(result);
            }
            else
            {
                return new ErrorDataResult<List<News>>(message: Messages.NotListed);
            }


        }
    }
}
