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
    public class FormsActiveManager : IFormsActiveService
    {
        IFormsActiveDal _formsActiveDal;

        public FormsActiveManager(IFormsActiveDal formsActiveDal)
        {
            _formsActiveDal = formsActiveDal;
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Add(FormsActiveUpdateDto formsActiveUpdateDto)
        {

            FormsActive formsActive = new FormsActive
            {

                Title = formsActiveUpdateDto.Title,
                Link = formsActiveUpdateDto.Link,


            };

            _formsActiveDal.Add(formsActive);

            return new SuccessResult();
        }
        [SecuredOperation("superAdmin,admin")]
        public IResult Delete(int id)
        {
            var getResult = _formsActiveDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _formsActiveDal.Delete(getResult);

            var secondGetResult = _formsActiveDal.Get(s => s.Id == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

        }

        public IDataResult<List<FormsActive>> GetAll()
        {
            var result = _formsActiveDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<FormsActive>>(result);
            }
            else
            {
                return new ErrorDataResult<List<FormsActive>>(message: Messages.NotListed);
            }
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Update(FormsActiveUpdateDto formsActiveUpdateDto)
        {
            var getResult = _formsActiveDal.Get(s => s.Id == formsActiveUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = formsActiveUpdateDto.Title != null ? formsActiveUpdateDto.Title : getResult.Title;
            getResult.Link = formsActiveUpdateDto.Link != null ? formsActiveUpdateDto.Link : getResult.Link;

            _formsActiveDal.Update(getResult);

            return new SuccessResult();
        }

    }
}
