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
    public class NewsDetailManager : INewsDetailService
    {
        INewsDetailDal _newsDetailDal;

        public NewsDetailManager(INewsDetailDal newsDetailDal)
        {
            _newsDetailDal = newsDetailDal;
        }
        [SecuredOperation("superAdmin,admin")]
        public IResult Add(NewsDetailUpdateDto newsDetailUpdateDto)
        {
            var result = _newsDetailDal.Get(n => n.NewsId == newsDetailUpdateDto.NewsId);
            
            if(result != null)
            {
                return new ErrorResult("Is Already Existst");
            }

            NewsDetail newsDetail = new NewsDetail
            {
                NewsId= newsDetailUpdateDto.NewsId,
                Title = newsDetailUpdateDto.Title,
                Summary = newsDetailUpdateDto.Summary,
                Photo = newsDetailUpdateDto.Photo,
                Date = newsDetailUpdateDto.Date,
                Body = newsDetailUpdateDto.Body,
            };

            _newsDetailDal.Add(newsDetail);

            return new SuccessResult();
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Delete(int id)
        {
            var getResult = _newsDetailDal.Get(s => s.NewsId == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _newsDetailDal.Delete(getResult);

            var secondGetResult = _newsDetailDal.Get(s => s.NewsId == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

        }

        public IDataResult<NewsDetail> Get(int id)
        {
            var result = _newsDetailDal.Get(newsDetail => newsDetail.NewsId == id);

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
        [SecuredOperation("superAdmin,admin")]
        public IResult Update(NewsDetailUpdateDto newsDetailUpdateDto)
        {
            var getResult = _newsDetailDal.Get(s => s.Id == newsDetailUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = newsDetailUpdateDto.Title != null ? newsDetailUpdateDto.Title : getResult.Title;
            getResult.Body = newsDetailUpdateDto.Body != null ? newsDetailUpdateDto.Body : getResult.Body;
            getResult.Summary = newsDetailUpdateDto.Summary != null ? newsDetailUpdateDto.Summary : getResult.Summary;
            getResult.Photo = newsDetailUpdateDto.Photo != null ? newsDetailUpdateDto.Photo : getResult.Photo;
            getResult.Date = newsDetailUpdateDto.Date != null ? newsDetailUpdateDto.Date : getResult.Date;

            _newsDetailDal.Update(getResult);

            return new SuccessResult();
        }

    }
}