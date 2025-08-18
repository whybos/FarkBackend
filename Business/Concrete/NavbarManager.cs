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
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class NavbarManager : INavbarService
    {
        INavbarDal _navbarDal;

        public NavbarManager(INavbarDal navbarDal)
        {
            _navbarDal = navbarDal;
        }

        public IResult Add(NavbarUpdateDto navbarUpdateDto)
        {

            Navbar navbar = new Navbar
            {
              
                Body = navbarUpdateDto.Body,
                Link = navbarUpdateDto.Link,
          

            };

            _navbarDal.Add(navbar);

            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            var getResult = _navbarDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _navbarDal.Delete(getResult);

            var secondGetResult = _navbarDal.Get(s => s.Id == id);

            if (secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

        }


        public IDataResult<List<Navbar>> GetAll()
        {
          var result = _navbarDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<Navbar>>(result);
            }
            else
            {
                return new ErrorDataResult<List<Navbar>>(message: Messages.NotListed);
            }


        }
        public IResult Update(NavbarUpdateDto navbarUpdateDto)
        {
            var getResult = _navbarDal.Get(s => s.Id == navbarUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Body = navbarUpdateDto.Body != null ? navbarUpdateDto.Body : getResult.Body;
            getResult.Link = navbarUpdateDto.Link != null ? navbarUpdateDto.Link : getResult.Link;

            _navbarDal.Update(getResult);

            return new SuccessResult();
        }

    }
}
