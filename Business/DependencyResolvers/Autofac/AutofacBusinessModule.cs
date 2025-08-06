using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Security.JWT;
using Entities.Concrete;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {


            builder.RegisterType<NavbarManager>().As<INavbarService>().SingleInstance();
            builder.RegisterType<EfNavbarDal>().As<INavbarDal>().SingleInstance();

            builder.RegisterType<BlogManager>().As<IBlogService>().SingleInstance();
            builder.RegisterType<EfBlogDal>().As<IBlogDal>().SingleInstance();

            builder.RegisterType<AboutUsManager>().As<IAboutUsService>().SingleInstance();
            builder.RegisterType<EfAboutUsDal>().As<IAboutUsDal>().SingleInstance();

            builder.RegisterType<SliderManager>().As<ISliderService>().SingleInstance();
            builder.RegisterType<EfSliderDal>().As<ISliderDal>().SingleInstance();

            builder.RegisterType<FooterManager>().As<IFooterService>().SingleInstance();
            builder.RegisterType<EfFooterDal>().As<IFooterDal>().SingleInstance();

            builder.RegisterType<NewsManager>().As<INewsService>().SingleInstance();
            builder.RegisterType<EfNewsDal>().As<INewsDal>().SingleInstance();

            builder.RegisterType<NewsDetailManager>().As<INewsDetailService>().SingleInstance();
            builder.RegisterType<EfNewsDetailDal>().As<INewsDetailDal>().SingleInstance();

            builder.RegisterType<FormsActiveManager>().As<IFormsActiveService>().SingleInstance();
            builder.RegisterType<EfFormsActiveDal>().As<IFormsActiveDal>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }


    }
}
