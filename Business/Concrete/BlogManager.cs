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
    public class BlogManager : IBlogService
    {
        IBlogDal _blogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }
        [SecuredOperation("superAdmin,admin")]
        public IResult Add(BlogUpdateDto blogUpdateDto)
        {

            Blog blog = new Blog
            {
                Title = blogUpdateDto.Title,
                Body = blogUpdateDto.Body,
                Speaker= blogUpdateDto.Speaker,
                Date = blogUpdateDto.Date,

            };

            _blogDal.Add(blog);

            return new SuccessResult();
        }
        [SecuredOperation("superAdmin,admin")]
        public IResult Delete(int id)
        {
            var getResult = _blogDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _blogDal.Delete(getResult);

            var secondGetResult = _blogDal.Get(s => s.Id == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

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
        [SecuredOperation("superAdmin,admin")]
        public IResult Update(BlogUpdateDto blogUpdateDto)
        {
            var getResult = _blogDal.Get(s => s.Id == blogUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = blogUpdateDto.Title != null ? blogUpdateDto.Title : getResult.Title;
            getResult.Body = blogUpdateDto.Body != null ? blogUpdateDto.Body : getResult.Body;
            getResult.Date = blogUpdateDto.Date != null ? blogUpdateDto.Date : getResult.Date;
            getResult.Speaker = blogUpdateDto.Speaker != null ? blogUpdateDto.Speaker : getResult.Speaker;

            _blogDal.Update(getResult);

            return new SuccessResult();
        }

    }
}
