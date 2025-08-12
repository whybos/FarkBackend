using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FooterManager : IFooterService
    {
        IFooterDal _footerDal;

        public FooterManager(IFooterDal footerDal)
        {
            _footerDal = footerDal;
        }
        public IDataResult<List<Footer>> GetAll()
        {
            var result = _footerDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<Footer>>(result);
            }
            else
            {
                return new ErrorDataResult<List<Footer>>(message: Messages.NotListed);
            }


        }
    }
}
