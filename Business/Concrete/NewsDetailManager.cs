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
    public class NewsDetailManager : INewsDetailService
    {
        INewsDetailDal _newsDetailDal;

        public NewsDetailManager(INewsDetailDal newsDetailDal)
        {
            _newsDetailDal = newsDetailDal;
        }

        public IDataResult<NewsDetail> Get(int id)
        {
            var result = _newsDetailDal.Get(newsDetail => newsDetail.Id == id);

            if(result != null)
            {
                return new SuccessDataResult<NewsDetail>(result);
            }
            else
            {
                return new ErrorDataResult<NewsDetail>(message: Messages.NotListed);

            }
        }

        public IDataResult<List<NewsDetail>> GetAll()
        {
            var result = _newsDetailDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<NewsDetail>>(result);
            }
            else
            {
                return new ErrorDataResult<List<NewsDetail>>(message: Messages.NotListed);
            }


        }
    }
}
