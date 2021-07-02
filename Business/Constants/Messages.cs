using Core.Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        //Zamanlar
        public static string DateTimePassingYear = "yıl önce";
        public static string DateTimePassingMonths = "ay önce";
        public static string DateTimePassingDays = "gün önce";
        public static string DateTimePassingHours = "saat önce";
        public static string DateTimePassingMinutes = "dakika önce";
        public static string DateTimeNow = "simdi";

        //BURALARI DÜZENLE
        public static string BlogAdded = "Blog Eklendi";
        public static string BlogUpdated = "Blog Güncellendi";
        public static string BlogDeleted = "Blog Silindi";
        public static string BlogNotFound = "Blog bulunamadı";
        public static string BlogNotAccessible = "Blog erisebilir değil";
        public static string UserRegistered = "Kullanıcı Kayıt Edildi";
        public static string UserNotFound = "Kullanıcı Bulunamadı";
        public static string UserUpdateBan = "Kullanıcı profil bilgilerinizi değistiremessinz banlısınız";
        public static string UserMessageBan = "Kullanıcı Messaj atamazsınız banlısınız";
        public static string UserAuthorBan = "Kullanıcı Yazar olamazsınız banlısınız";
        public static string PasswordError = "Sifre Yanlıs";
        public static string SuccessfulLogin = "Giris Basarılı";
        public static string UserAlreadyExists = "Kullanıcı Zaten Kayıtlı";
        public static string AccessTokenCreated = "Access Token basarıyla olusturuldu";
        public static string UserOperationClaimAdded = "Kullanıcıya Rol Basarıyla Verildi";
        public static string UserOperationClaimDeleted = "Kullanıcıdan Rol Basarıyla Alındı";
        public static string OperationClaimNotFond = "Rol Bulunamadı";
        public static string OperationClaimAlreadyExist = "Kullanıcı Zaten Bu Role Sahip";
        public static string OperationClaimNotAvailable = "Kullanıcı Zaten Bu Role Sahip Değil";
        public static string AuthorAdded = "Yazar Eklendi";
        public static string NicknameUnchangeable = "Bu Nickname i kullanamazsınız";
        public static string EmailUnchangeable = "Bu Email i kullanamazsınız";
        public static string AuthorAlreadyExists = "Yazar zaten mevcut";
        public static string AuthorNotFound = "Yazar bulunamadı";
        public static string BlogCommentAdded = "Yorumunuz eklendi";
        public static string BlogCommentNotAccessible = "Yorum erisebilir değil";
        public static string BlogCommentUpdated = "Yorum güncellendi";
        public static string BlogCommentDeleted = "Yorum silindi";
        public static string BlogCommentNotFound = "Yorum bulunamadı";
        public static string BlogTagAdded = "BlogTag eklendi";
        public static string BlogTagUpdated = "BlogTag güncellendi";
        public static string BlogTagDeleted = "BlogTag silindi";
        public static string TagAdded = "Tag eklendi";
        public static string TagUpdated = "Tag güncellendi";
        public static string TagDeleted = "Tag silindi";
        public static string BlogEmojiAdded = "BlogEmoji eklendi eklendi";
        public static string BlogEmojiUpdated = "BlogEmoji eklendi güncellendi";
        public static string BlogEmojiDeleted = "BlogEmoji eklendi silindi";
        public static string UserNotificationAdded = "Notification eklendi";
        public static string UserNotificationUpdated = "Notification güncellendi";
        public static string UserNotificationDeleted = "Notification silindi";
    }
}