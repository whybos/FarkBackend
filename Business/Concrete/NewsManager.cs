using Business.Abstract;
using Business.BusinessAspects.Autofac;
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

        [SecuredOperation("superAdmin,admin")]
        public IResult Add(NewsUpdateDto newsUpdateDto)
        {

            News news = new News
            {

                Title = newsUpdateDto.Title,
                Summary = newsUpdateDto.Summary,
                Photo = newsUpdateDto.Photo,
                Date = newsUpdateDto.Date,


            };

            _newsDal.Add(news);

            return new SuccessResult();
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Delete(int id)
        {
            var getResult = _newsDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _newsDal.Delete(getResult);

            var secondGetResult = _newsDal.Get(s => s.Id == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

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
        [SecuredOperation("superAdmin,admin")]
        public IResult Update(NewsUpdateDto newsUpdateDto)
        {
            var getResult = _newsDal.Get(s => s.Id == newsUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = newsUpdateDto.Title != null ? newsUpdateDto.Title : getResult.Title;
            getResult.Summary = newsUpdateDto.Summary != null ? newsUpdateDto.Summary : getResult.Summary;
            getResult.Photo = newsUpdateDto.Photo != null ? newsUpdateDto.Photo : getResult.Photo;
            getResult.Date = newsUpdateDto.Date != null ? newsUpdateDto.Date : getResult.Date;

            _newsDal.Update(getResult);

            return new SuccessResult();
        }

    }
}