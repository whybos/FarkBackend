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
    public class FormsActiveManager : IFormsActiveService
    {
        IFormsActiveDal _formsActiveDal;

        public FormsActiveManager(IFormsActiveDal formsActiveDal)
        {
            _formsActiveDal = formsActiveDal;
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
    }
}
