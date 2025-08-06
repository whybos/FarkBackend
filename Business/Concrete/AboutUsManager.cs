using Business.Abstract;
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
    public class AboutUsManager : IAboutUsService
    {
        IAboutUsDal _aboutUsDal;

        public AboutUsManager(IAboutUsDal aboutUsDal)
        {
            _aboutUsDal = aboutUsDal;
        }
        public IDataResult<List<AboutUs>> GetAll()
        {
            var result = _aboutUsDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<AboutUs>>(result);
            }
            else
            {
                return new ErrorDataResult<List<AboutUs>>(message: Messages.NotListed);
            }


        }
    }
}
