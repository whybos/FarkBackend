using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SliderManager : ISliderService
    {
        ISliderDal _sliderDal;

        public SliderManager(ISliderDal sliderDal)
        {
            _sliderDal = sliderDal;
        }
        public IDataResult<List<Slider>> GetAll()
        {
            var result = _sliderDal.GetAll();

            if (result != null)
            {
                return new SuccessDataResult<List<Slider>>(result);
            }
            else
            {
                return new ErrorDataResult<List<Slider>>(message: Messages.NotListed);
            }


        }
    }
}
