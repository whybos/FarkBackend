using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class FarkindaManager : IFarkindaService
    {
        private readonly IFarkindaDal _farkindaDal;

        public FarkindaManager(IFarkindaDal farkindaDal)
        {
            _farkindaDal = farkindaDal;
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Add(FarkindaUpdateDto farkindaUpdateDto)
        {
            if (farkindaUpdateDto == null)
            {
                return new ErrorResult("Geçersiz veri gönderildi");
            }

            Farkinda farkinda = new Farkinda
            {
                Title = farkindaUpdateDto.Title ?? string.Empty,
                Date = farkindaUpdateDto.Date ?? string.Empty,
                Link = NormalizeDriveLink(farkindaUpdateDto.Link),
                Photo = farkindaUpdateDto.Photo ?? string.Empty
            };

            _farkindaDal.Add(farkinda);
            return new SuccessResult(Messages.Added);
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Delete(int id)
        {
            var getResult = _farkindaDal.Get(s => s.Id == id);
            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            _farkindaDal.Delete(getResult);

            var secondGetResult = _farkindaDal.Get(s => s.Id == id);
            if (secondGetResult == null)
            {
                return new SuccessResult(Messages.Deleted);
            }

            return new ErrorResult(Messages.NotDeleted);
        }

        public IDataResult<Farkinda> Get(int id)
        {
            var result = _farkindaDal.Get(f => f.Id == id);
            if (result != null)
            {
                return new SuccessDataResult<Farkinda>(result, Messages.Listed);
            }
            return new ErrorDataResult<Farkinda>(Messages.NotListed);
        }

        public IDataResult<List<Farkinda>> GetAll()
        {
            var result = _farkindaDal.GetAll();
            if (result != null)
            {
                return new SuccessDataResult<List<Farkinda>>(result, Messages.Listed);
            }
            return new ErrorDataResult<List<Farkinda>>(message: Messages.NotListed);
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Update(FarkindaUpdateDto farkindaUpdateDto)
        {
            if (farkindaUpdateDto == null)
            {
                return new ErrorResult("Geçersiz veri gönderildi");
            }

            Farkinda getResult = null;
            if (farkindaUpdateDto.Id > 0)
            {
                getResult = _farkindaDal.Get(s => s.Id == farkindaUpdateDto.Id);
            }
            else
            {
                getResult = _farkindaDal.GetAll().FirstOrDefault();
            }

            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Title = farkindaUpdateDto.Title != null ? farkindaUpdateDto.Title : getResult.Title;
            getResult.Date = farkindaUpdateDto.Date != null ? farkindaUpdateDto.Date : getResult.Date;
            getResult.Link = farkindaUpdateDto.Link != null ? NormalizeDriveLink(farkindaUpdateDto.Link) : getResult.Link;
            getResult.Photo = farkindaUpdateDto.Photo != null ? farkindaUpdateDto.Photo : getResult.Photo;

            _farkindaDal.Update(getResult);
            return new SuccessResult(Messages.Updated);
        }

        private string NormalizeDriveLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return string.Empty;
            }

            var trimmed = link.Trim();
            if (!trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                trimmed = "https://" + trimmed;
            }

            return trimmed;
        }
    }
}
