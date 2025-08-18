using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        public IResult Add(AboutUsUpdateDto aboutUsUpdateDto)
        {

            AboutUs aboutUs = new AboutUs
            {
 
                Body = aboutUsUpdateDto.Body,
                Summary = aboutUsUpdateDto.Summary,
                Date = aboutUsUpdateDto.Date,

            };

            _aboutUsDal.Add(aboutUs);

            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            var getResult = _aboutUsDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _aboutUsDal.Delete(getResult);

            var secondGetResult = _aboutUsDal.Get(s => s.Id == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

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

        public IResult Update(AboutUsUpdateDto aboutUsUpdateDto)
        {
            var getResult = _aboutUsDal.Get(s => s.Id == aboutUsUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }
            getResult.Summary = aboutUsUpdateDto.Summary != null ? aboutUsUpdateDto.Summary : getResult.Summary;
            getResult.Body = aboutUsUpdateDto.Body != null ? aboutUsUpdateDto.Body : getResult.Body;
            getResult.Date = aboutUsUpdateDto.Date != null ? aboutUsUpdateDto.Date : getResult.Date;


            _aboutUsDal.Update(getResult);

            return new SuccessResult();
        }

    }
}
