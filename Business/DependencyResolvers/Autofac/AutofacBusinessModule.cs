using Autofac;
using Business.Abstract;
using Business.Concrete;
using Business.Helpers;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogManager>().As<IBlogService>();
            builder.RegisterType<EfBlogDal>().As<IBlogDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();

            builder.RegisterType<BlogManager>().As<IBlogService>();
            builder.RegisterType<EfBlogDal>().As<IBlogDal>();

            builder.RegisterType<BlogTagManager>().As<IBlogTagService>();
            builder.RegisterType<EfBlogTagDal>().As<IBlogTagDal>();

            builder.RegisterType<BlogEmojiManager>().As<IBlogEmojiService>();
            builder.RegisterType<EfBlogEmojiDal>().As<IBlogEmojiDal>();

            builder.RegisterType<BlogEmojiViewManager>().As<IBlogEmojiViewService>();
            builder.RegisterType<EfBlogEmojiViewDal>().As<IBlogEmojiViewDal>();

            builder.RegisterType<BlogEmojiViewViewManager>().As<IBlogEmojiViewViewService>();
            builder.RegisterType<EfBlogEmojiViewViewDal>().As<IBlogEmojiViewViewDal>();

            builder.RegisterType<BlogEmojiCountViewManager>().As<IBlogEmojiCountViewService>();
            builder.RegisterType<EfBlogEmojiCountViewDal>().As<IBlogEmojiCountViewDal>();

            builder.RegisterType<TagManager>().As<ITagService>();
            builder.RegisterType<EfTagDal>().As<ITagDal>();

            builder.RegisterType<BlogCommentManager>().As<IBlogCommentService>();
            builder.RegisterType<EfBlogCommentDal>().As<IBlogCommentDal>();

            builder.RegisterType<UserNotificationManager>().As<IUserNotificationService>();
            builder.RegisterType<EfUserNotificationsDal>().As<IUserNotificationDal>();

            builder.RegisterType<DateTimeBasicHelper>().As<IDateTimeHelper>();
        }
    }
}