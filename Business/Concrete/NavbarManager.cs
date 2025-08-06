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
    public class NavbarManager : INavbarService
    {
        INavbarDal _navbarDal;

        public NavbarManager(INavbarDal navbarDal)
        {
            _navbarDal = navbarDal;
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
    }
}
