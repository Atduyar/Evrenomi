using Core.Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        public static string BlogAdded = "Blog Eklendi";
        public static string BlogUpdated = "Blog Güncellendi";
        public static string BlogDeleted = "Blog Silindi";
        public static string UserRegistered = "Kullanıcı Kayıt Edildi";
        public static string UserNotFound = "Kullanıcı Bulunamadı";
        public static string PasswordError = "Sifre Yanlıs";
        public static string SuccessfulLogin = "Giris Basarılı";
        public static string UserAlreadyExists = "Kullanıcı Zaten Kayıtlı";
        public static string AccessTokenCreated = "Access Token basarıyla olusturuldu";
        public static string UserOperationClaimAdded = "Kullanıcıya Rol Basarıyla Verildi";
        public static string UserOperationClaimDeleted = "Kullanıcıdan Rol Basarıyla Alındı";
        public static string OperationClaimNotFond = "Rol Bulunamadı";
        public static string OperationClaimAlreadyExist = "Kullanıcı Zaten Bu Role Sahip";
        public static string OperationClaimNotAvailable = "Kullanıcı Zaten Bu Role Sahip Değil";
    }
}