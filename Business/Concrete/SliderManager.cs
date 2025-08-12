using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
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

        public IResult Add(SliderUpdateDto sliderUpdateDto)
        {

            Slider slider = new Slider
            {
                Title = sliderUpdateDto.Title,
                Body = sliderUpdateDto.Body,
                Photo = sliderUpdateDto.Photo
            };

            _sliderDal.Add(slider);

            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            var getResult = _sliderDal.Get(s => s.Id == id);
            if(getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _sliderDal.Delete(getResult);

            var secondGetResult  = _sliderDal.Get(s => s.Id == id);

            if(secondGetResult == null)
            {
                return new SuccessResult();

            }
            return new ErrorResult();

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

        public IResult Update(SliderUpdateDto sliderUpdateDto)
        {
            var getResult = _sliderDal.Get(s => s.Id == sliderUpdateDto.Id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = sliderUpdateDto.Title != null ? sliderUpdateDto.Title : getResult.Title;
            getResult.Photo = sliderUpdateDto.Photo != null ? sliderUpdateDto.Photo : getResult.Photo;
            getResult.Body = sliderUpdateDto.Body != null ? sliderUpdateDto.Body : getResult.Body;

            _sliderDal.Update(getResult);

            return new SuccessResult();
        }

    }
}
